using UnityEngine;
using System.Collections;

public class BadgeIcon : MonoBehaviour 
{
	[SerializeField] private Material[] keyMaterials;
	[SerializeField] private GameObject parentObject;

	// Use this for initialization
	void Start () 
	{
		
		if (parentObject.GetComponent<Keybearer>() != null)
			{
				renderer.enabled = true;
				//	GetComponent<Light>().enabled = true;
			}
			else
			{
				renderer.enabled = false;
				//	GetComponent<Light>().enabled = false;
			}



	}
	
	// Update is called once per frame
	void Update () 
	{
		//transform.LookAt(Camera.main.transform.position);
		if (parentObject.GetComponent<Keybearer>() != null && parentObject.GetComponent<Keybearer>().enabled == true)
		{
			renderer.enabled = true;
			//GetComponent<Light>().enabled = true;
			switch (parentObject.GetComponent<Keybearer>().posessedKey)
			{
			case 1:
				renderer.material = keyMaterials[1];
		//		GetComponent<Light>().color = Color.blue;
				break;
			case 2:
				renderer.material = keyMaterials[2];
		//		GetComponent<Light>().color = Color.yellow;
				break;
			case 3:
				renderer.material = keyMaterials[3];
		//		GetComponent<Light>().color = Color.cyan;
				break;
			case 4:
				renderer.material = keyMaterials[4];
		//		GetComponent<Light>().color = Color.magenta;
				break;
			default:
				renderer.material = keyMaterials[0];
		//		GetComponent<Light>().color = Color.white;
				break;
			}
		}
		else
		{
			renderer.enabled = false;
		//	GetComponent<Light>().enabled = false;
		}

	
	}
}
