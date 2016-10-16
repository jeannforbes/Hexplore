using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float camHeight = 20f, camDist = 20f;
	public Vector3 currentRot = Vector3.down;

    private GameObject Player = null;

    // Use this for initialization
    void Start () {

		Player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
        //experimental changes
		Vector3 pos = Player.transform.position;
        pos.y += camHeight;
        pos.z -= camDist;
        transform.position = new Vector3(0f, pos.y+camHeight, pos.z-camDist);
        
		//Swap cameras based on player's current gravity
		if (currentRot != Player.GetComponent<PlayerMovement> ().gravityDirection) {
			
		}
    }
}
