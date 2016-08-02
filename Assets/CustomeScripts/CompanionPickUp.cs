using UnityEngine;
using System.Collections;

public class CompanionPickUp : MonoBehaviour 
{
	
	[SerializeField] private CompanionAI companionToMove;

	[SerializeField] private CompanionDropOff[] dropOffsToDisable;
	[SerializeField] private NPCMagnet[] magnetsToDisable;

	[SerializeField] private bool singleUse = true;

	public bool hasBeenHit = false;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (!hasBeenHit && companionToMove != null)
			{
				companionToMove.activeFollow = true;
				if (dropOffsToDisable.Length > 0)
				{
					for (int i = 0; i < dropOffsToDisable.Length; i++)
					{
						if (dropOffsToDisable[i] != null)
						{
							dropOffsToDisable[i].enabled = false;
						}
					}
				}
				if (magnetsToDisable.Length > 0)
				{
					for (int i = 0; i < magnetsToDisable.Length; i++)
					{
						if (magnetsToDisable[i] != null)
						{
							magnetsToDisable[i].ReturnCompanion();
						}
					}
				}
				if (singleUse)
				{
					hasBeenHit = true;
				}
			}
		}
	}
	
	

}
