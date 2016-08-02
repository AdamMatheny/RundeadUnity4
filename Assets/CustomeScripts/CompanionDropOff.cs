using UnityEngine;
using System.Collections;

public class CompanionDropOff : MonoBehaviour 
{

	private GameObject droppedCompanion;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Companion")
		{
			droppedCompanion = other.gameObject;
			droppedCompanion.GetComponent<CompanionAI>().StopFollowing();
			droppedCompanion.GetComponent<CompanionAI>().atDropOff = true;
			droppedCompanion.GetComponent<CompanionAI>().DropOff = gameObject;
		}
		else if (other.tag == "Player" && droppedCompanion != null)
		{
			droppedCompanion.GetComponent<CompanionAI>().StartFollowing();
			droppedCompanion.GetComponent<CompanionAI>().atDropOff = false;
			droppedCompanion.GetComponent<CompanionAI>().DropOff = null;
			droppedCompanion = null;
		}
	}
	void OnTriggerExit (Collider other)
	{
		if(other.tag == "Companion")
		{
			droppedCompanion = other.gameObject;
			droppedCompanion.GetComponent<CompanionAI>().atDropOff = false;
		}
	}

}
