using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using Assets.CustomScripts;



public class Swtich_Gas : MonoBehaviour 
{
	[HideInInspector] public bool IsActive = false; //If the switch is active or not
	[HideInInspector] public bool Pressed = false; //IF the switch was just pressed
	[HideInInspector] public bool timedSwitch = false;
	public List<CircuitPath> Paths = new List<CircuitPath>();
	private float TimerStart = -1.0f;
	

	
	int buttonPresserCount = 0;

	[SerializeField] private bool requiresKey = false; //if this is true, requiredKey should not be 0
	[SerializeField] private int requiredKey = 0; //0 is generic, 1 is blue, 2 is yellow, 3 is cyan, 4 is magenta
	[SerializeField] private GameObject switchPanelBase;
	public GameObject[] LinkedObjects;
	
	AudioManager mAudioManager;
	public AudioClip mRightKeyEffect;
	public AudioClip mWrongKeyEffect;
	public float mDoorSoundEffectStartTimer;
	public float mDoorSoundEffectLength;
	bool mDoorSoundplaying = false;

	[SerializeField] private Material[] switchMaterials;


	private bool switchAnim = false;

	void Start()
	{

		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			Paths.Add(transform.GetChild(i).GetComponent<CircuitPath>());
		}
		foreach (CircuitPath path in Paths)
		{
			//path.bIsTimed = GetTimedSwitch(out path.mTimerLength);
		}
		mAudioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
		if (mAudioManager)
		{
			mAudioManager.AddSoundEffect("Door Correct", mRightKeyEffect);
			mAudioManager.AddSoundEffect("Door Incorrect", mWrongKeyEffect);
		}
	}
	void Update () 
	{
		if (mDoorSoundplaying)
		{
			if (Time.time > mDoorSoundEffectStartTimer + mDoorSoundEffectLength)
			{
				mDoorSoundplaying = false;
			}


		}
		Pressed = false; //@TODO maybe remove

		if (requiredKey >= 1) //replace with simpler assignment @TODO
		{
			switch (requiredKey)
			{
			case 1:
				switchPanelBase.renderer.material = switchMaterials[1];
				break;
			case 2:
				switchPanelBase.renderer.material = switchMaterials[2];
				break;
			case 3:
				switchPanelBase.renderer.material = switchMaterials[3];
				break;
			case 4:
				switchPanelBase.renderer.material = switchMaterials[4];
				break;
			default:
				break;
			}
		}

	}

	void OnTriggerEnter( Collider other)
	{
		
		if (other.GetComponent<Keybearer>() != null && other.GetComponent<Keybearer>().enabled)
		{

			if ( !requiresKey || (requiresKey && other.GetComponent<Keybearer>().posessedKey == requiredKey) )
			{
				if (!mDoorSoundplaying)
				{
					mAudioManager.PlaySoundEffect("Door Correct");
					mDoorSoundplaying = true;
					mDoorSoundEffectLength = mRightKeyEffect.length;
					mDoorSoundEffectStartTimer = Time.time;

					switchAnim = !switchAnim;
					if(switchAnim)
						GetComponent<Animator>().Play("turnOn");
					else
						GetComponent<Animator>().Play("turnOff");
				}
				Pressed = true;
				bool test = true;
				buttonPresserCount++;
				if (buttonPresserCount == 1)
				{

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
				if (!mDoorSoundplaying)
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