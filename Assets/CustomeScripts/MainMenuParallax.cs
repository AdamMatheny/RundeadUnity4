using UnityEngine;
using System.Collections;

public class MainMenuParallax : MonoBehaviour {


	public float startPosition;
	public float endPosition;
	public float movementSpeed;
	public float waitTime = 3.0f;
	public float waitToStartTime = 5.0f;
	public bool waitToStart = false;

	// Use this for initialization
	void Start () {

	}


	IEnumerator WaitToMoveAgain() {
		yield return new WaitForSeconds(waitTime);
		Vector3 newPosition = transform.position;
		transform.position = new Vector3(startPosition, newPosition.y, newPosition.z);
	}

	IEnumerator WaitToStart() {
		yield return new WaitForSeconds(waitToStartTime);
		waitToStart = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(waitToStart == false)
		{
			if(startPosition > endPosition)
			{
				if(transform.position.x <= endPosition)
				{
					StartCoroutine(WaitToMoveAgain());
					
				}
				else
				{
					Vector3 newPosition = transform.position;
					transform.position = new Vector3(newPosition.x - movementSpeed, newPosition.y, newPosition.z);
				}
			}
			else if(startPosition < endPosition)
			{
				if(transform.position.x >= endPosition)
				{
					StartCoroutine(WaitToMoveAgain());
					
				}
				else
				{
					Vector3 newPosition = transform.position;
					transform.position = new Vector3(newPosition.x - movementSpeed, newPosition.y, newPosition.z);
				}
			}
		}
		else if(waitToStart == true)
		{
			StartCoroutine(WaitToStart());
		}


	

	}
}
