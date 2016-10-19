using UnityEngine;
using System.Collections;

public class WeatherScript : MonoBehaviour {
    private Skybox skybox;
    private Material initMaterial;

    private GameObject player;
    private Vector3 weatherVector;

	// Use this for initialization
	void Start () {
        skybox = (Skybox)FindObjectOfType(typeof(Skybox));
        initMaterial = skybox.material;
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        weatherVector = player.transform.position;
        weatherVector.y = 30f;
        this.transform.position = weatherVector;
	}
}
