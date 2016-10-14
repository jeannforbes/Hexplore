using UnityEngine;
using System.Collections;

public class HexMaker : MonoBehaviour {

	public float randomness = 2f;
	public GameObject Hex;
	private GameObject hex;

	// Use this for initialization
	void Start () {
		hex = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {

		if (other.CompareTag ("hexagon")) {
			Destroy (this.gameObject);
		}else if (other.CompareTag ("Player")) {
			hex = (GameObject)Instantiate (Hex);
			hex.transform.position = new Vector3 (this.transform.GetChild(0).transform.position.x + Random.Range (-randomness, randomness), 
			                                      this.transform.position.y - 5f + Random.Range (-randomness, randomness), 
			                                      this.transform.GetChild (0).transform.position.z + Random.Range (-randomness, randomness));
			Destroy (this.gameObject);
		}
	}
}
