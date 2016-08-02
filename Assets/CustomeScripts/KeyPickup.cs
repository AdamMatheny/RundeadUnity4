using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour 
{
	[SerializeField] private int keyType = 1; //0 is generic, 1 is blue, 2 is yellow, 3 is cyan, 4 is magenta
	[SerializeField] private Material[] keyMaterials;
	[SerializeField] private GameObject badgeMesh;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		badgeMesh.transform.LookAt(Camera.main.transform.position);
		switch (keyType)
		{
		case 1:
			badgeMesh.renderer.material = keyMaterials[1];
		//	GetComponent<Light>().color = Color.blue;
			break;
		case 2:
			badgeMesh.renderer.material = keyMaterials[2];
		//	GetComponent<Light>().color = Color.yellow;
			break;
		case 3:
			badgeMesh.renderer.material = keyMaterials[3];
			//GetComponent<Light>().color = Color.cyan;
			break;
		case 4:
			badgeMesh.renderer.material = keyMaterials[4];
			//GetComponent<Light>().color = Color.magenta;
			break;
		case 5:
			badgeMesh.renderer.material = keyMaterials[0];
			GetComponent<Light>().color = Color.green;
			break;
		default:
			badgeMesh.renderer.material = keyMaterials[0];
			//GetComponent<Light>().color = Color.white;
			break;
			
		}

	}

	void OnTriggerEnter ( Collider other)
	{
		if (other.tag == "Player" || other.tag == "Companion")
		{
			if (other.GetComponent<Keybearer>() != null)
			{
				if (other.GetComponent<Keybearer>().enabled == true)
				{
					int tempKeyType = other.GetComponent<Keybearer>().posessedKey;
					other.GetComponent<Keybearer>().posessedKey = keyType;
					if (tempKeyType == 0)
					{
						Destroy(this.gameObject);
					}
					else
					{
						keyType = tempKeyType;
					}
				}
			}
		}
	}
}
