﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterBehavior : MonoBehaviour {

	public ParticleSystem splash;
	public float buoyancy = 200f;

	private List<ParticleSystem> splashes;

	// Use this for initialization
	void Start () {
		splashes = new List<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Get rid of particle systems that have played
		for(int i=0; i<splashes.Count; i++) {
			if(!splashes[i].IsAlive ()){
				Destroy (splashes[i].gameObject);
				splashes.RemoveAt (i);
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")) makeSplash (other.transform.position);
	}

	void OnTriggerStay(Collider other){
		if(other.CompareTag("Player")) other.gameObject.GetComponent<Rigidbody> ().AddForce (Vector3.up * buoyancy * 1000 * Time.deltaTime);
	}

	//creates a particle emitter at the gameobject's position
	void makeSplash(Vector3 pos){
		splashes.Add((ParticleSystem)Instantiate (splash, pos, Quaternion.identity));
	}
}
