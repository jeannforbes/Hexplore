using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    //////////////////// Public Values ////////////////////
    public float maxSpeed = 15.0f;
    public int slideForce = 1200;
    public int jumpStrength = 4000;

    public float gravityStrength = 500f;
    public Vector3 gravityDirection = Vector3.down;

    /////////////////// Private Values ///////////////////
    //ability to jump is now controlled by a bool value.
    private bool onJumpableSurface = true;
    private bool jumpReady = true;
    private int score = 0;

    private Rigidbody rBody;
    private GameObject collidingGO = null;

    private Vector3 rightVector;
    private Vector3 frontVector;

	private Color originalColor;

    // Use this for initialization
    void Start () {
        rBody = (Rigidbody)this.GetComponent("Rigidbody");
		originalColor = GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update() {

        /* New movement code*/
        rightVector = Vector3.Cross(gravityDirection, new Vector3(0,0,1));
        if(rightVector.sqrMagnitude == 0)
        {
            rightVector = Vector3.Cross(gravityDirection, new Vector3(0, 1, 0));
        }
        frontVector = Vector3.Cross(gravityDirection, rightVector);
        //print("Front: " + frontVector + ", Right: " + rightVector);

        if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        { 
            rBody.AddForce(rightVector * Time.deltaTime * slideForce);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rBody.AddForce(-1*frontVector * Time.deltaTime * slideForce);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rBody.AddForce(frontVector * Time.deltaTime * slideForce);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rBody.AddForce(-1* rightVector * Time.deltaTime * slideForce);
        }

        bool spacePressed = Input.GetKey(KeyCode.Space);
        if (spacePressed && onJumpableSurface && jumpReady)
        {
            rBody.AddForce(-1 * gravityDirection * jumpStrength);
            jumpReady = false;
        }
        if (!spacePressed && !jumpReady)
        {
            jumpReady = true;
        }

        rBody.AddForce(gravityDirection * gravityStrength * Time.deltaTime);

        rBody.velocity = Vector3.ClampMagnitude(rBody.velocity, maxSpeed);

		//Check if the player is still alive & reset if they aren't
		if (this.transform.position.y < -15) {
			this.transform.position = new Vector3(0,30,0);
			this.transform.rotation = Quaternion.identity;
			this.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        onJumpableSurface = false;
	}

    //Handles any collisions with the player. Mostly treasure and monster hexes.
    void OnCollisionEnter(Collision collisions)
    {
        collidingGO = collisions.gameObject;
		if (collidingGO.CompareTag ("Obelisk"))
			collidingGO.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
		else if(collidingGO.GetComponent<Renderer>()) 
			this.GetComponent<Renderer> ().material = collidingGO.GetComponent<Renderer> ().material;
    }

	void OnCollisionExit(Collision collisions){
		//this.GetComponent<Renderer> ().material.color = originalColor;
	}

	void OnTriggerEnter(Collider other){
		if(other.GetComponent<Renderer>()) this.GetComponent<Renderer> ().material = other.gameObject.GetComponent<Renderer> ().material;
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
		//this.GetComponent<Renderer> ().material.color = originalColor;
	}

    public override string ToString()
    {
        return "Score: " + score;
    }
}

