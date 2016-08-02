using UnityEngine;
using System.Collections;

public class GasMaskVisibility2 : MonoBehaviour 
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

		if (GameObject.Find("PlayerAvatar").GetComponent<PlayerMovement>().mHasGasMask == true && GameObject.Find("PlayerAvatar").GetComponent<PlayerMovement>().myCompanion != null)
		{
			renderer.enabled = true;
		}
		else
		{
			renderer.enabled = false;
		}
	}
}
