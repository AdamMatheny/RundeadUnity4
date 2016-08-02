using UnityEngine;
using System.Collections;

public class NPCMagnetEndTrigger : MonoBehaviour 
{
	[SerializeField] private NPCMagnet associatedMagnet;
	public bool hasBeenHit = false;
	[SerializeField] private NPCMagnetEndTrigger[] siblingTriggers;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter (Collider other)
	{
		if (!hasBeenHit && associatedMagnet.tellingCompanionWhereToGo && other.tag == "Player")
		{
			associatedMagnet.ReturnCompanion();
			hasBeenHit = true;

			if (siblingTriggers != null)
			{
				for (int i = 0; i < siblingTriggers.Length; i++)
				{
					siblingTriggers[i].hasBeenHit = true;
				}
			}
		}
	}
}
