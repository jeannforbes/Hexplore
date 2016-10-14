using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private Quaternion relativeRot;
	private Vector3 relativePos, worldPos;

	// Use this for initialization
	void Start () {
		relativeRot = this.transform.rotation;
		relativePos = this.transform.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.rotation = relativeRot;
		//this.transform.localPosition = relativePos;
	}
}
