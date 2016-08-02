using UnityEngine;
using System.Collections;

public class ZombieRandom : MonoBehaviour {

	public GameObject[] HairMesh;
	public GameObject randMesh;
	public Material[] TextureMesh;

	// Use this for initialization
	void Start () {
	
		int randNum = Random.Range(0,HairMesh.Length);
		for(int i = 0; i<HairMesh.Length; i++)
		{
			if(randNum == i){
				HairMesh[i].GetComponent<MeshRenderer>().enabled = true;
			}
			else{
				HairMesh[i].GetComponent<MeshRenderer>().enabled = false;
			}
		}



		randNum = Random.Range(0,TextureMesh.Length);
		for(int i = 0; i<TextureMesh.Length; i++)
		{
			if(randNum == i){
				randMesh.renderer.material = TextureMesh[i];
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
