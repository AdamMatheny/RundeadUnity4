using UnityEngine;
using System.Collections;

public class NPCMagnet : MonoBehaviour 
{

	[SerializeField] private CompanionAI companionToMove;
	[SerializeField] private GameObject magnetPoint;
	[SerializeField] private bool endOnTimer;

	[SerializeField] private float magnetTime;
	private float magnetStartTime;

	public bool tellingCompanionWhereToGo;

	bool companionsPriorActiveFollow;

	public bool hasBeenHit = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (tellingCompanionWhereToGo)
		{
			if (companionToMove != null)
			{

				companionToMove.GetComponent<NavMeshAgent>().SetDestination(magnetPoint.transform.position);
				if(Vector3.Distance(companionToMove.transform.position, magnetPoint.transform.position) <=2.0f)
				{
					companionToMove.GetComponent<Animator>().SetFloat("Speed", 0f);
				}
		
				if (endOnTimer)
				{
					if (Time.time > magnetTime+magnetStartTime)
					{
						ReturnCompanion();
					}
				}
			}
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (!hasBeenHit && companionToMove != null)
			{
				companionsPriorActiveFollow=companionToMove.activeFollow;
				companionToMove.StopFollowing();
				tellingCompanionWhereToGo = true;
				companionToMove.GetComponent<Animator>().SetFloat("Speed", 1f);
				magnetStartTime = Time.time;
				hasBeenHit = true;
			}
		}
	}


	public void ReturnCompanion()
	{
		tellingCompanionWhereToGo = false;
		if (companionsPriorActiveFollow && companionToMove != null)
		{
			companionToMove.StartFollowing();
		}
	}
}
