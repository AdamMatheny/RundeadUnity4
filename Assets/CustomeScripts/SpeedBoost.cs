using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour {

    [SerializeField]
    public float mSpeed = 7.0f;
    [SerializeField]
    public float mAcceleration = 16.0f;
    [SerializeField]
    public float mTimer = 120.0f;


	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           // Debug.Log("Player hit the puddle");
        }
    }
}
