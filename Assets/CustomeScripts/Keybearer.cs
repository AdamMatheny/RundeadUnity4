using UnityEngine;
using System.Collections;

public class Keybearer : MonoBehaviour 
{

	public int posessedKey = 0;  //0 is generic, 1 is blue, 2 is yellow, 3 is cyan, 4 is magenta
	[SerializeField ]private GameObject feetCircle;
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (feetCircle != null)
		{
			feetCircle.renderer.enabled = true;

			switch (posessedKey)
			{
			case 0:
				feetCircle.renderer.material.color = Color.white;
				break;
			case 1:
				feetCircle.renderer.material.color = Color.blue;
				break;
			case 2:
				feetCircle.renderer.material.color = Color.yellow;
				break;
			case 3:
				feetCircle.renderer.material.color = Color.cyan;
				break;
			case 4:
				feetCircle.renderer.material.color = Color.magenta;
				break;
			case 5:
				feetCircle.renderer.material.color = Color.green;
				break;
			default:
				break;
				
			}
		}
	
	}
}
