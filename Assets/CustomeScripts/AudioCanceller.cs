using UnityEngine;
using System.Collections;

public class AudioCanceller : MonoBehaviour 
{
    public TextBoxSummoner mCancelledObject;

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
        if ((other.tag == "Player") && (mCancelledObject))
        {
            AudioManager manager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
            PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
            PauseButton pauseButton = FindObjectOfType(typeof(PauseButton)) as PauseButton;
            if (manager)
            {
                manager.audio.Stop();
            }
            mCancelledObject.mDisplayedMessageAlready = true;
            if (mCancelledObject.mPlayOnce)
            {
                Assets.CustomeScripts.InformationManager.UpdateTextBoxSummonerState(mCancelledObject.mTextBoxId);
            }
            if (mCancelledObject.mZoom)
            {
                Camera.main.GetComponent<TopDownCamera>().ZoomOut();
                mCancelledObject.mZoom = false;
                mCancelledObject.GetComponent<ZoomArea>().enabled = false;
            }
            if (player)
            {
                player.gameObject.GetComponent<Stunable>().IsStunned = false;
                player.mShowGUI = true;
            }
            if (pauseButton)
            {
                pauseButton.mShowGUI = true;
            }
        }
    }
}
