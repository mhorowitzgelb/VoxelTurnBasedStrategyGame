using UnityEngine;
using System.Collections;

public class MasterController : MonoBehaviour {
	public World world;
	public ModifyTerrain modifyTerrain;
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
				TryTakeTurn(hit.collider.gameObject);
			}
			else{
				Debug.Log ("Adding player");
				modifyTerrain.AddPlayerCursor();
			}

		}
	}

	void TryTakeTurn(GameObject character){
		character.renderer.material.color = Color.blue;
		RecursiveMoves (30, Mathf.RoundToInt(character.transform.position.x), Mathf.RoundToInt(character.transform.position.y), Mathf.RoundToInt (character.transform.position.z));
	}

	void RecursiveMoves(int moves, int x, int y, int z){
		if(moves < 0 )
			return;
		Debug.Log ("moves" + x + " , " + z);

		if (!world.selectedTiles.ContainsKey (new Vector3 (x, y -1, z))) {
			world.selectedTiles.Add (new Vector3 (x, y -1, z), true);
			world.chunks [x / world.chunkSize, (y-1) / world.chunkSize, z / world.chunkSize].update = true;
		}
		bool found = false;
		TryMove(moves -1,x,  y, z, 1,0); 
		TryMove(moves -1,x,  y, z, -1,0);
		TryMove(moves -1,x,  y, z, 0,1);
		TryMove(moves -1,x,  y, z, 0,-1);
	}

	void TryMove(int moves, int x, int y, int z, int dx, int dz){
		if (world.data[x + dx, y, z + dz] == 0) {
			for(int i = 0; i <= maxFallHeight; i ++){
				if(y - i < 0)
					break;
				if(world.data[x + dx, y - i, z + dz] != 0){
					RecursiveMoves(moves, x +dx, y -i + 1, z + dz); 	
				}
			}
		}
		else{
			for(int i = 1; i <= moves; i ++){
				if(world.data[x +dx, y + i, z +dz] == 0){
					RecursiveMoves(moves - i, x + dx, y + i , z + dz);
				}
			}
		}
	}

    

    static int GetHash(int x, int z){
			Vector2 vector = new Vector2 (x, z);
			return vector.GetHashCode ();
	}


		                       

}


