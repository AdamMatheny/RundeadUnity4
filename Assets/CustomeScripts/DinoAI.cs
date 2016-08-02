using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

public class DinoAI : MonoBehaviour
{
    [SerializeField]
    private string dinoID = "";
    //get this when you want to move the player via nav mesh
    NavMeshAgent agent;
    //telling the player where to move to
    Vector3 moveTarget;
    GameObject victim;
    [SerializeField] private float chaseDistance = 8f;
	private List<GameObject> lunchTargets;
	Stunable mStunCheck;
    public AudioClip mGrowl;
    bool mAttacked;
	private bool canLunge = true;
	[SerializeField] private bool canEverLunge = true;
	private float lungeChargeTime = 0.5f;
	private float lungeStartTime = 0f;
	private float lungeCooldown = 10f;
	private bool eatingThings = false;


	[SerializeField] private float defaultSpeed = 3.7f;
	[SerializeField] private float lungeSpeed = 10;
	[SerializeField] private float defaultAccel = 8f;
	[SerializeField] private float lungeAccel = 30;
	private Vector3 lungeTarget;




	bool mSleeping; 
    AudioManager mAudioManager;
	void Start () 
    {
        lunchTargets = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
		mStunCheck = GetComponent<Stunable>();
        lunchTargets.Add(GameObject.Find("PlayerAvatar"));
        ZombieAI[] zombies = FindObjectsOfType(typeof(ZombieAI)) as ZombieAI[];
        foreach (ZombieAI zombie in zombies)
        {
            lunchTargets.Add(zombie.gameObject);
        }
		CompanionAI[] companions = FindObjectsOfType(typeof(CompanionAI)) as CompanionAI[];
		foreach (CompanionAI companion in companions)
		{
			if (!companion.vitalNPC)//For making the Companion invulnerable
			{
				lunchTargets.Add(companion.gameObject);
			}
		}

        mAttacked = false;
        mAudioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
        if (mAudioManager)
        {
            mAudioManager.AddSoundEffect("Dino Growl", mGrowl);
        }

        moveTarget = lunchTargets[0].transform.position;
        mSleeping = false;
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
				GetComponent<Animator>().Play("Dive");
				agent.acceleration = lungeAccel;
				Ray pathToPoint = new Ray(transform.position, lungeTarget-transform.position);
				lungeTarget = pathToPoint.GetPoint(2f);
				
				moveTarget = lungeTarget;
			}
			if (Time.time > lungeChargeTime*2f + lungeStartTime)
			{
				agent.speed = 0f;
				//GetComponent<Animator>().SetFloat("Speed", 0f);
			}
			if (Time.time > lungeChargeTime*4f + lungeStartTime)
			{
				mAttacked = false;
			}
		}

		if(!mStunCheck.IsStunned)
		{
			agent.Resume();

			GetComponent<Animator>().SetBool("ShockBool", false);
			mStunCheck.UpdateStunInvinciblity();
			//For everything on this level dino can eat
			foreach(GameObject meal in lunchTargets)
			{
				if (meal != null)
				{
					//Distance from dino to the potential meal
					float newDistance = Vector3.Distance(this.transform.position, meal.transform.position);
					//Distance it was travel to last
					float oldDistance = Vector3.Distance(this.transform.position, moveTarget);
					//If the potential meal is further from the 
					if (newDistance <= oldDistance)
					{
						moveTarget = meal.transform.position;
						victim = meal;
					}
				}
			}

            if (Vector3.Distance(this.transform.position, moveTarget) < chaseDistance)
            {
                if (CanSeePastObstacles())
                {
                    agent.SetDestination(moveTarget);
					GetComponent<Animator>().SetFloat("Speed", 1f);
                }
            }
            else
            {
                agent.Stop();
				GetComponent<Animator>().SetFloat("Speed", 0f);
            }
			//lunge when the target is close
			if (victim != null && Vector3.Distance(this.transform.position, victim.transform.position) < 3)
			{
				if (!mAttacked && canLunge && canEverLunge)
				{
					if (mAudioManager)
					{
						mAudioManager.PlaySoundEffect("Dino Growl");
					}
					mAttacked = true;
					canLunge = false;
					agent.speed = 0f;
					//GetComponent<Animator>().SetFloat("Speed", 0f);
					lungeStartTime = Time.time;
					//moveTarget = victim.transform.position;
					//UnityEngine.Debug.Log(moveTarget);
					lungeTarget = victim.transform.position;
					Ray pathToPoint = new Ray(transform.position, lungeTarget-transform.position);
					moveTarget = pathToPoint.GetPoint(1f);
					//UnityEngine.Debug.Log(moveTarget);
				}
				
			}
			
			if (victim != null && Vector3.Distance(this.transform.position, victim.transform.position) < 1)
			{
				GetComponent<Animator>().Play("Eat");
				if(victim.GetComponent<PlayerMovement>() == null)
				victim.GetComponent<Animator>().Play("Eat");
				if (!eatingThings)
                {
                    if (mAudioManager)
                    {
                       mAudioManager.PlaySoundEffect("Dino Growl");
						eatingThings = true;
                    }
                }
                
				if (victim == lunchTargets[0] || (victim.GetComponent<CompanionAI>() != null && victim.GetComponent<CompanionAI>().vitalNPC == true) )
				{	
					if (victim.GetComponent<PlayerMovement>() != null && victim.GetComponent<PlayerMovement>().NumberofShields == 0)
					{
						victim.GetComponent<Animator>().Play("Eat");
						if (victim.GetComponent<NavMeshAgent>().enabled == true)
						{
							victim.GetComponent<NavMeshAgent>().ResetPath();
							victim.GetComponent<NavMeshAgent>().enabled = false;
						}
                        //lunchTargets[0].GetComponent<PlayerMovement>().playDeathAnim();
						Invoke("restartLevel", mGrowl.length);
						GameObject.Find("LevelHUDManager").GetComponent<LevelHUD>().playerDied = true;
						//lunchTargets[0].GetComponent<PlayerMovement>().playDeathAnim();
					}
					else if(victim.tag == "Companion" && 
					        ( (Vector3.Distance(victim.transform.position, lunchTargets[0].transform.position) <= 2f && lunchTargets[0].GetComponent<PlayerMovement>().NumberofShields == 0) 
					 || (Vector3.Distance(victim.transform.position, lunchTargets[0].transform.position) > 2f) ))
					{
                        victim.GetComponent<CompanionAI>().playDeathAnim();
						Invoke("restartLevel", mGrowl.length);
						GameObject.Find("LevelHUDManager").GetComponent<LevelHUD>().vitalNPCDied = true;
						//lunchTargets[0].GetComponent<CompanionAI>().playDeathAnim();
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
						if (victim.GetComponent<Keybearer>().enabled == true)
						{
							GetComponent<Keybearer>().enabled = true;
							GetComponent<Keybearer>().posessedKey = victim.GetComponent<Keybearer>().posessedKey;
						}
					}
					StartCoroutine(WaitToDestroy(victim));
					victim = lunchTargets[0];
					moveTarget = victim.transform.position;

				}
			}

            
		}
		else
		{
			mStunCheck.UpdateStunnedTimer();
			if (victim != null)
			{
				moveTarget = victim.transform.position;
			}
			agent.Stop();
			if(GetComponent<Stunable>().mTypeofStun == Stunable.StunType.ShockStun)
			GetComponent<Animator>().SetBool("ShockBool", true);
            mAttacked = false;
		}
	}

	IEnumerator WaitToDestroy(GameObject deadman){
		yield return new WaitForSeconds(1f);
		Destroy(deadman);
		eatingThings = false;
		mAttacked = false;
	}


	bool CanSeePastObstacles()
	{
		bool bCanDetect = true;

		//Raycast towards the player will only be called when the player
		//is within the detection radius
		RaycastHit[] targetsHit;
		float targetDistance = Vector3.Distance(transform.position, moveTarget);
		targetsHit = Physics.RaycastAll(transform.position, moveTarget - transform.position, targetDistance);

		GameObject targetAcquired;
		//If the raycast hit more than just the player
		if (targetsHit.Length > 1)
		{
			//for every hit
			foreach (RaycastHit hit in targetsHit)
			{
				//Get the gameobject
				targetAcquired = hit.collider.gameObject;
				//If it's not the player
				if (targetAcquired.tag != "Player" || targetAcquired.tag != "Zombie")
				{
					//See if it is a blocking object
					MonsterObstacle blockingObject = targetAcquired.GetComponent<MonsterObstacle>();
					if (blockingObject != null)
					{
						if (blockingObject.DinosaurProof)
						{
							if (!(blockingObject.GetComponent<DoorNavigation>() != null && blockingObject.GetComponent<DoorNavigation>().open == true))
								bCanDetect = false;
						}
					}
				}
			}
		}
		//		UnityEngine.Debug.Log("Raycast hit " + targetsHit.Length);
		return bCanDetect;
	}



    void restartLevel()
	{
		PlayerMovement deadPlayer = GameObject.Find("PlayerAvatar").GetComponent<PlayerMovement>();
		if(deadPlayer != null)
		{
			deadPlayer.PlayerDeath(gameObject, dinoID);
		}
    }
}
