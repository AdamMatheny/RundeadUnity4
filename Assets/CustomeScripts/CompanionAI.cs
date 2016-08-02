using UnityEngine;
using System.Collections;

public class CompanionAI : MonoBehaviour 
{
	//[SerializeField] private string companionID = "";
	//get this when you want to move the player via nav mesh
	NavMeshAgent agent;
	//telling the player where to move to
	Vector3 moveTarget;
	[SerializeField] private float chaseDistance = 12f;
	//Stunable mStunCheck;
	
	private GameObject playerAvatar;

	public bool activeFollow = true;
	public bool vitalNPC = true; //  if this is true, restart the level when the companion dies.
	public bool LeaveBehind = false; //Once this NPC leave don't spawn from checkpoints 
	public bool SpawnAtLastLocation = false; //Spawn this character wherever they are were previously, not their starting location
	public bool atDropOff = false;
	[HideInInspector]public GameObject DropOff;
	public Animator anim;
	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent>();
		//mStunCheck = GetComponent<Stunable>();
		playerAvatar = GameObject.Find("PlayerAvatar");
		anim = GetComponent<Animator>();
		anim.applyRootMotion = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (activeFollow)
		{
			agent.Resume();
			agent.speed = playerAvatar.GetComponent<NavMeshAgent>().speed + 1f;
			anim.SetFloat("Speed", 2f);

			moveTarget = playerAvatar.transform.position;

			//Stop Moving if the companion is right over the player
			if (Vector3.Distance(this.transform.position, moveTarget) < 1.5f)
			{
				agent.Stop();
				anim.SetFloat("Speed", 0f);
			}
			//Move if the player is within the chase distance
			else if (Vector3.Distance(this.transform.position, moveTarget) < chaseDistance)
			{
				if (playerAvatar.GetComponent<PlayerMovement>().myCompanion == null)
				{
					playerAvatar.GetComponent<PlayerMovement>().myCompanion = this;
				}
				if (CanSeePastObstacles())
				{
					agent.SetDestination(moveTarget);
				}
				
			}
			//Stop moving if the Companion is too far away from the player
			else
			{
				agent.Stop();
				anim.SetFloat("Speed", 0f);
			}
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
		//		UnityEngine.Debug.Log("Raycast hit " + targetsHit.Length);

		bCanDetect = true;		//ignore all the above and always see the player


		return bCanDetect;
	}

	public void playDeathAnim()
	{
		anim.Play("Death");
		StopFollowing();
	}


	public void StartFollowing()
	{
		activeFollow = true;
	}
	public void StopFollowing()
	{
		activeFollow = false;
		agent.Stop();
		//agent.ResetPath();
	}
}
