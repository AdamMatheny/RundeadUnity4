using UnityEngine;
using System.Collections;

public class Menu_LevelSelect : MonoBehaviour 
{

	[SerializeField] private GUIStyle levelHeaderStyle;
	[SerializeField] private Texture2D levelSelectHeader;
	[SerializeField] private Texture2D mainMenuUIButton;
	[SerializeField] private Texture2D mainMenuUIButtonHighlight;
	[SerializeField] private GUIStyle levelSelectStyle;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	void OnGUI ()
	{
		//Level Select Text
		levelHeaderStyle.normal.background = null;
		GUI.Label(new Rect(Screen.width*.25f, Screen.height*0.05f, Screen.width*0.5f, Screen.height*0.15f), levelSelectHeader, levelHeaderStyle);

		//Return to Main Menu Button
		levelHeaderStyle.normal.background = mainMenuUIButton;
		levelHeaderStyle.hover.background = mainMenuUIButtonHighlight;
		if ( GUI.Button(new Rect(Screen.width*0.01f, Screen.height*0.1f, Screen.width*0.2f, Screen.height*0.1f), "", levelHeaderStyle) )
		{
			Application.LoadLevel("MainMenuScene");
		}

		//make an field of buttons based on how many levels we have
		float buttonXPos;
		float buttonYPos;
		for (int i = 0; i < Application.levelCount-2; i++)
		{
			buttonXPos = Screen.width*0.35f + (i%3 * Screen.width * 0.11f);
			buttonYPos = Screen.height*0.25f + (i/3 * Screen.height * 0.11f);
			if (GUI.Button(new Rect(buttonXPos,buttonYPos, Screen.width*0.1f, Screen.height*0.1f ),""+(i+1), levelSelectStyle) )
			{
				Application.LoadLevel(i+2);
			}
		}
	}
}
