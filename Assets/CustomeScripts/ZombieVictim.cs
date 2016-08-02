using UnityEngine;
using System.Collections;

public class ZombieVictim : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void TurnToTheUndead()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = false;
        }
        GameObject zombie = (GameObject)Instantiate(Resources.Load("Zombie"));
        zombie.transform.position = this.gameObject.transform.position;
        zombie.transform.rotation = this.gameObject.transform.rotation;
        
    }
}
