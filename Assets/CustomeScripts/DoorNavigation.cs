using UnityEngine;
using System.Collections;

public class DoorNavigation : Toggler 
{

	//public bool toggleOpen = false; //for getting toggled open and closed by non-timed switches

	public int ActiveSwitches = 0;
	[HideInInspector] public float TimerStart = -1.0f;
	[HideInInspector] public bool open = false;
	[HideInInspector] public bool toggleOpen = false;
	[HideInInspector] public bool openedBySwitch = false;
	private bool openByProximity = false;
	private bool timedSwitch = false;
	private float jumpDistance;
	private Vector3 jumpPoint;

	[SerializeField] private string doorID = "";
	[SerializeField] private bool StartOpen = false;
	[SerializeField] private bool leaveOpen = false;
	[SerializeField] private bool useRemoteSwitchOnly = false;
	public int NeededSwitches = 0;
	[SerializeField] private float TimerLength = 0.5f;
	[SerializeField] private bool requiresKey = false; //if this is true, requiredKey should not be 0
	[SerializeField] private int requiredKey = 0; //0 is generic, 1 is blue, 2 is yellow, 3 is cyan, 4 is magenta	
	[SerializeField] private Transform[] shuntPoints;//for pushing out zombies that get stuck in the door
	[SerializeField] private GameObject doorAnimator;

	[SerializeField] private GameObject doorMesh;//for changing the color of the door based on required key
	[SerializeField] private GameObject[] sideWalls;//for changing the color of the door based on whether a switch is required

    AudioManager mAudioManager;
    public AudioClip mSoundEffect;
    public bool mWasOpen = false;
    public float mSoundEffectStartTimer;
    bool mSoundEffectPlaying = false;

    public bool mWaitforDoor = false;
    public float mWaitTimer = 0.0f; 

