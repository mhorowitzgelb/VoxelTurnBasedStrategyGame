﻿using UnityEngine;
using System.Collections;

public class PanningCamera : MonoBehaviour {

    public bool PanWithKeyBoard = true;
    public bool PanWithMouse = true;

    //How close the mouse has to be to the edge of the screen for the camera to pan
    private const int MOUSE_PAN_MARGIN = 5;

	// Update is called once per frame
	void Update () {
        float right = 0;
        
        float forward = 0;
        if (PanWithKeyBoard)
        {
            right = Input.GetAxis("Horizontal");
            forward  = Input.GetAxis("Vertical");
        }
        if (PanWithMouse)
        {
            if (Screen.width - Input.mousePosition.x <= MOUSE_PAN_MARGIN)
                right = 1;
            else if (Input.mousePosition.x <= MOUSE_PAN_MARGIN)
                right = -1;
            if (Input.mousePosition.y >= Screen.height)
                forward = 1;
            else if (Input.mousePosition.y <= 0)
                forward = -1;
        }
		transform.position += transform.right * right;
		transform.position += transform.forward * forward;
		transform.eulerAngles += Vector3.up * Input.GetAxis ("RotateCamera");
	}

}
