using UnityEngine;
using System.Collections;

public class Menu_MainMenu : MonoBehaviour 
{
	[SerializeField] private GUIStyle sideMenuStyle;
	[SerializeField] private GUIStyle optionsMenuStyle;
	[SerializeField] private Texture2D logo;
	//public static bool canTilt = false;

	[SerializeField] private Texture2D startGameUIButton;
	[SerializeField] private Texture2D startGameUIButtonHighlight;
	[SerializeField] private Texture2D levelSelectUIButton;
	[SerializeField] private Texture2D levelSelectUIButtonHighlight;
	[SerializeField] private Texture2D optionsUIButton;
	[SerializeField] private Texture2D optionsUIButtonHighlight;
	[SerializeField] private Texture2D quitGameUIButton;
	[SerializeField] private Texture2D quitGameUIButtonHighlight;
	[SerializeField] private Texture2D ctmPathfindingUIButton;
	[SerializeField] private Texture2D ctmPathfindingUIButtonHighlight;
	[SerializeField] private Texture2D ctmDirectionalUIButton;
	[SerializeField] private Texture2D ctmDirectionalUIButtonHighlight;
	[SerializeField] private Texture2D useJoystickUIButton;
	[SerializeField] private Texture2D useJoystickUIButtonHighlight;
	[SerializeField] private Texture2D tiltOffUIButton;
	[SerializeField] private Texture2D tiltOffUIButtonHighlight;
	[SerializeField] private Texture2D tiltOnUIButton;
	[SerializeField] private Texture2D tiltOnUIButtonHighlight;
	[SerializeField] private Texture2D applyUIButton;
	[SerializeField] private Texture2D applyUIButtonHighlight;
    [SerializeField] private Texture2D muteAudioUIButton;
    [SerializeField] private Texture2D muteAudioUIButtonHighlight;
    [SerializeField] private Texture2D unmuteAudioUIButton;
    [SerializeField] private Texture2D unmuteAudioUIButtonHighlight;

	[SerializeField] private Texture2D optionsHeader;
	[SerializeField] private Texture2D optionsBox;

