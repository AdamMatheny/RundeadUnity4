using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour 
{

	GameObject playerAvatar;

	bool paused = false;
	bool inOptions = false;

	[SerializeField] private Texture2D pauseUIButton;
	[SerializeField] private Texture2D pauseUIButtonHighlight;
	[SerializeField] private Texture2D resumeUIButton;
	[SerializeField] private Texture2D resumeUIButtonHighlight;
	[SerializeField] private Texture2D restartLevelUIButton;
	[SerializeField] private Texture2D restartLevelUIButtonHighlight;
	[SerializeField] private Texture2D skipLevelUIButton;
	[SerializeField] private Texture2D skipLevelUIButtonHighlight;
	[SerializeField] private Texture2D mainMenuUIButton;
	[SerializeField] private Texture2D mainMenuUIButtonHighlight;
	[SerializeField] private Texture2D levelSelectUIButton;
	[SerializeField] private Texture2D levelSelectUIButtonHighlight;
	[SerializeField] private Texture2D optionsUIButton;
	[SerializeField] private Texture2D optionsUIButtonHighlight;
	[SerializeField] private Texture2D ctmPathfindingUIButton;
	[SerializeField] private Texture2D ctmPathfindingUIButtonHighlight;
	[SerializeField] private Texture2D ctmDirecitonalUIButton;
	[SerializeField] private Texture2D ctmDirecitonalUIButtonHighlight;
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
	[SerializeField] private Texture2D pauseBackgroundWindow;
	[SerializeField] private Texture2D pauseOptionsBackgroundWindow;

	public GUIStyle pauseGUIStyle;
    public bool mShowGUI = true;

	// Use this for initialization
	void Start () 
	{
		playerAvatar = GameObject.Find("PlayerAvatar");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnGUI()
	{
		if (mShowGUI)
        {
            if (paused)
            {
                if (!inOptions)
                {
					//draw the background window
					pauseGUIStyle.normal.background = pauseBackgroundWindow;
					GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.125f, Screen.width * 0.8f, Screen.height * 0.8f), "", pauseGUIStyle);

					//pauseGUIStyle.onNormal
                    pauseGUIStyle.normal.background = resumeUIButton;
                    pauseGUIStyle.hover.background = resumeUIButtonHighlight;
                    if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.2f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "Resume Game"), pauseGUIStyle))
                    {
                        paused = false;
                        Time.timeScale = 1;
                    }
                    pauseGUIStyle.normal.background = restartLevelUIButton;
                    pauseGUIStyle.hover.background = restartLevelUIButtonHighlight;
                    if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.31f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "Restart the Level"), pauseGUIStyle))
                    {
                        paused = false;
                        Time.timeScale = 1;
                        PlayerPrefs.DeleteKey("CheckPointReached");
                        PlayerPrefs.DeleteKey("PlayerKey");
                        Application.LoadLevel(Application.loadedLevel);
                    }
                    pauseGUIStyle.normal.background = skipLevelUIButton;
                    pauseGUIStyle.hover.background = skipLevelUIButtonHighlight;
                    if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.42f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "Skip current level"), pauseGUIStyle))
                    {
                        paused = false;
                        Time.timeScale = 1;
                        PlayerPrefs.DeleteKey("CheckPointReached");
                        PlayerPrefs.DeleteKey("PlayerKey");
                        if (Application.loadedLevel < Application.levelCount)
                            Application.LoadLevel(Application.loadedLevel + 1);
                        else
                            Application.LoadLevel("MainMenuScene");
                    }
                    pauseGUIStyle.normal.background = mainMenuUIButton;
                    pauseGUIStyle.hover.background = mainMenuUIButtonHighlight;
                    if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.53f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "Go to the Main Menu"), pauseGUIStyle))
                    {
                        paused = false;
                        Time.timeScale = 1;
                        PlayerPrefs.DeleteKey("CheckPointReached");
                        PlayerPrefs.DeleteKey("PlayerKey");
                        Application.LoadLevel("MainMenuScene");
                    }
                    pauseGUIStyle.normal.background = levelSelectUIButton;
                    pauseGUIStyle.hover.background = levelSelectUIButtonHighlight;
                    if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.64f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "Go to the Level Selection screen"), pauseGUIStyle))
                    {
                        paused = false;
                        Time.timeScale = 1;
                        PlayerPrefs.DeleteKey("CheckPointReached");
                        PlayerPrefs.DeleteKey("PlayerKey");
                        Application.LoadLevel("LevelSelectScene");
                    }
                    pauseGUIStyle.normal.background = optionsUIButton;
                    pauseGUIStyle.hover.background = optionsUIButtonHighlight;
                    if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.75f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "Control Options"), pauseGUIStyle))
                    {
                        inOptions = true;
                    }
                }
                else
                {
					//draw the background window
					pauseGUIStyle.normal.background = pauseOptionsBackgroundWindow;
					GUI.Label(new Rect(Screen.width * 0.1f, Screen.height * 0.125f, Screen.width * 0.8f, Screen.height * 0.8f), "", pauseGUIStyle);

					//Set Click-To-Move type (Pathfind vs. Directional)
                    if (PlayerMovement.clickToPathfind && !PlayerMovement.useJoystick)
                    {
                        pauseGUIStyle.normal.background = ctmPathfindingUIButton;
                        pauseGUIStyle.hover.background = ctmPathfindingUIButtonHighlight;
                        if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.45f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "This is the tooltip"), pauseGUIStyle))
                        {
                            PlayerMovement.clickToPathfind = false;
							PlayerMovement.useJoystick = true;
						}
                    }
					else if (!PlayerMovement.clickToPathfind && !PlayerMovement.useJoystick)
                    {
                        pauseGUIStyle.normal.background = ctmDirecitonalUIButton;
                        pauseGUIStyle.hover.background = ctmDirecitonalUIButtonHighlight;
                        if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.45f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "This is the tooltip"), pauseGUIStyle))
                        {
                            PlayerMovement.clickToPathfind = true;
						}
                    }
					else if (PlayerMovement.useJoystick)
					{
						pauseGUIStyle.normal.background = useJoystickUIButton;
						pauseGUIStyle.hover.background = useJoystickUIButtonHighlight;
						if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.45f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "This is the tooltip"), pauseGUIStyle))
						{
							PlayerMovement.useJoystick = false;
							PlayerMovement.clickToPathfind = false;
							Joystick theJoystick = FindObjectOfType(typeof(Joystick)) as Joystick;
							//theJoystick.ToggleJoystickEnabled();
						}
					}

                    if (BackgroundAudioManager.sShouldBePlaying)
                    {
                        pauseGUIStyle.normal.background = muteAudioUIButton;
                        pauseGUIStyle.hover.background = muteAudioUIButtonHighlight; 
                        if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.35f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "This is the tooltip"), pauseGUIStyle))
                        {
                            BackgroundAudioManager.sShouldBePlaying = false;
                            //theJoystick.ToggleJoystickEnabled();
                        }
                    }
                    else 
                    {
                        pauseGUIStyle.normal.background = unmuteAudioUIButton;
                        pauseGUIStyle.hover.background = unmuteAudioUIButtonHighlight;
                        if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.35f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "This is the tooltip"), pauseGUIStyle))
                        {
                            BackgroundAudioManager.sShouldBePlaying = true;
                            //theJoystick.ToggleJoystickEnabled();
                        }
                    }
                    //If we're on a iPhone/iPad, give the option to toggle tilt settings
                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        if (PlayerMovement.canTilt)
                        {
                            pauseGUIStyle.normal.background = tiltOnUIButton;
                            pauseGUIStyle.hover.background = tiltOnUIButtonHighlight;
							if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.55f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "This is the tooltip"), pauseGUIStyle))
                            {
                                PlayerMovement.canTilt = false;
                            }
                        }
                        else
                        {
                            pauseGUIStyle.normal.background = tiltOffUIButton;
                            pauseGUIStyle.hover.background = tiltOffUIButtonHighlight;
							if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.55f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "This is the tooltip"), pauseGUIStyle))
                            {
                                PlayerMovement.canTilt = true;
                            }
                        }
                    }
                    pauseGUIStyle.normal.background = applyUIButton;
                    pauseGUIStyle.hover.background = applyUIButtonHighlight;
                    if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.75f, Screen.width * 0.2f, Screen.height * 0.1f), new GUIContent("", "Apply options"), pauseGUIStyle))
                    {
                        inOptions = false;
                    }

                }



            }
            else if (GameObject.Find("LevelExit").GetComponent<LevelExit>().levelDone == false)
            {
                //Debug.Log("Making a pause button");
                pauseGUIStyle.normal.background = pauseUIButton;
                pauseGUIStyle.hover.background = pauseUIButtonHighlight;
                if (GUI.Button(new Rect(Screen.width - Screen.width * 0.1f, 0, Screen.width * 0.1f, Screen.height * 0.1f), new GUIContent("", "Pause Game"), pauseGUIStyle))
                {
                    paused = true;
                    Time.timeScale = 0;
                }
            }

            if (Event.current.type == EventType.Repaint)
            {
                if (!paused)
                {
                    if (GUI.tooltip != "")
                    {
                        playerAvatar.GetComponent<PlayerMovement>().mouseOverGUI = true;
                    }
                    else
                    {
                        playerAvatar.GetComponent<PlayerMovement>().mouseOverGUI = false;
                    }
                }
                else
                {
                    playerAvatar.GetComponent<PlayerMovement>().mouseOverGUI = true;
                }
            }
        }
	}


}
