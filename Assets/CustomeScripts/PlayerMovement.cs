using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerMovement : MonoBehaviour 
{
	//get this when you want to move the player via nav mesh
	NavMeshAgent agent;
	//telling the player where to move to
	Vector3 clickToMoveTarget;
	//telling the player how far to move with WASD
	public float speed = 3.5f;
	public float acceleration = 8.0f;
    //bool telling whether to boost the players speed or not
    bool mBoostSpeed = false;
    //Timer for the speed boost
    private float mTimer = 60.0f;

    //Gas Timer to be used, set in the editor
    public float mGasTimer = 10.0f;
    public float mGasMaskTimer = 10.0f;
    public float mInitialGasTimer;
    public float mGasStartTime {get;set;}
    public bool mInGas = false;
	[HideInInspector] public int mNumGases = 0;
	//For displaying the remaining breath while in gas
	public float mGasBarLength;
	public Texture2D mGasBarTexture;
	public Texture2D mGasMaskBarTexture;

	public bool accelNeutral = true;
	public static bool canTilt = false; //whether or not tilt-to-move is enabled
	public static bool clickToPathfind = false;
	public static bool useJoystick = false;

	//Shock Timer to be used, set in the editor
	public float mShockTimer = 3.0f;
	public float mInitialShockTimer;
	public float mShockStartTime {get;set;}
	public bool mInShock = false;
	[HideInInspector] public int mNumShocks = 0;
	//For displaying the remaining shock tolerance while on a shock panel
	public float mShockBarLength;
	public Texture2D mShockBarTexture;

	//Allows this to be stunable
	Stunable mStunCheck;

	//Shields Yay
	public int NumberofShields {get;set;}

	
    //GasMasks Yay
    public bool mHasGasMask {get;set;}
    public List<Gas> mGasesVisited = new List<Gas>();
	
	//The big yellow bobbing arrow
	public GameObject clickToMoveIndicator;

	//whether or not the mouse is over a GUI button
	public bool mouseOverGUI = false;

    AudioManager mAudioManager;
    public AudioClip mCoughSoundEffect;
    public float mCoughSoundEffectStartTimer;
    bool mCoughplaying = false;
    public AudioClip mShockSoundEffect;
    public float mShockSoundEffectStartTimer;
    bool mShockplaying = false;

    public AudioClip mSIDDieEffect;
    public AudioClip mSIDHoverEffect;
    public float mSidDieStartTimer;
    public float mSidHoverStartTimer;
    public bool mSidDied = false;
    public bool mSidHover = false;

	Animator anim;
	public GameObject Sid;
	bool waitToReturn = false;

	MapConsole levelMap;

    public bool mShowGUI = true;

	public CompanionAI myCompanion;

	// Use this for initialization
	void Start () 
	{
		NumberofShields = 0;
		agent = GetComponent<NavMeshAgent>();
		mStunCheck = GetComponent<Stunable>();
		Object pointerArrow = Instantiate(Resources.Load("Prefabs/ClickToMoveTarget", typeof(GameObject)));
		pointerArrow.name = "ClickToMoveIndicator";
		clickToMoveIndicator = GameObject.Find("ClickToMoveIndicator");
        mTimer = 0.0f;
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			//canTilt = true;
		}
		//Respawn with checkpoint put in start
        mInitialGasTimer = mGasTimer;
        mGasBarLength = mInitialGasTimer;
		mInitialShockTimer = mShockTimer;
		mShockBarLength = mInitialShockTimer;
		anim = GetComponent<Animator>();
		anim.applyRootMotion = false;
		Sid = GameObject.Find("Sid_All_Anim");

        mAudioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
        if (mAudioManager)
        {
            mAudioManager.AddSoundEffect("Cough", mCoughSoundEffect);
            mAudioManager.AddSoundEffect("Shock", mShockSoundEffect);
            mAudioManager.AddSoundEffect("Sid Hover", mSIDHoverEffect);
            mAudioManager.AddSoundEffect("Sid Die", mSIDDieEffect);
        }

		levelMap = FindObjectOfType(typeof(MapConsole)) as MapConsole;

	}
	
	// Update is called once per frame
	void Update()
	{
        if (agent.velocity == Vector3.zero)
        {
            mBoostSpeed = false;
        }

        if (mCoughplaying)
        {
            if (Time.time > mCoughSoundEffectStartTimer + mCoughSoundEffect.length)
            {
                mCoughplaying = false;
            }
        }

        if (mShockplaying)
        {
            if (Time.time > mShockSoundEffectStartTimer + mShockSoundEffect.length)
            {
                mShockplaying = false;
            }
        }

        if ((NumberofShields > 0) && (mSidHover))
        {
            if (Time.time > mSidHoverStartTimer + mSIDHoverEffect.length)
            {
                mSidHover = false;
            }
        }

        if (mSidDied)
        {
            if (Time.time > mSidDieStartTimer + mSIDDieEffect.length)
            {
                mSidDied = false;
            }
        }

        if ((NumberofShields > 0) && (!mSidHover))
        {
            mAudioManager.PlaySoundEffect("Sid Hover");
            mSidHover = true;
            mSidHoverStartTimer = Time.time;
        }

		//Decreasing breath while in gas
        if (mInGas)
        {
            if (mHasGasMask)
            {
                mGasBarLength = (mGasMaskTimer - (Time.time - mGasStartTime)) * 10;
                if (Time.time > mGasStartTime + mGasMaskTimer)
                {
                    mHasGasMask = false;
                    foreach (Gas gas in mGasesVisited)
                    {
                        gas.mGasMaskAffected = false;
                    }
                    mGasStartTime = Time.time;
                }
            }
            else
            {
                if (!mCoughplaying)
                {
                    mAudioManager.PlaySoundEffect("Cough");
                    mCoughplaying = true;
                    mCoughSoundEffectStartTimer = Time.time;
                }
                
                mGasBarLength = (mGasTimer - (Time.time - mGasStartTime)) * 10;
                if (Time.time > mGasStartTime + mGasTimer)
                {
                    mAudioManager.audio.Stop();
                    mAudioManager.PlaySoundEffect("Zombie Growl");
                    mInGas = false;
                    playDeathAnim();
                    agent.enabled = false;
                    GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                    foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
                    {
                        mesh.enabled = false;
                    }
                    GameObject zombie = (GameObject)Instantiate(Resources.Load("Zombie"));
                    zombie.transform.position = this.gameObject.transform.position;
                    zombie.transform.rotation = this.gameObject.transform.rotation;
                    Invoke("KilledbyGas", 2);
                    //Application.LoadLevel(Application.loadedLevel);
                   // Assets.CustomeScripts.Metrics.AddMetric("Killer ID was:", "The Gas");
                }
            }
            
        }


		//Decreasing shock tolerance while on a shock panel
		if (mInShock)
		{
			if (!mShockplaying)
			{
				mAudioManager.PlaySoundEffect("Shock");
				mShockplaying = true;
				mShockSoundEffectStartTimer = Time.time;
				anim.SetBool("ShockBool", true);
			}

			mShockBarLength = (mShockTimer - (Time.time - mShockStartTime)) * 10;
			if (Time.time > mShockStartTime + mShockTimer)
			{
				mAudioManager.audio.Stop();
				//mAudioManager.PlaySoundEffect("Zombie Growl");
				mInShock = false;
				playDeathAnim();
				agent.enabled = false;

				Invoke("KilledbyShock", 2);
				//Application.LoadLevel(Application.loadedLevel);
				//Assets.CustomeScripts.Metrics.AddMetric("Killer ID was:", "The Shock");
			}
			
			
		}

		if (!mStunCheck.IsStunned)
		{
			if (agent.enabled == true){
				agent.Resume();
				anim.SetBool("ShockBool", false);
			}
				
			mStunCheck.UpdateStunInvinciblity();

			if (mBoostSpeed == true)
			{
				decreaseSpeedBoostTimer();
			}
			else
			{
				agent.speed = speed;
				agent.acceleration =acceleration;
			}

		//Click-To-Move controls
			if (Input.GetMouseButton(0) && agent.enabled == true && !useJoystick)
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.tag != "Companion" && !mouseOverGUI)
					{
						if (clickToPathfind)
						{
							clickToMoveTarget = hit.point;
							clickToMoveIndicator.GetComponent<ClickToMovePointer>().RevealAtLocation(clickToMoveTarget);
							clickToMoveTarget.y = 0;// clickToMoveTarget - new Vector3(0, clickToMoveTarget.y, 0);
							agent.SetDestination(clickToMoveTarget);
						}
						else
						{
							Ray pathToPoint = new Ray(transform.position, hit.point-transform.position);
							clickToMoveTarget.y = 0;// = clickToMoveTarget - new Vector3(0, clickToMoveTarget.y, 0);
							clickToMoveTarget = pathToPoint.GetPoint(1f);
							clickToMoveIndicator.GetComponent<ClickToMovePointer>().RevealAtLocation(hit.point);
							agent.SetDestination(clickToMoveTarget);
						}
					}
				}
				//else
					//anim.SetFloat("Speed", 0f);
			}//End of Click-To-Move controls

			//Virtual Joystick Controls
			if (Input.GetMouseButton(0) && agent.enabled == true && useJoystick)
			{
				Vector3 joystickDestination = transform.position + (new Vector3(Joystick.mNormals.x, 0, Joystick.mNormals.y));
				agent.SetDestination(joystickDestination);
				Debug.Log("Using Joystick to Move");
			}

			float vertAccel = Input.acceleration.y + 0.5f;
			if ((Mathf.Abs(Input.acceleration.x) > 0.2f || Mathf.Abs(vertAccel) > 0.2f) && canTilt)
			{
				accelNeutral = false;
				clickToMoveIndicator.GetComponent<ClickToMovePointer>().Hide();
			}
			else
			{
				accelNeutral = true;
			}

			if (!accelNeutral && !((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) || Input.GetMouseButton(0)))
			{

				float horzMove = Input.acceleration.x/Mathf.Abs(Input.acceleration.x);
				float vertMove = vertAccel/Mathf.Abs(vertAccel);

				if (Mathf.Abs(Input.acceleration.x) <= 0.2f)
				{
					horzMove = 0f;
				}

				if (Mathf.Abs(vertAccel) <= 0.2f)
				{
					vertMove = 0f;
				}
				
				clickToMoveTarget = transform.position + new Vector3(horzMove * speed, 0, vertMove * speed);
				if (agent.enabled == true)
					agent.SetDestination(clickToMoveTarget);
			}

		//WASD controls
			if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
			{

				clickToMoveIndicator.GetComponent<ClickToMovePointer>().Hide();
				clickToMoveTarget = transform.position + new Vector3(Input.GetAxis("Horizontal") * 1f, 0, Input.GetAxis("Vertical") * 1f);
				if (agent.enabled == true)
					agent.SetDestination(clickToMoveTarget);
			}



			
		//Hide the arrow if the player is at the arrow
			if (Vector3.Distance(this.transform.position, clickToMoveIndicator.transform.position) < 1f)
			{
				//Debug.Log("At the arrow.");
				clickToMoveIndicator.GetComponent<ClickToMovePointer>().Hide();
				
			}
			
			if (!agent.hasPath)
			{
				clickToMoveIndicator.GetComponent<ClickToMovePointer>().Hide();
				anim.SetFloat("Speed", 0f);
			}
			else
				anim.SetFloat("Speed", 2f);
		}
		else
		{
			mStunCheck.UpdateStunnedTimer();
			if (agent.enabled == true)
			{
				agent.Stop();
			}
		}
	}

	public void playDeathAnim()
	{
		//anim.SetBool("Death", true);
		anim.Play("Death");
	}
	

    //increase speed if colliding with speed boosting slime
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Puddle")
        {
            mBoostSpeed = true;
            agent.speed = other.GetComponent<SpeedBoost>().mSpeed;
            agent.acceleration = other.GetComponent<SpeedBoost>().mAcceleration;
            mTimer = other.GetComponent<SpeedBoost>().mTimer;
        }
		else if (other.tag == "Gas")
		{
			mNumGases++;
		}
        else if ((other.tag != "Companion") && (other.tag != "Dialog") && (other.tag != "Gas") && mBoostSpeed)
        {
            mBoostSpeed = false;
        }
    }

    //decrease the timer for the speed boost
    void decreaseSpeedBoostTimer()
    {
        mTimer--;
        if(mTimer <= 0.0f)
        {
            mBoostSpeed = false;
        }
    }

	//Trigger by the dinos and the zombies when the player dies
	public void PlayerDeath(GameObject killer, string killerID)
	{
		//Check to see if the player has a shield already
		if (NumberofShields > 0 && (killerID != "Killed by Gas" && killerID != "Killed by Shock"))
		{
			Stunable stunOther = killer.gameObject.GetComponent<Stunable>();
			if (stunOther != null)
			{
				stunOther.Stun();
				Sid.transform.position = killer.transform.position;
				Sid.GetComponent<Animator>().Play("Death");
				Sid.transform.position = killer.transform.position;

			}
			Sid.transform.position = killer.transform.position;
			StartCoroutine(WaitForAnim(killer));
		}
		else
		{
			//Assets.CustomeScripts.Metrics.AddMetric("Killer ID was:", killerID);
			FindObjectOfType<CameraFade>().sceneEnding = true;
		}

	}// end player death

	IEnumerator WaitForAnim(GameObject kill) 
	{
		Sid.transform.position = kill.transform.position;
		yield return new WaitForSeconds(.5f);
		--NumberofShields;
        if (!mSidDied)
        {
            mAudioManager.PlaySoundEffect("Sid Die");
            mSidDied = true;
            mSidDieStartTimer = Time.time;
        }
	}

	void toggleTilt()
	{
		int fingerCount = 0;
		foreach (Touch touch in Input.touches) 
		{
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				fingerCount++;
			
		}
		if (fingerCount == 2 )
		{

			if (canTilt)
				canTilt = false;
			else
				canTilt = true;
		
		}
	}

	void OnGUI()
	{
        if (mShowGUI)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {

                /*if (canTilt)
                {
                    if (GUI.Button(new Rect(0,Screen.height-Screen.height/5, Screen.width/20, Screen.height/10 ), new GUIContent("Tilt is on", "This is the tooltip") ) )
                    {
                        canTilt = false;
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(0,Screen.height-Screen.height/5, Screen.width/20, Screen.height/10 ), new GUIContent("Tilt is off", "This is the tooltip") ) ) 
                    {
                        canTilt = true;
                    }
                }*/
            }

            if (levelMap != null && levelMap.showMap == true)
            {
                //Debug.Log("Displaying button to HIDE map");
               /* if (GUI.Button(new Rect(0, Screen.height * 0.6f, Screen.width * 0.1f, Screen.height * 0.025f), new GUIContent("Hide Map", "This is the tooltip")))
                {
                    levelMap.showMap = false;

                }
               // GUI.Box(new Rect(0, Screen.height * 0.65f, Screen.width * 0.25f, Screen.height * 0.25f), "");

            }
            else if (levelMap != null && levelMap.showMap == false)
            {
                //Debug.Log("Displaying button to SHOW map");
                if (GUI.Button(new Rect(0, Screen.height * 0.6f, Screen.width * 0.1f, Screen.height * 0.025f), new GUIContent("Show Map", "This is the tooltip")))
                {
                    levelMap.showMap = true;


                }*/
            }

            if (Event.current.type == EventType.Repaint)
            {
                if (GUI.tooltip != "")
                {
                    mouseOverGUI = true;
                }
                else
                {
                    mouseOverGUI = false;
                }
            }


            if (mInGas)
            {
                if (mHasGasMask)
                {
                    GUI.DrawTexture(new Rect(Screen.width / 2, 10, mGasBarLength, 20), mGasMaskBarTexture);
                }
                else
                {
                    GUI.DrawTexture(new Rect(Screen.width / 2, 10, mGasBarLength, 20), mGasBarTexture);
                }

            }
            if (mInShock)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2, 10, mShockBarLength, 20), mShockBarTexture);
            }
        }
		
	}

    void KilledbyGas()
    {
        PlayerDeath(this.gameObject, "Killed by Gas");
    }
	void KilledbyShock()
	{
		PlayerDeath(this.gameObject, "Killed by Shock");
	}

}
