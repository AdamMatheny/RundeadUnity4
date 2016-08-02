using UnityEngine;
using System.Collections;

public class BackgroundAudioManager : MonoBehaviour 
{
    public AudioClip mBackgroundMusic;
    public float mBackgroundVolume = 0.25f;
    public static bool sShouldBePlaying = true;
	// Use this for initialization
	void Start () 
    {
        if (mBackgroundMusic && sShouldBePlaying)
        {
            audio.loop = true;
            audio.volume = mBackgroundVolume;
            audio.clip = mBackgroundMusic;
            audio.Play();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (mBackgroundMusic && sShouldBePlaying && !audio.isPlaying)
        {
            audio.loop = true;
            audio.volume = mBackgroundVolume;
            audio.clip = mBackgroundMusic;
            audio.Play();
        }

        if (audio.isPlaying && !sShouldBePlaying)
        {
            audio.Stop();
        }
	}
}
