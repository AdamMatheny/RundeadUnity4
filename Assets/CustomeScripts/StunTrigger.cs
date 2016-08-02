using UnityEngine;
using System.Collections;

public class StunTrigger : Toggler 
{
	public bool toggledActive = false;
	public bool timedSwitch = false;
	public float activeTime = 3f;
	private float TimerLengthrStart = 0f;
	private Color basicColor;
	[SerializeField]
	private bool bActive = false;
	private ParticleSystem myElectric;
    AudioManager mAudioManager;
    public AudioClip mShockPannelSoundEffect;
    public float mShockPannelSoundEffectStartTimer;
    bool mShockPannelSoundEffectplaying = false;
	private bool mAffectedPlayer = false;


	public GameObject openPanel;
	public GameObject closedPanel;

	void Start()
	{
		myElectric = GetComponent<ParticleSystem>();
		//basicColor = renderer.material.color;
		myElectric.enableEmission = false;
		openPanel.GetComponent<MeshRenderer>().enabled = false;
		closedPanel.GetComponent<MeshRenderer>().enabled = true;
		if(bActive)
		{
			myElectric.enableEmission = true;
			openPanel.GetComponent<MeshRenderer>().enabled = true;
			closedPanel.GetComponent<MeshRenderer>().enabled = false;
			//renderer.material.color = Color.yellow;
		}
        mAudioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
        if (mAudioManager)
        {
            mAudioManager.AddSoundEffect("Shock Pannel", mShockPannelSoundEffect);
        }
	}
	void Update()
	{
		if(timedSwitch && Time.time > activeTime + TimerLengthrStart && !toggledActive)
		{
			bActive = false;
			myElectric.enableEmission = false;
			openPanel.GetComponent<MeshRenderer>().enabled = false;
			closedPanel.GetComponent<MeshRenderer>().enabled = true;
			//renderer.material.color = basicColor;
		}

        if ((mShockPannelSoundEffectplaying) && bActive)
        {
            if (Time.time > mShockPannelSoundEffectStartTimer + mShockPannelSoundEffect.length)
            {
                mShockPannelSoundEffectplaying = false;
            }
        }

        if ((!mShockPannelSoundEffectplaying) && bActive)
        {
            mAudioManager.PlaySoundEffect("Shock Pannel");
            mShockPannelSoundEffectplaying = true;
            mShockPannelSoundEffectStartTimer = Time.time;
        }
	}
	private void TimedActivation(float time)
	{
		activeTime = time;
		TimerLengthrStart = Time.time;
		bActive = true;
		myElectric.enableEmission = true;
		openPanel.GetComponent<MeshRenderer>().enabled = true;
		closedPanel.GetComponent<MeshRenderer>().enabled = false;
	}
	//If the stunable entity is stays here while its active, zap them
	void OnTriggerEnter(Collider other)
	{
		if (bActive)
		{
			if(other.gameObject.GetComponent<PlayerMovement>())
			{
				other.gameObject.GetComponent<PlayerMovement>().mNumShocks++;
			}
			Stunable stunOther = other.gameObject.GetComponent<Stunable>();
			if (stunOther != null)
			{
				stunOther.Stun();
                stunOther.mTypeofStun = Stunable.StunType.ShockStun;
			}
		}
	}
	void OnTriggerStay(Collider other)
	{
		PlayerMovement player = other.GetComponent<PlayerMovement>();

		if (bActive)
		{
			Stunable stunOther = other.gameObject.GetComponent<Stunable>();
			if (stunOther != null)
			{
				stunOther.Stun();
                stunOther.mTypeofStun = Stunable.StunType.ShockStun;
			}

			if (player != null)
			{
				if (player.mNumShocks <=1)
				{
					if(!mAffectedPlayer)
					{
						player.mShockStartTime = Time.time;
						player.mInShock = true;
						player.GetComponent<Stunable>().mTypeofStun = Stunable.StunType.ShockStun;
						player.mShockBarLength = other.GetComponent<PlayerMovement>().mShockTimer * 10;

						mAffectedPlayer = true;
					}
				}
				else
				{
					mAffectedPlayer = true;
				}
			}
		}
		else
		{
			if (player != null)
			{
				player.mInShock = false;
				mAffectedPlayer = false;
                player.GetComponent<Stunable>().mTypeofStun = Stunable.StunType.NotStunned;
			}
		}
		player = null;
	}

	void OnTriggerExit(Collider other)
	{
		PlayerMovement player = other.GetComponent<PlayerMovement>();

		if (player != null)
		{
			player.mNumShocks--;
			if(player.mNumShocks <=0)
			{
				player.mNumShocks = 0;
				player.mInShock = false;
				mAffectedPlayer = false;
				player.GetComponent<Stunable>().mTypeofStun = Stunable.StunType.NotStunned;
			}
		}
	}

	public override bool Activate(Collider other, GameObject parent)
	{
		bool activated = false;

		if (parent.GetComponent<SwitchToggler>() != null)
		{
			if(parent.GetComponent<SwitchToggler>().timedSwitch)
			{
				TimedActivation(parent.GetComponent<SwitchToggler>().TimerLength);
				toggledActive = false;
				activated = true;
				//renderer.material.color = Color.yellow;
				myElectric.enableEmission = true;
				openPanel.GetComponent<MeshRenderer>().enabled = true;
				closedPanel.GetComponent<MeshRenderer>().enabled = false;
			}
			else
			{
				toggledActive = !toggledActive;
				bActive = !bActive;
				activated = bActive;
				if(bActive)
				{
					myElectric.enableEmission = true;
					openPanel.GetComponent<MeshRenderer>().enabled = true;
					closedPanel.GetComponent<MeshRenderer>().enabled = false;
					//renderer.material.color = Color.yellow;
                    mAudioManager.gameObject.transform.position = this.gameObject.transform.position;
				}
				else
				{
					myElectric.enableEmission = false;
					openPanel.GetComponent<MeshRenderer>().enabled = false;
					closedPanel.GetComponent<MeshRenderer>().enabled = true;
					//renderer.material.color = basicColor;
				}
			}
			base.Activate(other, parent);
		}
		return activated;
	}
	public override bool Reactivate(Collider other, GameObject parent)
	{
		bool activated = false;

		if (parent.GetComponent<SwitchToggler>() != null)
		{
			if (parent.GetComponent<SwitchToggler>().timedSwitch)
			{
				TimedActivation(parent.GetComponent<SwitchToggler>().TimerLength);
				toggledActive = false;
				activated = true;
				myElectric.enableEmission = true;
				openPanel.GetComponent<MeshRenderer>().enabled = true;
				closedPanel.GetComponent<MeshRenderer>().enabled = false;
				//renderer.material.color = Color.yellow;
			}
			base.Reactivate(other, parent);
		}
		return activated;
	}
	public override bool Deactivate(Collider other, GameObject parent)
	{
		bool activated = false;

		if (parent.GetComponent<SwitchToggler>() != null)
		{
			activated = base.Deactivate(other, parent);
		}
		return activated;
	}
}
