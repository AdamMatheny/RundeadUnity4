using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;


public class AudioManager : MonoBehaviour
{

    private Dictionary<string, AudioClip> mAudioList = new Dictionary<string,AudioClip>();
    private Dictionary<string, AudioClip> mSoundEffects = new Dictionary<string, AudioClip>();
    public bool mIsPlaying = false;
    private float mCurrentClipLength = 0.0f;
    private float mStartTime;
    // Use this for initialization
    void Start()
    {
        
    }

    void Update()
    {
        if (isPlaying() && mIsPlaying == false)
        {
            mIsPlaying = true;
        }
        if (!isPlaying() && mIsPlaying == true)
        {
            mIsPlaying = false;
        }
    }

    public void AddAudioClip(string title, AudioClip clip)
    {
        if (!mAudioList.ContainsKey(title))
        {
            mAudioList.Add(title, clip);
        }
    }

    public void PlayAudioClip(string title, bool remove = true)
    {
        AudioClip clip;
		audio.Stop();
        if (mAudioList.TryGetValue(title, out clip))
        {
            PlayAudioOnce(clip, 1.0f);
            if (remove)
            {
                mAudioList.Remove(title);
            }
        }
    }

    public void RemoveAudioClip(string title)
    {
        if (mAudioList.ContainsKey(title))
        {
            mAudioList.Remove(title);
        }
    }

    public void AddSoundEffect(string title, AudioClip clip)
    {
        if (!mSoundEffects.ContainsKey(title))
        {
            mSoundEffects.Add(title, clip);
        }
    }

    public void PlaySoundEffect(string title)
    {
        AudioClip clip;
        if (mSoundEffects.TryGetValue(title, out clip))
        {
            PlayAudioOnce(clip, 1.0f);
        }
    }

    public void RemoveSoundEffect(string title)
    {
        if (mSoundEffects.ContainsKey(title))
        {
            mSoundEffects.Remove(title);
        }
    }

    private void PlayAudioOnce(AudioClip clip, float volume = 1.0f)
    {
        audio.PlayOneShot(clip, volume);
        mStartTime = Time.time;
        mCurrentClipLength = clip.length;
    }

    public bool isPlaying()
    {
        if ((Time.time - mStartTime) >= mCurrentClipLength)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}


