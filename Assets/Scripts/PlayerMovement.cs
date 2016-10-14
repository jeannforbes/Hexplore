using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    //////////////////// Public Values ////////////////////
    public GameObject Hex;
    public float maxSpeed = 15.0f;
    public int slideForce = 18;
    public int jumpStrength = 75;

   // public float gravityStrength = 10f;
   // public Vector3 gravityDirection = Vector3.down;

    /////////////////// Private Values ///////////////////
    //ability to jump is now controlled by a bool value.
    private bool onJumpableSurface = true;
    private bool jumpReady = true;
   // private float maxHealth = 10;
    //private float health;
    private int score = 0;

    private Rigidbody rBody;
    private GameObject collidingGO = null;

	private Color originalColor;

    //properties
    /*
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
    */

    // Use this for initialization
    void Start () {
        rBody = (Rigidbody)this.GetComponent("Rigidbody");
        //health = maxHealth;
		originalColor = GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update() {
        //gravity
        //rBody.AddForce(gravityStrength * gravityDirection);

        //player controls
        /*Old movement code, always aligned to normal gravity*/
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            rBody.AddForce(new Vector3(-slideForce, 0, 0) * Time.deltaTime);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            rBody.AddForce(new Vector3(0, 0, slideForce) * Time.deltaTime);

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            rBody.AddForce(new Vector3(0, 0, -slideForce) * Time.deltaTime);

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            rBody.AddForce(new Vector3(slideForce, 0, 0) * Time.deltaTime);

        bool spacePressed = Input.GetKey(KeyCode.Space);
        if (spacePressed && onJumpableSurface && jumpReady)
        {
            rBody.AddForce(Vector3.up * jumpStrength /** Time.deltaTime*/);
            jumpReady = false;
        }
        if (!spacePressed && !jumpReady)
        {
            //print("Jump ready!");
            jumpReady = true;
        }
        

        /* New movement code, does not adjust correctly for gravity changes.
        if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        { 
            rBody.AddForce(Quaternion.Euler(0f,0f,-90.0f) * gravityDirection * slideForce);
            print(Quaternion.Euler(0f, 0f, -90.0f) * gravityDirection);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rBody.AddForce(Quaternion.Euler(-90.0f, 0f, 0f) * gravityDirection * slideForce);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rBody.AddForce(Quaternion.Euler(90.0f, 0f, 0f) * gravityDirection * slideForce);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rBody.AddForce(Quaternion.Euler(0f, 0f, 90.0f) * gravityDirection * slideForce);
        }
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            rBody.AddForce(-1* gravityDirection * jumpStrength);
		}
*/
        rBody.velocity = Vector3.ClampMagnitude(rBody.velocity, maxSpeed);

		//Check if the player is still alive & reset if they aren't
		if (this.transform.position.y < -15 /*|| health <= 0*/) {
			this.transform.position = new Vector3(0,10,0);
			this.transform.rotation = Quaternion.identity;
			this.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
			GameObject hex = (GameObject)Instantiate (Hex);
			hex.transform.position = new Vector3 (0,-1f,0);

           // health = maxHealth;
        }

        onJumpableSurface = false;
	}

    //Handles any collisions with the player. Mostly treasure and monster hexes.
    void OnCollisionEnter(Collision collisions)
    {
        collidingGO = collisions.gameObject;
        //print("Name: " + collidingGO.name);
		if(collisions.gameObject.GetComponent<Renderer>()) this.GetComponent<Renderer> ().material.color = collisions.gameObject.GetComponent<Renderer> ().material.color;
    }

	void OnCollisionExit(Collision collisions){
		this.GetComponent<Renderer> ().material.color = originalColor;
	}

	void OnTriggerEnter(Collider other){
		if(other.GetComponent<Renderer>()) this.GetComponent<Renderer> ().material.color = other.gameObject.GetComponent<Renderer> ().material.color;
	}

    //Checks if the player is touching a trigger.
    void OnTriggerStay(Collider other)
    {
        GameObject triggerGO = other.gameObject;
        //Jump renabled when the player is touching the outer hex trigger.
        if (triggerGO.tag == "Ground")
        {
            onJumpableSurface = true;
        }
    }

	void OnTriggerExit(Collider other){
		this.GetComponent<Renderer> ().material.color = originalColor;
	}

    public override string ToString()
    {
        return "Score: " + score;
    }
}

