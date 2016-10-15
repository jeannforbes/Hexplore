﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private int score = 0;

    private Rigidbody rBody;
    private GameObject collidingGO = null;

    private Vector3 rightVector;
    private Vector3 frontVector;

	private Color originalColor;

    //deformation handler
    private Vector3 collisionVector;
    //private List<Deform> deformList;

    private GameObject deformObject;
   // private Vector3 netDeform;
   // private Deform tempDeform;

    // Use this for initialization
    void Start () {
        rBody = (Rigidbody)this.GetComponent("Rigidbody");
		originalColor = GetComponent<Renderer>().material.color;

        //deform components
        //****deformList = new List<Deform>();
        deformObject = new GameObject();
       //****netDeform = new Vector3();
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

        //deforming logic
        /*netDeform *= 0;
        int i = 0;

        float avgDeform = 0;
        while(i < deformList.Count)
        {
            tempDeform = deformList[i];
            tempDeform.deformTime -= Time.deltaTime;
            if(tempDeform.deformTime <= 0)
            {
                deformList.RemoveAt(i);
                continue;
            }

            netDeform +=  tempDeform.deformTime*tempDeform.deformDirection;
            avgDeform += (500-tempDeform.deformTime);

            i++;
        }
 
        if (netDeform.sqrMagnitude != 0)
        {
            avgDeform /= deformList.Count;


            deformObject.transform.forward = Vector3.Normalize(netDeform);
            
            this.transform.localScale = Vector3.one;
            this.transform.parent = deformObject.transform;
            deformObject.transform.localScale = new Vector3(1, .5f, 1);
            this.transform.parent = null;
        }*/

        onJumpableSurface = false;
	}

    //Handles any collisions with the player. Mostly treasure and monster hexes.
    void OnCollisionEnter(Collision collisions)
    {
        collidingGO = collisions.gameObject;
		if(collisions.gameObject.GetComponent<Renderer>()) this.GetComponent<Renderer> ().material.color = collisions.gameObject.GetComponent<Renderer> ().material.color;

        if (collisions.gameObject.tag == "hexagon")
        {
            collisionVector = Vector3.one;
            Vector3 netImpulse = collisions.impulse / Time.fixedDeltaTime;
            collisionVector.x = (Mathf.Abs(netImpulse.x) - 1000) / -1000;
            collisionVector.y = (Mathf.Abs(netImpulse.y)-1000)/-1000;
            collisionVector.z = (Mathf.Abs(netImpulse.z) - 1000) / -1000;
            print("Collision: " + (collisions.impulse/Time.fixedDeltaTime));
            
            this.transform.localScale = collisionVector;
            print("CollisionVector: " + collisionVector);

            //deformList.Add(new Deform(Vector3.Normalize(collisionVector), 1000f));
        }
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