	private bool optionsMenuUp = false;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	void OnGUI()
	{
		//Title Logo
		GUI.Label(new Rect(Screen.width*.25f, Screen.height*0.025f, Screen.width*0.5f, Screen.height*0.3f), logo);

		//Side buttons
		sideMenuStyle.normal.background = startGameUIButton;
		sideMenuStyle.hover.background = startGameUIButtonHighlight;
		if ( GUI.Button(new Rect(Screen.width*.75f, Screen.height*0.3f, Screen.width*0.2f, Screen.height*0.1f), "",sideMenuStyle) )
		{
			Application.LoadLevel(2);
		}
		sideMenuStyle.normal.background = levelSelectUIButton;
		sideMenuStyle.hover.background = levelSelectUIButtonHighlight;
		if ( GUI.Button(new Rect(Screen.width*.75f, Screen.height*0.42f, Screen.width*0.2f, Screen.height*0.1f), "",sideMenuStyle) )
		{
			Application.LoadLevel("LevelSelectScene");
		}
		sideMenuStyle.normal.background = optionsUIButton;
		sideMenuStyle.hover.background = optionsUIButtonHighlight;
		if ( GUI.Button(new Rect(Screen.width*.75f, Screen.height*0.53f, Screen.width*0.2f, Screen.height*0.1f), "",sideMenuStyle) )
		{
			optionsMenuUp = true;
		}
		sideMenuStyle.normal.background = quitGameUIButton;
		sideMenuStyle.hover.background = quitGameUIButtonHighlight;
		if ( GUI.Button(new Rect(Screen.width*.75f, Screen.height*0.64f, Screen.width*0.2f, Screen.height*0.1f), "",sideMenuStyle) )
		{
			Application.Quit();
		}
	
		//Options Window
		if (optionsMenuUp)
		{
			//Options Window Background
			optionsMenuStyle.normal.background = optionsBox;
			optionsMenuStyle.hover.background = optionsBox;

			GUI.Box(new Rect(Screen.width*0.1f, Screen.height*0.35f, Screen.width*0.5f, Screen.height*0.6f), "", optionsMenuStyle);

			//Click-To-Move Mode
			if (PlayerMovement.clickToPathfind && !PlayerMovement.useJoystick)
			{
				optionsMenuStyle.normal.background = ctmPathfindingUIButton;
				optionsMenuStyle.hover.background = ctmPathfindingUIButtonHighlight;
				if (GUI.Button(new Rect(Screen.width*0.25f,Screen.height*0.5f, Screen.width*0.2f, Screen.width*0.1f ),"", optionsMenuStyle) )
				{
					PlayerMovement.clickToPathfind = false;
					PlayerMovement.useJoystick = true;
				}
			}
			else if (!PlayerMovement.clickToPathfind && !PlayerMovement.useJoystick)
			{
				optionsMenuStyle.normal.background = ctmDirectionalUIButton;
				optionsMenuStyle.hover.background = ctmDirectionalUIButtonHighlight;
				if (GUI.Button(new Rect(Screen.width*0.25f,Screen.height*0.5f, Screen.width*0.2f, Screen.width*0.1f ), "", optionsMenuStyle) ) 
				{
					PlayerMovement.clickToPathfind = true;
				}
			}
			else if (PlayerMovement.useJoystick)
			{
				optionsMenuStyle.normal.background = useJoystickUIButton;
				optionsMenuStyle.hover.background = useJoystickUIButtonHighlight;
				if (GUI.Button(new Rect(Screen.width*0.25f,Screen.height*0.5f, Screen.width*0.2f, Screen.width*0.1f ), "", optionsMenuStyle) ) 
				{
					PlayerMovement.clickToPathfind = false;
					PlayerMovement.useJoystick = false;
				}
			}
            if (BackgroundAudioManager.sShouldBePlaying)
            {
                optionsMenuStyle.normal.background = muteAudioUIButton;
                optionsMenuStyle.hover.background = muteAudioUIButtonHighlight;
				if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.7f, Screen.width * 0.2f, Screen.width * 0.1f), new GUIContent("", "This is the tooltip"), optionsMenuStyle))
                {
                    BackgroundAudioManager.sShouldBePlaying = false;
                }
            }
            else
            {
                optionsMenuStyle.normal.background = unmuteAudioUIButton;
                optionsMenuStyle.hover.background = unmuteAudioUIButtonHighlight;
				if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.7f, Screen.width * 0.2f, Screen.width * 0.1f), new GUIContent("", "This is the tooltip"), optionsMenuStyle))
                {
                    BackgroundAudioManager.sShouldBePlaying = true;
                }
            }

			//If we're on a iPhone/iPad, give the option to toggle tilt settings
            if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
				optionsMenuStyle.normal.background = tiltOnUIButton;
				optionsMenuStyle.hover.background = tiltOnUIButton;
				if (PlayerMovement.canTilt)
				{
					if (GUI.Button(new Rect(Screen.width*0.25f,Screen.height*0.6f, Screen.width*0.2f, Screen.width*0.1f ),"", optionsMenuStyle) )
					{
						PlayerMovement.canTilt = false;
					}
				}
				else
				{
					optionsMenuStyle.normal.background = tiltOffUIButton;
					optionsMenuStyle.hover.background = tiltOffUIButtonHighlight;
					if (GUI.Button(new Rect(Screen.width*0.25f,Screen.height*0.6f, Screen.width*0.2f, Screen.width*0.1f ), "", optionsMenuStyle) ) 
					{
						PlayerMovement.canTilt = true;
					}
				}
			}
			optionsMenuStyle.normal.background = applyUIButton;
			optionsMenuStyle.hover.background = applyUIButtonHighlight;
			if (GUI.Button(new Rect(Screen.width*0.45f, Screen.height*0.78f, Screen.width*0.1f, Screen.height*0.1f), "", optionsMenuStyle) )
			{
				optionsMenuUp = false;
			}
		}
	
	
	
	}





}
