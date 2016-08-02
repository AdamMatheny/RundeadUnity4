using UnityEngine;
using System.Collections;

public abstract class Toggler : MonoBehaviour 
{
	public bool Activated { get; set; }
	//private int NeededSwitches = 1;
	// Use this when linked switch activates
	public virtual bool Activate(Collider other, GameObject flippedSwitch)
	{
		return true;
	}

	public virtual bool Reactivate(Collider other, GameObject flippedSwitch)
	{
		return true;
	}

	//Use this when linked switch deactivtes
	public virtual bool Deactivate(Collider other, GameObject flippedSwitch)
	{
		return true;
	}
}
