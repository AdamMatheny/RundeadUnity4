using UnityEngine;
using System.Collections;

public class MapConsole : MonoBehaviour 
{
	[SerializeField] private Camera mapCamera;
	public bool showMap = false;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (showMap == true)
		{
				mapCamera.depth = 5;
		}
		else if (showMap == false)
		{
				mapCamera.depth = -5;
		}
	}

	void OnGUI ()
	{


	}

//	void OnTriggerEnter (Collider other)
//	{
//		if (other.tag == "Player")
//		{
//			mapCamera.depth = 5;
//			showMap = true;
//		}
//	}
//	void OnTriggerExit (Collider other)
//	{
//		if (other.tag == "Player")
//		{
//			mapCamera.depth = -5;
//			showMap = false;
//		}
//	}
}
