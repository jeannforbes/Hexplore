using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed = 0.25f;
	public int jumpStrength = 20;
	private int jumpTimer = 20;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow))
			this.transform.Translate (new Vector3 (-speed, 0, 0));
		
		if (Input.GetKey(KeyCode.W) || Input.GetKey (KeyCode.UpArrow) )
			this.transform.Translate (new Vector3 (0, 0, speed));
		
		if (Input.GetKey(KeyCode.S) || Input.GetKey (KeyCode.DownArrow) )
			this.transform.Translate (new Vector3 (0, 0, -speed));
		
		if (Input.GetKey(KeyCode.D) || Input.GetKey (KeyCode.RightArrow) )
			this.transform.Translate (new Vector3 (speed, 0, 0));
		if (Input.GetKey (KeyCode.Space) ) {
			transform.Translate(Vector3.up * jumpStrength * Time.deltaTime, Space.World);
		}
	}
}

