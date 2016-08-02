using UnityEngine;
using System.Collections;

public class AnimatorSpeed : MonoBehaviour {

	public float mySpeed;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.speed = mySpeed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
