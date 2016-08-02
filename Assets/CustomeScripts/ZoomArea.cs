using UnityEngine;
using System.Collections;

public class ZoomArea : MonoBehaviour 
{
	[SerializeField] private Transform transformPoint;
	private TopDownCamera topDownComponent;
    public float mSpeed = 0;
	// Use this for initialization
	void Start () 
	{
		topDownComponent = Camera.main.GetComponent<TopDownCamera>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerStay (Collider other)
	{
		if ((other.tag == "Player") && enabled == true)
		{
			topDownComponent.ZoomIn(transformPoint, mSpeed);
		}
	}
	void OnTriggerExit (Collider other)
	{
        if ((other.tag == "Player") && enabled == true)
		{
			topDownComponent.ZoomOut();
		}
	}
}
