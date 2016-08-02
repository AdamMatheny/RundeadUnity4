using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using Assets.CustomScripts;


public class ZombieTrap : MonoBehaviour 
{
	[HideInInspector] public bool IsActive = false; //If the switch is active or not
	[HideInInspector] public bool Pressed = false; //IF the switch was just pressed
	[HideInInspector] public bool timedSwitch = false;
	private float TimerStart = -1.0f;

	int buttonPresserCount = 0;


	public GameObject[] LinkedObjects;


	
	
	void Start()
	{
	}
	void Update () 
	{

//		Pressed = false; //@TODO maybe remove
	}

	void OnTriggerEnter( Collider other)
	{
		
		if (other.GetComponent<ZombieAI>() != null && other.GetComponent<ZombieAI>().enabled)
		{

			Pressed = true;
			bool test = true;
			buttonPresserCount++;
			//other.GetComponent<Stunable>().invincibleDuration = 0f;
			//other.GetComponent<Stunable>().Stun();
			//other.GetComponent<Stunable>().bStayStunned=true;

			IsActive = true;

			//IsActive = false; //Adjust so zombie doesnt change color ATTENTION
			DoorNavigation linkedDoor;
			for (int i = 0; i < LinkedObjects.Length; i++)
			{
				linkedDoor = LinkedObjects[i].GetComponent<DoorNavigation>();
				if (linkedDoor != null)
				{
					linkedDoor.ActiveSwitches++;
					if (linkedDoor.ActiveSwitches >= linkedDoor.NeededSwitches)
					{
						linkedDoor.open=true;
					}
				}
			}


		}

		
	}
	void OnTriggerExit( Collider other)
	{
		if (other.GetComponent<ZombieAI>() != null && other.GetComponent<ZombieAI>().enabled)
		{
			
			buttonPresserCount--;
			//other.GetComponent<Stunable>().invincibleDuration = 1f;
			//other.GetComponent<Stunable>().bStayStunned=false;
			

			//IsActive = false; //Adjust so zombie doesnt change color ATTENTION
			DoorNavigation linkedDoor;
			for (int i = 0; i < LinkedObjects.Length; i++)
			{
				linkedDoor = LinkedObjects[i].GetComponent<DoorNavigation>();
				if (linkedDoor != null)
				{
					linkedDoor.ActiveSwitches--;
					if (linkedDoor.ActiveSwitches < linkedDoor.NeededSwitches)
					{
						linkedDoor.open=false;
					}
				}
			}
		}
	}

}