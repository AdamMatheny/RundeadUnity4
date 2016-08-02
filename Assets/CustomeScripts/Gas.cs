using UnityEngine;
using System.Collections;

public class Gas : Toggler {

    private bool mAffectedPlayer = false;
    public bool mGasActive = true;

	private ParticleSystem myGas;

    AudioManager mAudioManager;
    public AudioClip mGasMovingSoundEffect;
    public float mGasSoundEffectStartTimer;
    bool mGasSoundPlaying = false;
    public bool mGasMaskAffected = false;

	// Use this for initialization
	void Start () 
    {
		myGas = GetComponent<ParticleSystem>();
	    if (!mGasActive)
        {
            renderer.enabled = false;
			myGas.enableEmission = false;
        }

        mAudioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
        if (mAudioManager)
        {
            mAudioManager.AddSoundEffect("Gas Travel", mGasMovingSoundEffect);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (mGasSoundPlaying)
        {
            if (Time.time > mGasSoundEffectStartTimer + mGasMovingSoundEffect.length)
            {
                mGasSoundPlaying = false;
            }
        }
	}

    public override bool Activate(Collider other, GameObject parent)
    {
        if (!mGasSoundPlaying)
        {
            mAudioManager.PlaySoundEffect("Gas Travel");
            mGasSoundPlaying = true;
            mGasSoundEffectStartTimer = Time.time;
        }
        mGasActive = !mGasActive;
        //renderer.enabled = !renderer.enabled;
		myGas.enableEmission = mGasActive;
        return base.Activate(other, parent);
    }

    //while colliding with the gas, Dinos and the Player will be affected by it
    void OnTriggerStay(Collider other)
    {
        if (mGasActive)
        {
            //if a Dino is colliding with the gas, it is stunned and will stay stunned as long as it touches the gas
            if (other.GetComponent<DinoAI>())
            {
                other.GetComponent<DinoAI>().GetComponent<Stunable>().bStayStunned = true;
                other.GetComponent<DinoAI>().GetComponent<Stunable>().IsStunned = true;
                other.GetComponent<DinoAI>().GetComponent<Stunable>().mTypeofStun = Stunable.StunType.GasStun;
				other.GetComponent<DinoAI>().GetComponent<Animator>().SetBool("SleepBool", true);
				other.GetComponent<DinoAI>().GetComponent<Animator>().SetBool("ShockBool", false);
            }
            else if (other.GetComponent<PlayerMovement>())
            {
                if ( other.GetComponent<PlayerMovement>().mNumGases <=1)
                {
					if(!mAffectedPlayer)
					{
						other.GetComponent<PlayerMovement>().GetComponent<Stunable>().mTypeofStun = Stunable.StunType.GasStun;
						if (!other.GetComponent<PlayerMovement>().mHasGasMask || (other.GetComponent<PlayerMovement>().mHasGasMask && !mGasMaskAffected))
						{
							other.GetComponent<PlayerMovement>().mGasStartTime = Time.time;
						}
                    
						other.GetComponent<PlayerMovement>().mInGas = true;
						if (other.GetComponent<PlayerMovement>().mHasGasMask)
						{
							other.GetComponent<PlayerMovement>().mGasBarLength = other.GetComponent<PlayerMovement>().mGasTimer * 10;
							mGasMaskAffected = true;
							if (other.GetComponent<PlayerMovement>().mGasesVisited.Contains(this))
							{
								other.GetComponent<PlayerMovement>().mGasesVisited.Add(this);
							}
                        
						}
						else
						{
							other.GetComponent<PlayerMovement>().mGasBarLength = other.GetComponent<PlayerMovement>().mGasMaskTimer * 10;
						}
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
            if (other.GetComponent<DinoAI>())
            {
                DinoAI dino = other.GetComponent<DinoAI>();
                dino.GetComponent<Stunable>().bStayStunned = false;
                dino.GetComponent<Stunable>().IsStunned = false;
				other.GetComponent<DinoAI>().GetComponent<Animator>().SetBool("SleepBool", false);
            }
            else if (other.GetComponent<PlayerMovement>())
            {
                PlayerMovement player = other.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.mInGas = false;
                    mAffectedPlayer = false;
                }

            }
        }
        
    }
    //after the gas leaves, Dinos resume can be unstun and will begin functioning normal again
    //after the gas leaves, the Player's gas timers are reset, and gas masks removed if any
	void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DinoAI>())
        {
            DinoAI dino = other.GetComponent<DinoAI>();
            dino.GetComponent<Stunable>().bStayStunned = false;
            dino.GetComponent<Stunable>().IsStunned = false;
            other.GetComponent<DinoAI>().GetComponent<Stunable>().mTypeofStun = Stunable.StunType.NotStunned;
        }
        else if (other.GetComponent<PlayerMovement>())
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            player.GetComponent<Stunable>().mTypeofStun = Stunable.StunType.NotStunned;
            if (player != null)
            {
				player.mNumGases--;
				if(player.mNumGases <= 0)
				{
					player.mInGas = false;
					mAffectedPlayer = false;
				}
                //if (player.mHasGasMask)
                //{
                //    player.mGasTimer = player.mInitialGasTimer;
                //    player.mHasGasMask = false;
                //}
            }
            
        }
    }
}
