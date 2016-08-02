using UnityEngine;
using System.Collections;

public class Computer_Synth : MonoBehaviour {

	bool started = false;

	public GameObject[] computers;
	public Material[] computerMat;
	int i =0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter()
	{
		if(!started)
		{
			started = true;
			ChangeToSynth();
		}

	}

	void ChangeToSynth()
	{
		for(i = 0; i < computers.Length; i++)
		computers[i].renderer.material = computerMat[0];
		StartCoroutine(WaitToChangeToDone());
	}

	IEnumerator WaitToChangeToDone() {
		yield return new WaitForSeconds(24f);
		ChangeToDone();
	}

	void ChangeToDone()
	{
		for(i = 0; i < computers.Length; i++)
			computers[i].renderer.material = computerMat[1];
		StartCoroutine(WaitToChangeToBlack());
	}

	IEnumerator WaitToChangeToBlack() {
		yield return new WaitForSeconds(5f);
		ChangeToBlack();
	}

	void ChangeToBlack()
	{
		for(i = 0; i < computers.Length; i++)
			computers[i].renderer.material = computerMat[2];
	}
}
