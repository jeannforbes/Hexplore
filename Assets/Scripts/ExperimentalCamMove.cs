using UnityEngine;
using System.Collections;

public class ExperimentalCamMove : MonoBehaviour {

	// portions of code used from http://wiki.unity3d.com/index.php?title=MouseOrbitImproved#Code_C.23
    public GameObject player;
	public Vector3 offset = new Vector3(0, +20, -20);


	private float distance = 15.0f;
	private float xSpeed = 120.0f;
	private float ySpeed = 120.0f;

	private float yMinLimit = -20f;
	private float yMaxLimit = 80f;

	private float distanceMin = 10f;
	private float distanceMax = 30f;

	float x = 0.0f;
	float y = 0.0f;

	void Start(){
		UnityEngine.Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		transform.position = player.transform.position + offset;
		transform.LookAt (player.transform.position);

		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			UnityEngine.Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}



		//set camera somewhere between the two positions, gives a lagging behind or trailing effect. I think. 
		transform.position = Vector3.Lerp (transform.position, player.transform.position + offset, 0.1f); 

	}

	void LateUpdate(){
		x += Input.GetAxisRaw ("Mouse X") * xSpeed * distance *  0.004f;
		y -= Input.GetAxisRaw ("Mouse Y") * ySpeed *  0.04f;

		y = ClampAngle (y, yMinLimit, yMaxLimit);

		Quaternion rotation = Quaternion.Euler (y, x, 0);
		distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*10, distanceMin, distanceMax);

		Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
		Vector3 position = rotation * negDistance + player.transform.position;

		transform.rotation = rotation;
		transform.position = position;
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}
