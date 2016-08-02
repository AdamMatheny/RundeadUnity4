using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

public class ZombieAI : MonoBehaviour 
{
	[SerializeField] private string zombieID = "";
	//get this when you want to move the player via nav mesh
	NavMeshAgent agent;
	//telling the player where to move to
	Vector3 moveTarget;
	GameObject victim;
	[SerializeField] private float chaseDistance = 8f;
	Stunable mStunCheck;
    public AudioClip mGrowl;
    bool mAttacked;
	private bool canLunge = true;
	[SerializeField] private bool canEverLunge = true;
	private float lungeChargeTime = 0.5f;
	private float lungeStartTime = 0f;
	private float lungeCooldown = 10f;
	private bool eatingThings = false;
	AudioManager mAudioManager;
	private GameObject playerAvatar;
	public List<GameObject> lunchTargets;

	[SerializeField] private float defaultSpeed = 3f;
	[SerializeField] private float lungeSpeed = 6;
	[SerializeField] private float defaultAccel = 8f;
	[SerializeField] private float lungeAccel = 20;
	private Vector3 lungeTarget;

	private Animator anim;
	private PlayerMovement playerMovement;
	// Use this for initialization
	void Start () 
	{
		lunchTargets = new List<GameObject>();
		agent = GetComponent<NavMeshAgent>();
		mStunCheck = GetComponent<Stunable>();
		playerAvatar = GameObject.Find("PlayerAvatar");
        mAttacked = false;
        mAudioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
        if (mAudioManager)
        {
            mAudioManager.AddSoundEffect("Zombie Growl", mGrowl);
        }
		lunchTargets.Add(GameObject.Find("PlayerAvatar"));
		CompanionAI[] companions = FindObjectsOfType(typeof(CompanionAI)) as CompanionAI[];
		foreach (CompanionAI companion in companions)
		{
			if (!companion.vitalNPC)//For making the Companion invulnerable
			{
				lunchTargets.Add(companion.gameObject);
			}
		}
		anim = GetComponent<Animator>();
		anim.applyRootMotion = false;
		playerMovement = lunchTargets[0].GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!mAttacked)
		{
			moveTarget = lunchTargets[0].transform.position;
			agent.speed = defaultSpeed;
			agent.acceleration = defaultAccel;
			if ((Time.time > lungeCooldown + lungeStartTime) && canEverLunge)
			{
				canLunge = true;
			}
		}
		else
		{
			if (Time.time > lungeChargeTime + lungeStartTime)
			{
				agent.speed = lungeSpeed;
				agent.acceleration = lungeAccel;
				Ray pathToPoint = new Ray(transform.position, lungeTarget-transform.position);
				lungeTarget = pathToPoint.GetPoint(2f);
				anim.SetBool("Dive", true);
				moveTarget = lungeTarget;
			}
			if (Time.time > lungeChargeTime*2f + lungeStartTime)
			{
				agent.speed = 0f;
				anim.SetBool("Dive", false);
			}
			if (Time.time > lungeChargeTime*4f + lungeStartTime)
			{
				mAttacked = false;
			}
		}
		if (!mStunCheck.IsStunned)
		{
			agent.Resume();
			anim.SetBool("ShockBool", false);
			mStunCheck.UpdateStunInvinciblity();

			//For everything on this level zombie can eat
			foreach(GameObject meal in lunchTargets)
			{
				if (meal != null)
				{
					//Distance from dino to the potential meal
					float newDistance = Vector3.Distance(transform.position, meal.transform.position);
					//Distance it was travel to last
					float oldDistance = Vector3.Distance(transform.position, moveTarget);
					//If the potential meal is further from the 
					if (newDistance <= oldDistance)
					{
						if (!mAttacked)
						{
							moveTarget = meal.transform.position;
							victim = meal;
						}
					}
				}
			}

			if (victim != null && Vector3.Distance(transform.position, victim.transform.position) < 3)
			{
				if (!mAttacked && canLunge && canEverLunge)
				{
					if (mAudioManager)
					{
						mAudioManager.PlaySoundEffect("Zombie Growl");
					}
					mAttacked = true;
					canLunge = false;
					agent.speed = 0f;
					lungeStartTime = Time.time;
					//moveTarget = victim.transform.position;
					//UnityEngine.Debug.Log(moveTarget);
					lungeTarget = victim.transform.position;
					Ray pathToPoint = new Ray(transform.position, lungeTarget-transform.position);
					moveTarget = pathToPoint.GetPoint(1f);
					//UnityEngine.Debug.Log(moveTarget);
				}

			}

			//If the zombie is right over the player
			if (victim != null && Vector3.Distance(transform.position, victim.transform.position) < 1)
			{
				if (!eatingThings)
				{
					if (mAudioManager)
					{
						mAudioManager.PlaySoundEffect("Zombie Growl");
					}
					eatingThings = true;
				}
				//playerAvatar.GetComponent<NavMeshAgent>().Stop();
				NavMeshAgent victimAgent = victim.GetComponent<NavMeshAgent>();
				if (victim == lunchTargets[0] || (victim.GetComponent<CompanionAI>() != null && victim.GetComponent<CompanionAI>().vitalNPC == true) )
				{
					if (playerMovement != null && playerMovement.NumberofShields == 0)
					{

						if (victimAgent.enabled == true)
						{
							victimAgent.ResetPath();
							victimAgent.enabled = false;
						}
						playerMovement.playDeathAnim();
						Invoke("restartLevel", mGrowl.length);
						GameObject.Find("LevelHUDManager").GetComponent<LevelHUD>().playerDied = true;

					}
					else if(victim.tag == "Companion" &&
							((Vector3.Distance(victim.transform.position, playerAvatar.transform.position) <= 2f && playerMovement.NumberofShields == 0) 
						 		|| (Vector3.Distance(victim.transform.position, playerAvatar.transform.position) > 2f) ))
					{
                        victim.GetComponent<CompanionAI>().playDeathAnim();
						Invoke("restartLevel", mGrowl.length);
						GameObject.Find("LevelHUDManager").GetComponent<LevelHUD>().vitalNPCDied = true;

						//GetComponent<Keybearer>().posessedKey = victim.GetComponent<Keybearer>().posessedKey;
						//Destroy(victim);
						agent.Stop();
						agent.ResetPath();
						victim = lunchTargets[0];
						//moveTarget = victim.transform.position;
						//mAttacked = false;

					}
					else
					{
						Invoke("restartLevel", 0f);
					}
				} 
				else
				{
					if (victim.GetComponent<Keybearer>() != null)
					{
						GetComponent<Keybearer>().posessedKey = victim.GetComponent<Keybearer>().posessedKey; 
					}
                    if (victim.tag != "Victim")
                    {
                        Destroy(victim);
                    }
                    else
                    {
                        victim.GetComponent<ZombieVictim>().TurnToTheUndead();
                        lunchTargets.Remove(victim);
                    }
					victim = lunchTargets[0];
					moveTarget = victim.transform.position;
					mAttacked = false;
					eatingThings = false;
				}				
			}
			if (Vector3.Distance(transform.position, moveTarget) < chaseDistance)
			{
				if (CanSeePastObstacles())
				{
					agent.SetDestination(moveTarget);
					anim.SetFloat("Speed", 2f);
				}
			}
			else
			{
				agent.Stop();
				anim.SetFloat("Speed", 0f);
			}
		}
		else
		{
			mStunCheck.UpdateStunnedTimer();
			agent.Stop();
			anim.SetBool("ShockBool", true);
			anim.SetBool("Dive", false);
            mAttacked = false;
		}
	}

	bool CanSeePastObstacles()
	{
		bool bCanDetect = true;
		
		//Raycast towards the player will only be called when the player
		//is within the detection radius
		RaycastHit[] targetsHit;
		float targetDistance = Vector3.Distance(transform.position,moveTarget);
		targetsHit = Physics.RaycastAll(transform.position, moveTarget - transform.position, targetDistance);
		
		GameObject targetAcquired;
		//If the raycast hit more than just the player
		if(targetsHit.Length > 1)
		{
			//for every hit
			foreach(RaycastHit hit in targetsHit)
			{
				//Get the gameobject
				targetAcquired = hit.collider.gameObject;
				//If it's not the player
				if(targetAcquired.tag != "Player")
				{
					//See if it is a blocking object
					MonsterObstacle blockingObject = targetAcquired.GetComponent<MonsterObstacle>();
					if (blockingObject != null)
					{
						if (blockingObject.ZombieProof)
						{
							if  ( !(blockingObject.GetComponent<DoorNavigation>() != null && blockingObject.GetComponent<DoorNavigation>().open == true) )
								bCanDetect = false;
						}
					}
				}
			}
		}
		return bCanDetect;
	}

    void restartLevel()
    {

        //PlayerMovement deadPlayer = playerAvatar.GetComponent<PlayerMovement>();
		if (playerMovement != null)
        {
			playerMovement.PlayerDeath(gameObject, zombieID);
        }
    }
}
