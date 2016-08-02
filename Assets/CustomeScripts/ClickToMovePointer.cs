using UnityEngine;
using System.Collections;

public class ClickToMovePointer : MonoBehaviour 
{
	private Vector3 bobPoint;
	private MeshRenderer[] componentMeshes;
	private bool bobbingUp = true;
	// Use this for initialization
	void Start () 
	{
		componentMeshes = gameObject.GetComponentsInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (transform.position.y <= bobPoint.y+0.1f)
		{
			bobbingUp = true;
		}
		else if (transform.position.y >= bobPoint.y+1f)
		{
			bobbingUp = false;
		}

		if (bobbingUp)
			transform.Translate(0,0.01f, 0);
		else
			transform.Translate(0,-0.01f, 0);
	}

	public void RevealAtLocation (Vector3 newLocation)
	{
		bobPoint = newLocation;
		transform.position = bobPoint + new Vector3(0,0.1f, 0);
		foreach (MeshRenderer arrowMesh in componentMeshes)
		{
			arrowMesh.enabled = true;
		}

	}
	public void Hide ()
	{
		foreach (MeshRenderer arrowMesh in componentMeshes)
		{
			arrowMesh.enabled = false;
		}
	}

}
