using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudBehavior : MonoBehaviour {
	
	public ParticleSystem splash;
	public float slow = 20f;
	public float integrity = 100f;
	private float maxIntegrity;
	private Color originalColor;
	
	private List<ParticleSystem> splashes;
	
	// Use this for initialization
	void Start () {
		splashes = new List<ParticleSystem> ();
		maxIntegrity = integrity;
		originalColor = GetComponent<Renderer> ().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		//Get rid of particle systems that have played
		for (int i=0; i<splashes.Count; i++) {
			if (!splashes [i].IsAlive ()) {
				Destroy (splashes [i].gameObject);
				splashes.RemoveAt (i);
			}
		}
		if (integrity < 0)
			this.gameObject.GetComponent<MeshCollider> ().enabled = false;
		else
			GetComponent<Renderer> ().material.color = new Color (originalColor.r, originalColor.g, originalColor.b, 0.8f * integrity / maxIntegrity);
		
	}
	
	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")) makeSplash (other.transform.position);
	}
	
	void OnTriggerStay(Collider other){
		//Slow the player while inside
		if (other.CompareTag ("Player")){
			if(slow > 0)
				other.gameObject.GetComponent<Rigidbody> ().velocity /= slow;
			else
				other.gameObject.GetComponent<Rigidbody> ().velocity *= -slow;

			//Reduce integrity while player is inside
			integrity -= 10 * Time.deltaTime;
		}
	}
	
	//creates a particle emitter at the gameobject's position
	void makeSplash(Vector3 pos){
		splashes.Add((ParticleSystem)Instantiate (splash, pos, Quaternion.identity));
	}
}