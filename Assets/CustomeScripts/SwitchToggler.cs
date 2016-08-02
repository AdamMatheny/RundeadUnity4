using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using Assets.CustomScripts;

namespace Assets.CustomScripts
{
	//Toggle switches the state of linked object on each press
	//Timed begins timer once switch is released to deactivate object
	//Pressure keeps object active only as long as the presser in on the switch
	//Locking will change the state of the paired objects once
	public enum SwitchType { Toggle, Timed, Pressure, Locking};
	public enum KeyColor { None, Blue, Yellow, Cyan, Magenta, Green };
}

public class SwitchToggler : MonoBehaviour 
{
	[HideInInspector] public bool IsActive = false; //If the switch is active or not
	[HideInInspector] public bool Pressed = false; //IF the switch was just pressed
	[HideInInspector] public bool timedSwitch = false;
	public List<CircuitPath> Paths = new List<CircuitPath>();
	private float TimerStart = -1.0f;
	private GameObject switchBlob;
	public SwitchType Type;

	int buttonPresserCount = 0;

	public float TimerLength = 3.0f;
	[SerializeField] private bool requiresKey = false; //if this is true, requiredKey should not be 0
	[SerializeField] private int requiredKey = 0; //0 is generic, 1 is blue, 2 is yellow, 3 is cyan, 4 is magenta
	public GameObject[] LinkedObjects;
	[SerializeField] private Material[] switchMaterials;

    AudioManager mAudioManager;
    public AudioClip mRightKeyEffect;
    public AudioClip mWrongKeyEffect;
    public float mDoorSoundEffectStartTimer;
    public float mDoorSoundEffectLength;
    bool mDoorSoundplaying = false;


