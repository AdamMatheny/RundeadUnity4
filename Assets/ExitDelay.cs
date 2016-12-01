using UnityEngine;
using System.Collections;

public class ExitDelay : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		StartCoroutine(finalWord ());
	}
	
	IEnumerator finalWord() {
		yield return new WaitForSeconds(3);
		Application.LoadLevel(Application.loadedLevel+1);
	}
}




	
