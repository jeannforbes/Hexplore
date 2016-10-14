using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    
	private Quaternion relativeRot;
	private Vector3 relativePos, worldPos;
    public Transform player = null;
    public float camHeight = 20.0f;
    private Transform cam = null;

    // Use this for initialization
    void Start () {
        //relativeRot = this.transform.rotation;
        //relativePos = this.transform.localPosition;   

        //experimental changes - inspiration taken from http://answers.unity3d.com/questions/938894/camera-following-without-rotation.html
        cam = transform;
        
	}
	
	// Update is called once per frame
	void Update () {
        //this.transform.rotation = relativeRot;
        //this.transform.localPosition = relativePos;

        //experimental changes
        Vector3 pos = player.transform.position;
        pos.y += camHeight;
        pos.z -= camHeight;
        cam.position = pos;
        

    }
}
