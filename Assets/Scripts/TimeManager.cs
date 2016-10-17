using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TimeManager : MonoBehaviour {

	public float dayLength = 1f;
	public ParticleSystem Firefly;

	private GameObject mainLight;
	private List<Hex> hexes;
	private GameObject[] fireflies;
	private bool isDay;
	private float timeChangeCounter;

	// Use this for initialization
	void Start () {
		mainLight = GameObject.FindGameObjectWithTag ("MainLight");
		fireflies = GameObject.FindGameObjectsWithTag("Fireflies");
		isDay = true;
		timeChangeCounter = 0;
		hexes = GetComponent<GridManager> ().grid.hexes;
	}
	
	// Update is called once per frame
	void Update () {

		updateTime ();
		Debug.Log (isDay);

		hexes = GetComponent<GridManager> ().grid.hexes;
		if (!isDay && fireflies.Length < 5) {
			GameObject hexGO = null;
			while (hexGO == null || !hexGO.GetComponent<Renderer>().enabled)
				if(hexes.Count > 0) hexGO = hexes [Random.Range (0, hexes.Count)].go;
			Vector3 fiPos = new Vector3 (hexGO.transform.position.x, 
			                            hexGO.transform.position.y + 10, 
			                            hexGO.transform.position.z);
			Instantiate (Firefly, fiPos, Quaternion.identity);
			fireflies = GameObject.FindGameObjectsWithTag ("Fireflies");
		} else if (isDay) {
			fireflies = GameObject.FindGameObjectsWithTag ("Fireflies");
			for(int i=0; i<fireflies.Length; i++){
				Destroy (fireflies[i].gameObject);
			}
		}
	}

	void updateTime(){
		float increment = (dayLength*500)/360 * Time.deltaTime;
		mainLight.transform.rotation *= Quaternion.AngleAxis (increment, Vector3.one);
		if (timeChangeCounter > 50 && isDay && mainLight.transform.rotation.eulerAngles.x < 5){
			isDay = false;
			timeChangeCounter = 0;
		}
		else if (timeChangeCounter > 50 && !isDay && mainLight.transform.rotation.eulerAngles.x < 1) {
			isDay = true;
			timeChangeCounter = 0;
		}
		timeChangeCounter++;
	}
}
