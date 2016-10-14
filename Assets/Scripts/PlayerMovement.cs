using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public GameObject Hex;
	public float speed = 0.25f;
	public int jumpStrength = 20;
	private int jumpTimer = 20;
    private float health = 100;
    private int score = 0;

    //properties

    //gets the player's health
    public float Health
    {
        get
        {
            return health;
        }
    }

    //gets the player's score
    public int Score
    {
        get
        {
            return score;
        }
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow))
			this.transform.Translate (new Vector3 (-speed, 0, 0), Space.World);
		
		if (Input.GetKey(KeyCode.W) || Input.GetKey (KeyCode.UpArrow) )
			this.transform.Translate (new Vector3 (0, 0, speed), Space.World);
		
		if (Input.GetKey(KeyCode.S) || Input.GetKey (KeyCode.DownArrow) )
			this.transform.Translate (new Vector3 (0, 0, -speed), Space.World);
		
		if (Input.GetKey(KeyCode.D) || Input.GetKey (KeyCode.RightArrow) )
			this.transform.Translate (new Vector3 (speed, 0, 0), Space.World);
		if (Input.GetKey (KeyCode.Space) && (jumpTimer > 50) ) {
			transform.Translate(Vector3.up * jumpStrength * Time.deltaTime, Space.World);
			jumpTimer -=2;
		}

		if(jumpTimer < 100) jumpTimer++;

		//Check if the player is still alive & reset if they aren't
		if (this.transform.position.y < -15) {
			this.transform.position = new Vector3(0,10,0);
			this.transform.rotation = Quaternion.identity;
			this.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
			GameObject hex = (GameObject)Instantiate (Hex);
			hex.transform.position = new Vector3 (0,-1f,0);
		}
	}
}

