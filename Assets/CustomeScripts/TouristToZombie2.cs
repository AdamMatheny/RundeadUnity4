using UnityEngine;
using System.Collections;

public class TouristToZombie2 : MonoBehaviour {

	public GameObject[] TouristList;
	public GameObject[] NoMeshRender;
	bool PlayDeathAnim=true;
	public bool ReVer=true;
	int i = 0;

	// Use this for initialization
	void Start () {
	if(!ReVer){
			for(i=0; i<TouristList.Length; i++){
				TouristList[i].GetComponent<Animator>().SetBool("switchUp", ReVer);
			}
		}
	}

	IEnumerator WaitToStart() {
		for(i=0; i<NoMeshRender.Length; i++){
			/*if(NoMeshRender[i].GetComponent<MeshRenderer>().enabled)
				NoMeshRender[i].GetComponent<MeshRenderer>().enabled = false;
			else
			NoMeshRender[i].GetComponent<MeshRenderer>().enabled = true;*/
			NoMeshRender[i].GetComponent<MeshRenderer>().enabled = !NoMeshRender[i].GetComponent<MeshRenderer>().enabled;
		}
		yield return new WaitForSeconds(5f);
		for(i=0; i<TouristList.Length; i++){
			TouristList[i].GetComponent<Animator>().SetBool("switchUp", !ReVer);
		}

		ReVer=!ReVer;
		PlayDeathAnim = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(){
		if(PlayDeathAnim)
		StartCoroutine(WaitToStart());

}
}