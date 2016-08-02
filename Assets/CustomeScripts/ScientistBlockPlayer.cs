using UnityEngine;
using System.Collections;

public class ScientistBlockPlayer : MonoBehaviour {



	private Animator anim;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();
		anim.applyRootMotion = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{

			anim.SetBool("ArmsUp", true);
						
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{

			anim.SetBool("ArmsUp", false);
			
		}

	}
}
