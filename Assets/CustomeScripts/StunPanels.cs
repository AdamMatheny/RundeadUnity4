using UnityEngine;
using System.Collections;

public class StunPanels : Toggler 
{

	public override bool Activate(Collider other, GameObject parent)
	{
		return true;
	}

	public override bool Reactivate(Collider other, GameObject parent)
	{
		return true;
	}

	//Use this when linked switch deactivtes
	public override bool Deactivate(Collider other, GameObject parent)
	{
		return false;
	}
}
