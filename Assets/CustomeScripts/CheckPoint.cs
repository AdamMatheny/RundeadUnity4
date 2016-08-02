using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
{
	//Whether checkpoint has been reached for one-time-use
	public string CheckpointName;
	private CompanionAI mCompanion;
	//Maybe always save the last time player hit this
	//public bool bMultipleSaves = false;
	void Start()
	{
		if(PlayerPrefs.GetInt("CheckPointReached") != 0 
			&& PlayerPrefs.GetString("CheckPointName") == CheckpointName)
		{
			StartAtCheckPoint();
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Keybearer player = other.gameObject.GetComponent<Keybearer>();
			if(player != null)
			{
				SaveValues(player);
			}
		}
	}
	private void StartAtCheckPoint()
	{
		PlayerMovement player = FindObjectOfType<PlayerMovement>();
		NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
		//Move the player
		agent.enabled = false;
		player.transform.position = transform.position + new Vector3(0, 2, 0.5f);
		agent.enabled = true;
		FindObjectOfType<PlayerMovement>().GetComponent<Keybearer>().posessedKey = PlayerPrefs.GetInt("PlayerKey");
		//Move Companion
		if (PlayerPrefs.HasKey("CompanionPresent"))
		{
			mCompanion = GameObject.Find(PlayerPrefs.GetString("CompanionPresent")).GetComponent<CompanionAI>();
			player.myCompanion = mCompanion;
			agent = mCompanion.GetComponent<NavMeshAgent>();
			agent.enabled = false;
			if(PlayerPrefs.HasKey("DropOffLocation"))
			{
				mCompanion.transform.position = GameObject.Find(PlayerPrefs.GetString("DropOffLocation")).transform.position + new Vector3(0, 2, -0.5f);
			}
			else if (PlayerPrefs.HasKey("CompanionX"))
			{

				mCompanion.transform.position = new Vector3(PlayerPrefs.GetFloat("CompanionX"), PlayerPrefs.GetFloat("CompanionY") + 2, PlayerPrefs.GetFloat("CompanionZ") - 0.5f);
			}
			else
			{
				mCompanion.transform.position = transform.position + new Vector3(0, 2, -0.5f);
			}

			agent.enabled = true;
		}
	}
	private void SaveValues(Keybearer player)
	{
		PlayerPrefs.SetInt("PlayerKey", player.posessedKey);
		PlayerPrefs.SetString("Name", CheckpointName);
		PlayerPrefs.SetInt("CheckPointReached", 1);
		PlayerPrefs.SetString("CheckPointName", CheckpointName);
		//CompanionAI[] Companions = FindObjectsOfType<CompanionAI>();
		mCompanion = player.GetComponent<PlayerMovement>().myCompanion;
		
		//If the player has a companion following him
		if (mCompanion != null) 
		{
			//If it is actvely following and within range 
			if((mCompanion.activeFollow && Vector3.Distance(mCompanion.gameObject.transform.position,gameObject.transform.position) < 20 ))
			{
				PlayerPrefs.SetString("CompanionPresent", mCompanion.name);
			}
				//If the companion is at a dropoff and shouldn't be left behind
			else if(mCompanion.atDropOff && !mCompanion.LeaveBehind && mCompanion.DropOff != null)
			{
				PlayerPrefs.SetString("CompanionPresent", mCompanion.name);
				PlayerPrefs.SetString("DropOffLocation", mCompanion.DropOff.name);
			}
				//If we just tell that companion to spawn at their last location
			else if(mCompanion.SpawnAtLastLocation)
			{
				PlayerPrefs.SetString("CompanionPresent", mCompanion.name);
				PlayerPrefs.SetFloat("CompanionX", mCompanion.transform.position.x);
				PlayerPrefs.SetFloat("CompanionY", mCompanion.transform.position.y);
				PlayerPrefs.SetFloat("CompanionZ", mCompanion.transform.position.z);
			}

		}
		else
		{
			PlayerPrefs.DeleteKey("CompanionPresent");
			PlayerPrefs.DeleteKey("DropOffLocation");
			PlayerPrefs.DeleteKey("CompanionX");
			PlayerPrefs.DeleteKey("CompanionY");
			PlayerPrefs.DeleteKey("CompanionZ");
		}
	}
}
