using UnityEngine;
using System.Collections;

public class Poster_cameraLootAt : MonoBehaviour {


	Vector3 originalPosition;
	Vector3 endPosition;
	bool moveCamera = false;
	Camera main;
	GameObject cam;


	// Use this for initialization
	void Start () {
		cam = GameObject.Find("Main Camera");
		main = cam.camera;
	}
	
	// Update is called once per frame
	void Update () {
		if(moveCamera == true)
		{
			main.transform.position = Vector3.Lerp(originalPosition, endPosition, 5.0f * Time.deltaTime);
			//moveCamera = false;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{

			originalPosition = GameObject.FindWithTag("MainCamera").camera.transform.position;
			endPosition = this.transform.position;
			moveCamera = true;

		}
	}

	void OnTriggerExit(Collider other)
	{
		endPosition = originalPosition;
	}
}
