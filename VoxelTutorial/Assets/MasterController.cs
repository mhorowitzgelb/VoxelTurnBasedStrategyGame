using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterController : MonoBehaviour {

	[SerializeField] public List<Team> teams = new List<Team> ();
	public int currentPlayerTurn = 0;
	public World world;
	public ModifyTerrain modifyTerrain;
	private GameObject pieceTakingTurn;
	private bool takingTurn = false;
	private int maxFallHeight = 3;
	public GameObject basicPiece;
	public Team currentTeam;
	public int currentTeamNum = 0;
	void Start(){
		//Set starting team as team 0
		currentTeam = teams [0];
		world = GetComponent<World> ();
		modifyTerrain = GetComponent<ModifyTerrain> ();

		//Instantiate all the pieces for all the different teams
		foreach (Team team in teams) {
			int squareLength = (int) Mathf.Sqrt(team.startingPlayerCount);
			for(int x = 0; x < squareLength + 1; x ++){
				for(int z = 0; z < squareLength; z ++){
					if(x + z >= team.startingPlayerCount){
						break;
					}
					//Actual x in the world instead of relative x
					int actualX = team.currentCenterX - (int)(0.5 * squareLength) + x;
					int actualZ = team.currentCenterZ - (int)(0.5 * squareLength) + z;
					int y;
					//Find top height on the make and set the y 
					for(y = world.worldY -1 ; y >= 0; y --){
						if(world.data[actualX,y,actualZ] != 0){
							y ++;
							break;
						}
					}
					GameObject pieceObject = Instantiate(basicPiece, new Vector3(actualX, y , actualZ), new Quaternion(0,0,0,0)) as GameObject;
					Piece piece = pieceObject.GetComponent<Piece>();
					piece.team = team;
					pieceObject.renderer.material.color = team.color;
					team.pieces.Add(piece);
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			ShootRay();		
		}
	}

	int piecesThatMoved = 0;

	void ShootRay(){
		//Get a ray from the mouse position on the screen
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			//Try to move character if it's their turn 
			if(hit.collider.tag.Equals ("Character") && !takingTurn){
				GameObject obj = hit.collider.gameObject;
				Piece piece = obj.GetComponent<Piece>();
				if(piece.team.Equals(currentTeam) && ! piece.doneTakingMove){
					takingTurn = true;
					TryTakeTurn(obj);
				}
			}


			else{
				//Removed for now. I don't want to be able to add players because ben is a little bitch.
				/*
				if(!takingTurn){
					Debug.Log ("Adding player");
					modifyTerrain.AddPlayerCursor();
				}
				else{*/
				if(takingTurn){
					Vector3 pos = hit.point - 0.5f*hit.normal;
					Vector3 cubePos = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
					if(world.selectedTiles.ContainsKey(cubePos)){
						world.pieces.Remove(pieceTakingTurn.transform.position);
						pieceTakingTurn.transform.position = cubePos + new Vector3(0,1,0);
						world.pieces.Add (pieceTakingTurn.transform.position, pieceTakingTurn);
						takingTurn = false;
						ClearSelectedTiles();
						pieceTakingTurn.GetComponent<Piece>().doneTakingMove = true;
						piecesThatMoved ++;
						if(piecesThatMoved == pieceTakingTurn.GetComponent<Piece>().team.pieces.Count){
							piecesThatMoved = 0;
							foreach(Piece piece in currentTeam.pieces){
								piece.doneTakingMove = false;
							}
							currentTeamNum  = (currentTeamNum  + 1) % teams.Count;
							Debug.Log("Changing to team number " + currentTeamNum);
							currentTeam = teams[currentTeamNum];
						}
					}
				}
			}

		}
	}


	//Clears all tiles that are selected colored.
	void ClearSelectedTiles(){
		foreach (Vector3 tile in world.selectedTiles.Keys) {
			int x = Mathf.RoundToInt(tile.x) / world.chunkSize;
			int y = Mathf.RoundToInt(tile.y) / world.chunkSize;
			int z = Mathf.RoundToInt(tile.z) / world.chunkSize;
			world.chunks[x,y,z].update = true;		
		}
		world.selectedTiles.Clear();
	}

	//Called when piece is selected to move
	void TryTakeTurn(GameObject character){
		pieceTakingTurn = character;
		RecursiveMoves (6, Mathf.RoundToInt(character.transform.position.x), Mathf.RoundToInt(character.transform.position.y), Mathf.RoundToInt (character.transform.position.z));
	}

	//
	void RecursiveMoves(int moves, int x, int y, int z){
		if(moves < 0 )
			return;

		int moveCost = -100;
		world.selectedTiles.TryGetValue (new Vector3 (x, y - 1, z), out moveCost);

		if (moveCost == -100) {
						world.selectedTiles.Add (new Vector3 (x, y - 1, z), moves);
						world.chunks [x / world.chunkSize, (y - 1) / world.chunkSize, z / world.chunkSize].update = true;
				} else if (moves > moveCost) {
						world.selectedTiles.Remove (new Vector3 (x, y - 1, z));
						world.selectedTiles.Add (new Vector3 (x, y - 1, z), moves);
						world.chunks [x / world.chunkSize, (y - 1) / world.chunkSize, z / world.chunkSize].update = true;
				} else {
						return;
				}
		Debug.Log ("moves" + x + " , " + z);
		bool found = false;
		TryMove(moves -1,x,  y, z, 1,0); 
		TryMove(moves -1,x,  y, z, -1,0);
		TryMove(moves -1,x,  y, z, 0,1);
		TryMove(moves -1,x,  y, z, 0,-1);
	}

	void TryMove(int moves, int x, int y, int z, int dx, int dz){
		if (x + dx < 0 || x + dx >= world.worldX || z + dz < 0 || z + dz >= world.worldZ)
				return;
		if (world.data[x + dx, y, z + dz] == 0) {
			for(int i = 0; i <= maxFallHeight; i ++){
				if(y - i < 0)
					break;
				if(world.data[x + dx, y - i, z + dz] != 0){
					RecursiveMoves(moves, x +dx, y -i + 1, z + dz); 
					break;
				}
			}
		}
		else{
			for(int i = 1; i <= moves; i ++){
				if(world.data[x +dx, y + i, z +dz] == 0){
					RecursiveMoves(moves - 2*i, x + dx, y + i , z + dz);
					break;
				}
			}
		}
	}

    

    static int GetHash(int x, int z){
			Vector2 vector = new Vector2 (x, z);
			return vector.GetHashCode ();
	}


		                       

}


