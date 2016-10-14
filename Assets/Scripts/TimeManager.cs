using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public float dayLength = 1f;

	private GameObject mainLight;

	// Use this for initialization
	void Start () {
		mainLight = GameObject.FindGameObjectWithTag ("MainLight");
	}
	
	// Update is called once per frame
	void Update () {
		Light l = mainLight.GetComponent<Light> ();
		//Rotate the main directional light based on current time
		mainLight.transform.Rotate (Vector3.left / dayLength * Time.deltaTime);
		//Change intensity!
		//l.intensity = 0.01f * Mathf.Sin (Time.time / dayLength);
	}
}
