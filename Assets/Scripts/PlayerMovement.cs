﻿using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour {
   /* public struct Deform
    {
        public Vector3 deformDirection;
        public float deformTime;

        public Deform(Vector3 direction,float time)
        {
            this.deformDirection = direction;
            this.deformTime = time;
        }
    }*/

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
    //private int score = 0;

    private Rigidbody rBody;
    private GameObject collidingGO = null;

    private Vector3 rightVector;
	private Vector3 frontVector;

    //deformation handler
    private Vector3 collisionVector;
    private GameObject deformObject;
    private Vector3 netImpulse;
    private int numImpacts = 0;
	private float originalScale;



    // Use this for initialization
    void Start () {
        rBody = (Rigidbody)this.GetComponent("Rigidbody");

        //deform components
        deformObject = new GameObject();
        netImpulse = Vector3.zero;
        //assuming we always have a cube, the magnitude is equal to one of the axises times square-root of three
        originalScale = transform.localScale.magnitude / Mathf.Sqrt(3);
        //print("Scale: " + originalScale);
	}
	
	// Update is called once per frame
	void Update() {
		
		/* Movement code (gravity based)
        rightVector = Vector3.Cross(gravityDirection, new Vector3(0,0,1));
        if(rightVector.sqrMagnitude == 0)
        {
            rightVector = Vector3.Cross(gravityDirection, new Vector3(0, 1, 0));
        }
        frontVector = Vector3.Cross(gravityDirection, rightVector);
        //print("Front: " + frontVector + ", Right: " + rightVector);
        */
		
		/*Movement code (camera based)*/
		if (Camera.current != null)
		{
			frontVector = Camera.current.transform.forward;
			frontVector.y = 0;
			//print("camera front: " + frontVector);
			rightVector = Vector3.Cross(frontVector, Vector3.up);
		}

        if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        { 
            rBody.AddForce(rightVector * Time.deltaTime * slideForce);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rBody.AddForce(frontVector * Time.deltaTime * slideForce);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rBody.AddForce(-1*frontVector * Time.deltaTime * slideForce);
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

		//Check if the player is beneath the grid, and flip them!
		if (this.transform.position.y < -20) {
			this.transform.position = new Vector3(0,30,0);
			this.transform.rotation = Quaternion.identity;
			this.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
			//gravityDirection = Vector3.up;
		} else {
			//gravityDirection = Vector3.down;
		}
        
        //deforming logic
        if (netImpulse.sqrMagnitude != 0 && numImpacts != 0)
        {
            netImpulse /= numImpacts;
            netImpulse = transform.InverseTransformVector(netImpulse);

            collisionVector = createCollisionVector(netImpulse);

            this.transform.localScale = collisionVector * originalScale;
            deformObject.transform.forward = Vector3.Normalize(collisionVector);

            /*
            this.transform.localScale = Vector3.one;
            this.transform.parent = deformObject.transform;
            deformObject.transform.localScale = new Vector3(1, collisionVector.magnitude/MAX_MAG, 1);
            this.transform.parent = null;
            */

            netImpulse *= 0;
            numImpacts = 0;
        }
        else
        {
            float scaleX = Mathf.Clamp((transform.localScale.x) * (1 + 0.45f * Time.deltaTime), 0f, originalScale);
            float scaleY = Mathf.Clamp((transform.localScale.y) * (1 + 0.45f * Time.deltaTime), 0f, originalScale);
            float scaleZ = Mathf.Clamp((transform.localScale.z) * (1 + 0.45f * Time.deltaTime), 0f, originalScale);
            this.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }

        onJumpableSurface = false;
	}

    //Handles any collisions with the player. Mostly treasure and monster hexes.
    void OnCollisionEnter(Collision collisions)
    {
        collidingGO = collisions.gameObject;
        //if(collisions.gameObject.GetComponent<Renderer>()) this.GetComponent<Renderer> ().material.color = collisions.gameObject.GetComponent<Renderer> ().material.color;
        
        if (collidingGO.CompareTag("hexagon") || collidingGO.CompareTag("Obelisk"))
        {
            netImpulse += collisions.impulse / Time.fixedDeltaTime;
            numImpacts++;
        }
		// if(collidingGO.GetComponent<Renderer>()) 
		//	this.GetComponent<Renderer> ().material = collidingGO.GetComponent<Renderer> ().material;
    }

	void OnCollisionExit(Collision collisions){
		//this.GetComponent<Renderer> ().material.color = originalColor;
	}

	void OnTriggerEnter(Collider other){
		//if(other.GetComponent<Renderer>()) this.GetComponent<Renderer> ().material = other.gameObject.GetComponent<Renderer> ().material;
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

   /*( public override string ToString()
    {
        return "Score: " + score;
    }
    */

	private Vector3 createCollisionVector(Vector3 impact)
	{
        Vector3 tempVector = Vector3.one;
		for(int i = 0; i < 3; i++)
		{
            tempVector[i] = (Mathf.Abs(impact[i]) - 1000) / -1000;
			if(tempVector[i] < 0.1f)
			{
				tempVector[i] = 0.1f;
			}
		}
		return tempVector;
	}
}
