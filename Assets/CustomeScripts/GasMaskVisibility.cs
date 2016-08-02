using UnityEngine;
using System.Collections;

public class GasMaskVisibility : MonoBehaviour 
{
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameObject.Find("PlayerAvatar").GetComponent<PlayerMovement>().mHasGasMask == false)
		{
			renderer.enabled = false;
		}
		else
		{
			renderer.enabled = true;
		}
	}
}
