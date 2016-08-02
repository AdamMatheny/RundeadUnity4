using UnityEngine;
using System.Collections;

public class HintScreen : MonoBehaviour 
{
	//for timing how long to keep the hint screen up
	[SerializeField] private float pauseLength = 1f;
	float pauseStartTime;

	//what to show on the hint screen
	[SerializeField] private Texture2D hintScreenBackground;
	[SerializeField] private string[] hintList;
	int hintToShow = 0;
	[SerializeField] private GUIStyle hintScreenStyle;

	//the things we want to disable while the hint screen is up
	PlayerMovement movementScript;
	PauseButton playerPause;
	LevelHUD theHUD;


	// Use this for initialization
	void Start () 
	{
		//start the timer
		pauseStartTime = Time.time;

		//choose which hint to show
		if (hintList.Length > 0)
		{
			hintToShow = Mathf.RoundToInt(Random.Range(0, hintList.Length));
			Debug.Log(hintToShow);
		}

		//find the things you want to disable
		movementScript = GameObject.FindObjectOfType<PlayerMovement>();
		playerPause = GameObject.FindObjectOfType<PauseButton>();
		theHUD = GameObject.FindObjectOfType<LevelHUD>();

		//disable the things you want to disable
		movementScript.enabled = false;
		playerPause.enabled = false;
		theHUD.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time > pauseStartTime+pauseLength)
		{
			movementScript.enabled = true;
			playerPause.enabled = true;
			theHUD.enabled = true;
		}

		if (Time.time > pauseStartTime+pauseLength+0.011f)
		{
			this.enabled = false;
		}

	}

	void OnGUI ()
	{
		if (Time.time <= pauseStartTime+pauseLength)
		{
			GUI.Label(new Rect(0f,0f,Screen.width,Screen.height), hintScreenBackground, hintScreenStyle);
			if (hintList.Length > 0 && hintList[hintToShow] != null)
			{
				GUI.Label(new Rect(Screen.width*0.2f,Screen.height*0.6f,Screen.width*0.6f,Screen.height*0.3f), hintList[hintToShow], hintScreenStyle);
			}
		}
	}
}
