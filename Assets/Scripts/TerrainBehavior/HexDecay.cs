using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexDecay : MonoBehaviour {

	public float decayRate = 200f;
	public float spreadDistance = 5f;
	private bool decaying;
	private float startTime;
	private Color startColor;

	// Use this for initialization
	void Start () {
		startTime = decayRate;
		decaying = true;
		startColor = this.GetComponent<Renderer> ().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (decaying && GetComponent<Renderer>().material)
			Decay ();
	}

	void Decay(){
		if (startTime < 0) {
			gameObject.GetComponent<MeshCollider> ().enabled = false;
		} else {
			GetComponent<Renderer> ().material.color = new Color (startColor.r, startColor.g, startColor.b, 1f * startTime / decayRate);
			startTime -= 10 * Time.deltaTime;
		}
	}
}
