using UnityEngine;
using System.Collections;

public class ShieldPickup : MonoBehaviour 
{
    public bool mOnlyonce = false;
	private GameObject playerAvatar;
	GameObject Sid;
	GameObject Sid_Dis;
	bool gotOnce = false;


	void Start()
	{
		Sid = GameObject.Find("Sid_All_Anim");
		playerAvatar = GameObject.Find("PlayerAvatar");
		Sid_Dis = GameObject.Find("SID_Dispenser");
	}
	//Player overlaps this pickup
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
            //Run player gain shield function
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if (player != null && player.NumberofShields == 0)
            {
				if(mOnlyonce == true && gotOnce == true)
				{
					Sid_Dis.GetComponent<Animator>().Play ("Empty");
				}
				else if(mOnlyonce == true && gotOnce == false)
				{
					player.NumberofShields = 1;
					Sid.transform.position = playerAvatar.transform.position;
					Sid.GetComponent<Animator>().Play("Idle");
					gotOnce = true;
					Sid_Dis.GetComponent<Animator>().Play ("Empty");
				}
				else
				{
					player.NumberofShields = 1;
					Sid.transform.position = playerAvatar.transform.position;
					Sid.GetComponent<Animator>().Play("Idle");
					gotOnce = true;
					Sid_Dis.GetComponent<Animator>().Play ("Pop");
				}
                
            }//end if player is there and num shields == 0

		}
	}
}
