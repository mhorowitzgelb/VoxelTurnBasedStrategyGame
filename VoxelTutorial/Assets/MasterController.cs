using UnityEngine;
using System.Collections;

public class MasterController : MonoBehaviour {
	public World world;

	void Start(){
		world = GetComponent<World> ();

	}
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
				
		}
	}

	void ShootRay(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			if(hit.collider.tag.Equals ("Character")){
				TryTakeTurn(hit.collider.gameObject);

			}
		}
	}

	void TryTakeTurn(GameObject character){
	}
}