	void Start()
	{
		timedSwitch = (Type == SwitchType.Timed);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			if (transform.GetChild(i).name != "SwitchBlob")
			{
				Paths.Add(transform.GetChild(i).GetComponent<CircuitPath>());
			}
		}
		foreach (CircuitPath path in Paths)
		{
			path.bIsTimed = GetTimedSwitch(out path.mTimerLength);
		}
        mAudioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
        if (mAudioManager)
        {
            mAudioManager.AddSoundEffect("Door Correct", mRightKeyEffect);
            mAudioManager.AddSoundEffect("Door Incorrect", mWrongKeyEffect);
        }
		switchBlob = transform.FindChild("SwitchBlob").gameObject;
		if(!GetComponent<MeshRenderer>().enabled)
			switchBlob.GetComponent<MeshRenderer>().enabled = false;
	}
	void Update () 
	{
		if(switchBlob == null)
		{
			switchBlob = transform.FindChild("SwitchBlob").gameObject;
		}
	
        if (mDoorSoundplaying)
        {
            if (Time.time > mDoorSoundEffectStartTimer + mDoorSoundEffectLength)
            {
                mDoorSoundplaying = false;
            }
        }

		if(Type == SwitchType.Timed)
		{
			TimerSwitchUpdate();
		}

		if (IsActive)
		{
			GetComponent<Light>().color = Color.green;
			switchBlob.renderer.material.color = Color.green;
		}
		else
		{
			GetComponent<Light>().color = Color.red;
			switchBlob.renderer.material.color = Color.red;
		}

		if (requiredKey >= 1) //replace with simpler assignment @TODO
		{
			switch (requiredKey)
			{
			case 1:
				renderer.material = switchMaterials[1];
				break;
			case 2:
				renderer.material = switchMaterials[2];
				break;
			case 3:
				renderer.material = switchMaterials[3];
				break;
			case 4:
				renderer.material = switchMaterials[4];
				break;
			default:
				break;
			}
		}
		Pressed = false; //@TODO maybe remove
	}
	private void TimerSwitchUpdate()
	{
		if (TimerStart > 0 && Time.time > TimerStart + TimerLength)
		{
			IsActive = false;
			TimerStart = -1.0f;
			DoorNavigation linkedToggler;
			for (int i = 0; i < LinkedObjects.Length; i++)
			{
				foreach (CircuitPath path in Paths)
				{
					path.bTimerRunning = false;
				}
				linkedToggler = LinkedObjects[i].GetComponent<DoorNavigation>();
				if (linkedToggler != null)
				{
					linkedToggler.ActiveSwitches--;
				}
			}
		}
	}
	void OnTriggerEnter( Collider other)
	{

		if (other.GetComponent<Keybearer>() != null && other.GetComponent<Keybearer>().enabled)
		{
			if ( !requiresKey || (requiresKey && other.GetComponent<Keybearer>().posessedKey == requiredKey) )
			{
				if (!mDoorSoundplaying && GetComponent<MeshRenderer>().enabled)
                {

                    mAudioManager.PlaySoundEffect("Door Correct");
                    mDoorSoundplaying = true;
                    mDoorSoundEffectLength = mRightKeyEffect.length;
                    mDoorSoundEffectStartTimer = Time.time;
                }
				Pressed = true;
				bool test = true;
				buttonPresserCount++;
				if (buttonPresserCount == 1)
				{
					switch (Type)
					{
						case SwitchType.Toggle:
							IsActive = !IsActive;
							break;
						case SwitchType.Timed:
							if (TimerStart != -1.0f)
							{
								test = false;
								TimerStart = -1.0f;
							}
							else
							{
								IsActive = true;
							}
							break;
						case SwitchType.Pressure:
							IsActive = true;
							break;
						case SwitchType.Locking:
							if(!IsActive)
							{
								IsActive = true;
							}
							else
							{
								test = false;
							}
							break;
						default:
							break;
					}
					if (test)
					{
						//IsActive = false; //Adjust so zombie doesnt change color ATTENTION
						Toggler linkedToggler;
						for (int i = 0; i < LinkedObjects.Length; i++)
						{
							linkedToggler = LinkedObjects[i].GetComponent<Toggler>();
							if (linkedToggler != null)
							{
								linkedToggler.Activate(other, gameObject);
							}
						}
					}
				}
			}
            else
            {
				if (!mDoorSoundplaying && GetComponent<MeshRenderer>().enabled)
                {
                    mAudioManager.PlaySoundEffect("Door Incorrect");
                    mDoorSoundplaying = true;
                    mDoorSoundEffectLength = mWrongKeyEffect.length;
                    mDoorSoundEffectStartTimer = Time.time;
                }
            }
		}
	}
	void OnTriggerExit( Collider other)
	{
		if (other.GetComponent<Keybearer>() != null && other.GetComponent<Keybearer>().enabled)
		{
			if ( !requiresKey || (requiresKey && other.GetComponent<Keybearer>().posessedKey == requiredKey) )
			{
				buttonPresserCount --;
				if (buttonPresserCount < 0)
				{
					buttonPresserCount = 0;
				}
				if (buttonPresserCount == 0)
				{
					Pressed = false;
					switch (Type)
					{
						case SwitchType.Toggle:
							//Change it to on or off
							break;
						case SwitchType.Timed:
							//If still on refresh timer
							TimerStart = Time.time;
							foreach(CircuitPath path in Paths)
							{
								path.bTimerRunning = true;
							}
							
							break;
						case SwitchType.Pressure:
							//Activate normally 
							IsActive = false;
							Toggler pressureLinkedToggler;
							for (int i = 0; i < LinkedObjects.Length; i++)
							{
							pressureLinkedToggler = LinkedObjects[i].GetComponent<Toggler>();
							if (pressureLinkedToggler != null)
								{
								pressureLinkedToggler.Activate(other, gameObject);
								}
							}
						break;
						case SwitchType.Locking:
							break;
						default:
							break;
					}
					Toggler linkedToggler;
					for (int i = 0; i < LinkedObjects.Length; i++)
					{
						linkedToggler = LinkedObjects[i].GetComponent<Toggler>();
						if (linkedToggler != null)
						{
							linkedToggler.Deactivate(other, gameObject);
						}
					}
				}
			}
		}
	}
	public bool GetTimedSwitch(out float timer)
	{
		timer = TimerLength;
		return (Type == SwitchType.Timed);
	}
	public float GetTimeRemaining()
	{
		float result = 0;
		if ((TimerStart + TimerLength) - Time.time > 0)
		{
			result = (TimerStart + TimerLength) - Time.time;
		}
		return result;
	}

	public void AddPath()
	{
#if UNITY_EDITOR
		CircuitPath newPath;

		//Starting rotation of the joint

		newPath = (CircuitPath)PrefabUtility.InstantiatePrefab(Resources.Load<CircuitPath>("Prefabs/CircuitPath"));
		newPath.transform.position = transform.position;
		newPath.transform.parent = transform;
		newPath.name = "Path" + gameObject.transform.childCount.ToString();
#endif
	}
}