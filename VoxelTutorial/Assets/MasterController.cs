using UnityEngine;
using System.Collections;

public class MasterController : MonoBehaviour {
	public World world;
	public ModifyTerrain modifyTerrain;

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
			if(hit.collider.tag.Equals ("Character")){
				TryTakeTurn(hit.collider.gameObject);
			}
			else{
				modifyTerrain.AddPlayerCursor();
			}

		}
	}

	void TryTakeTurn(GameObject character){
		character.renderer.material.color = Color.blue;
		RecursiveMoves (3, Mathf.RoundToInt(character.transform.position.x), Mathf.RoundToInt (character.transform.position.z));
	}

	void RecursiveMoves(int moves, int x, int z){
		if(moves < 0 )
			return;
		Debug.Log ("moves" + x + " , " + z);
		if(!world.selectedTiles.ContainsKey(new Vector2(x,z)))
			world.selectedTiles.Add (new Vector2(x,z), true);
		world.chunks [x/world.chunkSize, world.heightMap [x, z] / world.chunkSize, z / world.chunkSize].update = true;
		bool found = false;
		RecursiveMoves (moves - 1, x + 1, z);
		RecursiveMoves (moves - 1, x - 1, z);
		RecursiveMoves (moves - 1, x, z + 1);
		RecursiveMoves (moves - 1, x, z - 1);
	}
    

    static int GetHash(int x, int z){
			Vector2 vector = new Vector2 (x, z);
			return vector.GetHashCode ();
	}


		                       

}


