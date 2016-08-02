using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {

	public float flickerSpeed;
	private bool onOff;

	// Use this for initialization
	void Start () {
		onOff = true;
		lightOn();
	}

	IEnumerator lightOn(){
		yield return new WaitForSeconds(flickerSpeed);
		light.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		

	}
}
