using UnityEngine;
using System.Collections;

public class StoryCam : MonoBehaviour {

	public Vector3 newPos;
	public Vector3 newRot;
	public Transform newAngle;

	// Update is called once per frame
	void OnTriggerStay () 
	{
		//Camera.current.transform.position = Vector3.Lerp(Camera.current.transform.rotation, newPos, Time.deltaTime);
		Camera.current.transform.position = Vector3.Lerp(Camera.current.transform.position, newRot, Time.deltaTime);

	}
}
