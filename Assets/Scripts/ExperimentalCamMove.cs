using UnityEngine;
using System.Collections;

public class ExperimentalCamMove : MonoBehaviour {

    public GameObject player;
	public Vector3 offset = new Vector3(0, +20, -20);

	void Start(){
		transform.position = player.transform.position + offset;
		transform.LookAt (player.transform.position);
	}

	// Update is called once per frame
	void FixedUpdate () {
		//set camera somewhere between the two positions, gives a lagging behind or trailing effect. I think. 
		transform.position = Vector3.Lerp (transform.position, player.transform.position + offset, 0.1f); 
		//transform.position = player.transform.position + offset;
	}
}
