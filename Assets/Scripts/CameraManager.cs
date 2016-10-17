using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {
	
	public float camHeight = 10f, camDist = 10f;
	private List<GameObject> cameras; //top, bottom
	public GameObject topCam;
	public GameObject botCam;
	
	private GameObject Player = null;
	private GameObject activeCam = null;
	private Vector3 camDir = Vector3.down;
	
	// Use this for initialization
	void Start () {
		cameras = new List<GameObject> ();
		cameras.Add (topCam);
		botCam.transform.Rotate(0, 0*Time.deltaTime, 180);
		cameras.Add (botCam);
		Player = GameObject.FindGameObjectWithTag ("Player");

		if(cameras.Count > 0) activeCam = cameras [0];
	}
	
	// Update is called once per frame
	void Update () {
		if (cameras.Count > 0) {
			updateCam ();
			if (camDir != Player.GetComponent<PlayerMovement> ().gravityDirection)
				swapCam ();
		}
	}

	void updateCam(){
		Vector3 pos = Player.transform.position;
		if(activeCam == topCam) activeCam.transform.position = new Vector3(pos.x, pos.y+camHeight, pos.z-camDist);
		else if(activeCam == botCam) activeCam.transform.position = new Vector3(pos.x, pos.y-camHeight, pos.z-camDist);
	}

	//Swap cameras based on player's current gravity
	void swapCam(){
		//Set the current camera direction to the player's gravityDirection
		camDir = Player.GetComponent<PlayerMovement> ().gravityDirection;
		//Disable the old active camera
		activeCam.GetComponent<Camera>().enabled = false;

		switch((int)camDir.y){
		case 1:
			activeCam = cameras[1];
			break;
		default:
			activeCam = cameras[0];
			break;
		}

		activeCam.GetComponent<Camera> ().enabled = true;
	}
}
