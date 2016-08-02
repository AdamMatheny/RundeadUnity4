using UnityEngine;
using System.Collections;

public class TopDownCamera : MonoBehaviour 
{

	[SerializeField] private GameObject avatar;
	[SerializeField] private Vector3 followDistance = new Vector3(0,8,-7);
	[SerializeField] private Vector3 followAngle = new Vector3(60,0,0);
    public float mZoomOutTimer = 3.5f;
	public bool DisableAtStartofLvl1 = false;
    private float mSpeed = 0;
	private bool zoomedIn = false;

	// Use this for initialization
	void Start () 
	{
		transform.position = avatar.transform.position+followDistance;
		transform.rotation = Quaternion.Euler(followAngle);

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (DisableAtStartofLvl1)
		{
			camera.enabled = false;
		}
		if (!zoomedIn)
		{
			if (Vector3.Distance(transform.position, avatar.transform.position+followDistance) < 1 )
			{
				transform.position = avatar.transform.position+followDistance;
				transform.rotation = Quaternion.Euler(followAngle);

			}
			else
			{
				//Debug.Log("Zooming out to the player");
                if (mSpeed == 0)
                {
                    transform.position = Vector3.Lerp(transform.position, avatar.transform.position+followDistance, Time.deltaTime);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(followAngle), Time.deltaTime);
                }
				else
                {
                    transform.position = Vector3.Lerp(transform.position, avatar.transform.position + followDistance, Time.deltaTime * mSpeed);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(followAngle), Time.deltaTime * mSpeed);

                    mSpeed = 0;
                }
			}
		}
		else
		{
			camera.enabled = true;
		}
	}

	public void ZoomIn(Transform zoomTransform, float speed = 0)
	{
		
        mSpeed = speed;
        if (speed == 0)
        {
            transform.position = Vector3.Lerp(transform.position, zoomTransform.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, zoomTransform.rotation, Time.deltaTime);
        }
        else
        {
            //transform.position = Vector3.MoveTowards(transform.position, zoomTransform.position, Time.deltaTime * speed);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, zoomTransform.rotation, Time.deltaTime * speed);
            transform.position = Vector3.Lerp(transform.position, zoomTransform.position, Time.deltaTime * speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, zoomTransform.rotation, Time.deltaTime * speed);
        }
        
		zoomedIn = true;

	}
	public void ZoomOut()
	{
		zoomedIn = false;
        Invoke("ReturnToPlayer", mZoomOutTimer);
	}

    public void ReturnToPlayer()
    {
        PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
        if (player)
        {
            player.gameObject.GetComponent<Stunable>().IsStunned = false;
        }
    }

}
