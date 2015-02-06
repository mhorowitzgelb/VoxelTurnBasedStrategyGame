using UnityEngine;
using System.Collections;

public class MasterController : MonoBehaviour {
	public World world;
	public ModifyTerrain modifyTerrain;
	private GameObject pieceTakingTurn;
	private bool takingTurn = false;
	private int maxFallHeight = 3;

	void Start(){
		world = GetComponent<World> ();
		modifyTerrain = GetComponent<ModifyTerrain> ();

	}
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			ShootRay();		
		}
	}

	void ShootRay(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			if(hit.collider.tag.Equals ("Character") && !takingTurn){
				takingTurn = true;
				TryTakeTurn(hit.collider.gameObject);
			}
			else{
				if(!takingTurn){
					Debug.Log ("Adding player");
					modifyTerrain.AddPlayerCursor();
				}
				else{
					Vector3 pos = hit.point - 0.5f*hit.normal;
					Vector3 cubePos = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
					if(world.selectedTiles.ContainsKey(cubePos)){
						pieceTakingTurn.transform.position = cubePos + new Vector3(0,1,0);
						takingTurn = false;
						ClearSelectedTiles();
					}
				}
			}

		}
	}

	void ClearSelectedTiles(){
		foreach (Vector3 tile in world.selectedTiles.Keys) {
			int x = Mathf.RoundToInt(tile.x) / world.chunkSize;
			int y = Mathf.RoundToInt(tile.y) / world.chunkSize;
			int z = Mathf.RoundToInt(tile.z) / world.chunkSize;
			world.chunks[x,y,z].update = true;		
		}
		world.selectedTiles.Clear();
	}

	void TryTakeTurn(GameObject character){
		pieceTakingTurn = character;
		character.renderer.material.color = Color.blue;
		RecursiveMoves (10, Mathf.RoundToInt(character.transform.position.x), Mathf.RoundToInt(character.transform.position.y), Mathf.RoundToInt (character.transform.position.z));
	}

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
					RecursiveMoves(moves - i, x + dx, y + i , z + dz);
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


