using UnityEngine;
using System.Collections;

public class PanningCamera : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		transform.position += transform.right * Input.GetAxis ("Horizontal");
		transform.position += transform.forward * Input.GetAxis ("Vertical");
		transform.eulerAngles += Vector3.up * Input.GetAxis ("RotateCamera");
	}

}
