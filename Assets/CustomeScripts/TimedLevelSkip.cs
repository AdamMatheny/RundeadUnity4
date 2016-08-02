using UnityEngine;
using System.Collections;

public class TimedLevelSkip : MonoBehaviour 
{
	[SerializeField] private float timeOnThisLevel;
	private float startTime;
	// Use this for initialization
	void Start () 
	{
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > timeOnThisLevel+startTime)
		{
			if (Application.loadedLevel == Application.levelCount-1)
			{
				Application.LoadLevel(0);
			}
			else
			{
				Application.LoadLevel(Application.loadedLevel+1);
			}
		}
	}
}
