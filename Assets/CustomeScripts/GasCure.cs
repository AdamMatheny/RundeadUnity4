using UnityEngine;
using System.Collections;

public class GasCure : Toggler 
{
	
	private bool mAffectedPlayer = false;
	public bool mGasActive = true;
	
	private ParticleSystem myGas;
	
	// Use this for initialization
	void Start () 
	{
		myGas = GetComponent<ParticleSystem>();
		if (!mGasActive)
		{
			renderer.enabled = false;
			myGas.enableEmission = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public override bool Activate(Collider other, GameObject parent)
	{
		mGasActive = !mGasActive;
		//renderer.enabled = !renderer.enabled;
		myGas.enableEmission = mGasActive;
		return base.Activate(other, parent);
	}
	
	//while colliding with the gas, Dinos and the Player will be affected by it
	void OnTriggerStay(Collider other)
	{
		if (mGasActive)
		{
			//if a Dino is colliding with the gas, it is stunned and will stay stunned as long as it touches the gas
			if (other.GetComponent<ZombieAI>())
			{
				other.GetComponent<ZombieAI>().GetComponent<Stunable>().bStayStunned = true;
				other.GetComponent<ZombieAI>().GetComponent<Stunable>().IsStunned = true;
			}
		}
		else
		{
			if (other.GetComponent<ZombieAI>())
			{
				ZombieAI zombie = other.GetComponent<ZombieAI>();
				zombie.GetComponent<Stunable>().bStayStunned = false;
				zombie.GetComponent<Stunable>().IsStunned = false;
			}
		}
		
	}
	//after the gas leaves, Dinos resume can be unstun and will begin functioning normal again
	//after the gas leaves, the Player's gas timers are reset, and gas masks removed if any
	void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<ZombieAI>())
		{
			ZombieAI zombie = other.GetComponent<ZombieAI>();
			zombie.GetComponent<Stunable>().bStayStunned = false;
			zombie.GetComponent<Stunable>().IsStunned = false;
		}
	}
}