	// Use this for initialization
	void Start () 
	{
		if(StartOpen)
		{
			//open = true;
            if (mWaitforDoor)
            {
                Invoke("DoorTimer", mWaitTimer);
            }
            else
            {
                open = true;
            }
		}
		if (requiresKey)
		{
		switch (requiredKey)
			{
				case 0:
					GetComponentInChildren<Light>().color = Color.white;
					doorMesh.renderer.material.color = Color.white;
					break;
				case 1:
					GetComponentInChildren<Light>().color = Color.blue;
					doorMesh.renderer.material.color = Color.blue;
					break;
				case 2:
					GetComponentInChildren<Light>().color = Color.yellow;
					doorMesh.renderer.material.color = Color.yellow;
					break;
				case 3:
					GetComponentInChildren<Light>().color = Color.cyan;
					doorMesh.renderer.material.color = Color.cyan;
					break;
				case 4:
					GetComponentInChildren<Light>().color = Color.magenta;
					doorMesh.renderer.material.color = Color.magenta;
					break;
				case 5:
					GetComponentInChildren<Light>().color = Color.green;
					doorMesh.renderer.material.color = Color.green;
					break;
				default:
					GetComponentInChildren<Light>().color = Color.white;
					doorMesh.renderer.material.color = Color.white;
					break;
			}
        
		}
		//set wall colors if a remote switch is required
		if (useRemoteSwitchOnly)
		{
			if (open)
			{
				sideWalls[0].renderer.material.color = Color.green;
				sideWalls[1].renderer.material.color = Color.green;
			}
			else
			{
				sideWalls[0].renderer.material.color = Color.red;
				sideWalls[1].renderer.material.color = Color.red;
			}
		}
        mAudioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
        if (mAudioManager)
        {
            mAudioManager.AddSoundEffect("Door Open", mSoundEffect); 
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (mSoundEffectPlaying)
        {
            if (Time.time > mSoundEffectStartTimer + mSoundEffect.length)
            {
                mSoundEffectPlaying = false;
            }
        }
		if (!leaveOpen) 
		{
			if (NeededSwitches > 0)
			{
				if(ActiveSwitches - NeededSwitches < 0)
				{
					if (useRemoteSwitchOnly)
					{
						open = openedBySwitch;
					}
					else if (!openByProximity)
					{
						open = false;
					}
				}
			}
			else
			{
				if (ActiveSwitches - NeededSwitches <= 0)
				{
					if (useRemoteSwitchOnly)
					{
						open = openedBySwitch;
					}
					else if (!openByProximity)
					{
						open = false;
					}
				}
			}
		}

		if(TimerStart > 0 && Time.time > TimerStart + TimerLength)
		{
			open = false;
			TimerStart = -1.0f;
		}

        //if (open && !mWaitforDoor)
        if (open)
		{
			doorAnimator.GetComponent<Animator>().Play("Open");
            if ((!mSoundEffectPlaying) && !leaveOpen)
            {
                mAudioManager.PlaySoundEffect("Door Open");
                mWasOpen = true;
                mSoundEffectPlaying = true;
                mSoundEffectStartTimer = Time.time;
            }
            
			//	//GetComponent<MeshRenderer>().enabled = false;
		}
        //else if (open)
        //{
        //    Invoke("DoorTimer", mWaitTimer);
        //}
		else
		{
			doorAnimator.GetComponent<Animator>().Play("Close");
            if ((mWasOpen) && (!mSoundEffectPlaying))
            {
                mAudioManager.PlaySoundEffect("Door Open");
                mWasOpen = false;
                mSoundEffectPlaying = true;
                mSoundEffectStartTimer = Time.time;
            }
			//GetComponent<MeshRenderer>().enabled = true;
		}
		//set wall colors if a remote switch is required
		if (useRemoteSwitchOnly)
		{
			if (open == true)
			{
				sideWalls[0].renderer.material.color = Color.green;
				sideWalls[1].renderer.material.color = Color.green;
			}
			else
			{
				sideWalls[0].renderer.material.color = Color.red;
				sideWalls[1].renderer.material.color = Color.red;
			}
		}

	}

	void OnTriggerEnter( Collider other)
	{
		if (!useRemoteSwitchOnly && (other.GetComponent<Keybearer>() != null && other.GetComponent<Keybearer>().enabled == true) && (requiresKey == false || (requiresKey == true && requiredKey == other.GetComponent<Keybearer>().posessedKey) ) )
		{
			openByProximity = true;
			TimerStart = -1.0f;
            //open = true;
            if (mWaitforDoor)
            {
                Invoke("DoorTimer", mWaitTimer);
            }
            else
            {
                open = true;
            }
			
		}
		else if (!open && other.GetComponent<NavMeshAgent>() != null)
		{
			//Debug.Log("Door not opening");
			other.GetComponent<NavMeshAgent>().Stop();
			other.GetComponent<NavMeshAgent>().ResetPath();
			//Debug.Log("We should be stopping the player");
			// for making things not get stuck in doors:
			DoorShunt(other, (Time.time < TimerLength+TimerStart+0.5f) );
		}
	}
	//void OnTriggerStay( Collider other)
	//{
		//Keybearer keyHolder = other.GetComponent<Keybearer>();
		//NavMeshAgent navAgent = other.GetComponent<NavMeshAgent>();
		//if (!useRemoteSwitchOnly && (keyHolder != null && keyHolder.enabled == true) && (requiresKey == false || (requiresKey == true && requiredKey == keyHolder.posessedKey)))
		//{
		//	//if (!open)
		//	//{
		//	//	OpenDoor(TimerLength);
		//	//}
		//	open = true;
		//}
		//else if (!open && navAgent != null)
		//{
		//	if (navAgent.enabled == true)
		//	{
		//		navAgent.Stop();
		//		navAgent.ResetPath();
		//		//Debug.Log("We should be stopping the player");
		//	}
		//	// for making things not get stuck in doors:
		//	DoorShunt(other, (Time.time < TimerLength+TimerStart+0.1f));
		//}

	//}
	void OnTriggerExit( Collider other)
	{
		
		if (!useRemoteSwitchOnly && (other.GetComponent<Keybearer>() != null && other.GetComponent<Keybearer>().enabled == true) && (requiresKey == false || (requiresKey == true && requiredKey == other.GetComponent<Keybearer>().posessedKey)))
		{
			openByProximity = false;
			if (!openedBySwitch)
			{ 
				open = false;
			}
			TimerStart = Time.time;
		}
	}
	public void OpenDoor( float timeToStayOpen )
	{
		TimerLength = timeToStayOpen;
		//TimerStart = Time.time;
		
        //open = true;
        if (mWaitforDoor)
        {
            Invoke("DoorTimer", mWaitTimer);
        }
        else
        {
            open = true;
        }

        //mAudioManager.PlaySoundEffect("Door Open");
        //Assets.CustomeScripts.Metrics.AddMetric("Door Opened, Door ID was:", doorID);
	}

	void DoorShunt (Collider other, bool doorJustClosed)
	{
		for (int i = 0; i < shuntPoints.Length; i++)
		{
			if (i == 0)
			{
				jumpPoint = shuntPoints[i].position;
				jumpDistance = Vector3.Distance(other.transform.position, shuntPoints[i].position);
			}
			else
			{
				if (!doorJustClosed)
				{
					if (jumpDistance > Vector3.Distance(other.transform.position, shuntPoints[i].position))
					{
						jumpPoint = shuntPoints[i].position;
						jumpDistance = Vector3.Distance(other.transform.position, shuntPoints[i].position);
					}
				}
				else if (other.tag == "Player" || other.tag == "Companion")
				{
					if (jumpDistance > Vector3.Distance(other.GetComponent<NavMeshAgent>().destination, shuntPoints[i].position))
					{
						jumpPoint = shuntPoints[i].position;
						jumpDistance = Vector3.Distance(other.GetComponent<NavMeshAgent>().destination, shuntPoints[i].position);
					}
				}
				else
				{
					GameObject playerAvatar = GameObject.FindWithTag("Player");
					//Debug.Log("Caught zombie in door.");
					if (jumpDistance < Vector3.Distance(playerAvatar.transform.position, shuntPoints[i].position))
					{
						jumpPoint = shuntPoints[i].position;
						jumpDistance = Vector3.Distance(other.GetComponent<NavMeshAgent>().destination, shuntPoints[i].position);
					}
				}
			}

		}
		other.transform.position = jumpPoint;
	}

	public override bool Activate(Collider other, GameObject flippedSwitch)
	{
		bool active = false;
		SwitchToggler theSwitch = flippedSwitch.GetComponent<SwitchToggler>();
		if(theSwitch.IsActive)
		{
			ActiveSwitches++;
		}
		else
		{
			ActiveSwitches--;
		}
		active = theSwitch.IsActive;
				
		//After modifying it see if the door still needs to be opened
		if (ActiveSwitches - NeededSwitches == 0)
		{
					
			//open the door on a timer if timedSwitch is true
			if (theSwitch != null &&
				theSwitch.timedSwitch)
			{
				//OpenDoor(theSwitch.TimerLength);

				//open = true;
                if (mWaitforDoor)
                {
                    Invoke("DoorTimer", mWaitTimer);
                }
                else
                {
                    open = true;
                }
				toggleOpen = false;
			}
			//toggle the door if timedSwitch is false
			else if (open == false)
			{
				openedBySwitch = true;
				//open = true;
                if (mWaitforDoor)
                {
                    Invoke("DoorTimer", mWaitTimer);
                }
                else
                {
                    open = true;
                }
				toggleOpen = true;
			}
		}
		else if(ActiveSwitches > NeededSwitches)
		{
			open = !open;
			openedBySwitch = !openedBySwitch;
			toggleOpen = !toggleOpen;
		}
		else
		{
			if (open == true && !leaveOpen)
			{
				openedBySwitch = false;
				open = false;
				toggleOpen = false;
			}
		}
base.Activate(other, flippedSwitch);
		return active;
	}


	public override bool Reactivate(Collider other, GameObject flippedSwitch)
	{
		bool reactive = false;
		//if ((other.GetComponent<Keybearer>() != null && other.GetComponent<Keybearer>().enabled == true))
		//{
		//	if ((requiresKey == false) || (requiresKey == true && requiredKey == other.GetComponent<Keybearer>().posessedKey))
		//	{
		//		if (NeededSwitches == 0)
		//		{
		//			//open the door on a timer if timedSwitch is true
		//			if (flippedSwitch.GetComponent<SwitchToggler>() != null &&
		//				flippedSwitch.GetComponent<SwitchToggler>().timedSwitch)
		//			{
		//				OpenDoor(flippedSwitch.GetComponent<SwitchToggler>().TimerLength);
		//				toggleOpen = false;
		//			}

		//			reactive = base.Reactivate(other, flippedSwitch);
		//		}
		//	}
		//}
		return reactive;
	}
	public override bool Deactivate(Collider other, GameObject flippedSwitch)
	{
		bool deactive = false;
		if ((other.GetComponent<Keybearer>() != null && other.GetComponent<Keybearer>().enabled == true && useRemoteSwitchOnly == true))
		{
			if ((requiresKey == false) || (requiresKey == true && requiredKey == other.GetComponent<Keybearer>().posessedKey))
			{
				deactive = base.Deactivate(other, flippedSwitch);	
			}
		}
		return deactive;
	}

    public void DoorTimer()
    {
        mWaitforDoor = false;
        open = true;
    }
}
