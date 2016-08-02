using UnityEngine;
using System.Collections;

public class GasMaskPickup : MonoBehaviour {

    public bool mOnlyonce = true;
    public float mExtraTimeInGas = 10.0f;
	bool gotOnce = false;
	

	void Start()
	{
        
	}

	// Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Run player gain shield function
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
				if (mOnlyonce == true && gotOnce == true)
				{
					GetComponent<Animator>().Play("Empty");
				}
				else if(mOnlyonce == true && gotOnce == false)
				{
					player.mHasGasMask = true;
					player.mGasStartTime = Time.time;
					GetComponent<Animator>().Play("Retract");
					gotOnce = true;
				}
				else
				{
					player.mHasGasMask = true;
					player.mGasStartTime = Time.time;
					GetComponent<Animator>().Play("Fill");
				}
            }
            
        }
    }
}
