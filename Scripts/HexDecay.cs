using UnityEngine;
using System.Collections;

public class HexDecay : MonoBehaviour {

	private float decayTime = 200;
	private bool decaying;
	private float startTime;
	private Material mat;

	// Use this for initialization
	void Start () {
		startTime = 0;
		decaying = false;
		mat = this.GetComponent<Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		float percent = decayTime/startTime;
		if (startTime > decayTime) Destroy (this.gameObject);
		if (mat)
			mat.color = Color.Lerp(Color.green, Color.red, Mathf.PingPong(startTime, decayTime) / decayTime);
		startTime++;
	}
}
