using UnityEngine;
using System.Collections;

public class ElevatorLift : MonoBehaviour {

	public GameObject hitterOfTrigger;
	public GameObject thisElevator;
	bool gotHit = false;
	bool readyToLift = false;
	int i = 0;
	public GameObject magnetStuff;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(gotHit)
		{

			if(thisElevator.transform.position.y  <= 150f)
			{
				Vector3 newPosition = thisElevator.transform.position;
				thisElevator.transform.position = new Vector3(newPosition.x, newPosition.y+.1f, newPosition.z);
				Vector3 newPosition2 = hitterOfTrigger.transform.position;
				hitterOfTrigger.transform.position = new Vector3(newPosition2.x, newPosition2.y+.1f, newPosition2.z);
			
			}
		}
	}


	IEnumerator WaitToStart(Collider girl) {
		yield return new WaitForSeconds(1f);
		gotHit = true;
		girl.GetComponent<NavMeshAgent>().enabled = false;
		magnetStuff.GetComponent<NPCMagnet>().enabled = false;

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == hitterOfTrigger.tag && readyToLift == false)
		{
			readyToLift = true;
			StartCoroutine(WaitToStart(other));
		}
			
	}
}
