using UnityEngine;
using System.Collections;

public class LevelHUD : MonoBehaviour 
{
	GameObject playerAvatar;
	string keyText;
	//[SerializeField] private string levelName;
	public bool playerDied;
	public bool vitalNPCDied;

	public Texture2D[] badgeIcons;
	Texture2D badgeToDisplay;

    public bool mShowHUD = true;

	// Use this for initialization
	void Start () 
	{
		playerAvatar = GameObject.Find("PlayerAvatar");
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (playerAvatar.GetComponent<Keybearer>().posessedKey)
		{
		case 0:
			keyText = "BASIC ";
			badgeToDisplay = badgeIcons[0];
			break;
		case 1:
			keyText = "BLUE ";
			badgeToDisplay = badgeIcons[1];
			break;
		case 2:
			keyText = "YELLOW ";
			badgeToDisplay = badgeIcons[2];
			break;
		case 3:
			keyText = "CYAN ";
			badgeToDisplay = badgeIcons[3];
			break;
		case 4:
			keyText = "MAGENTA ";
			badgeToDisplay = badgeIcons[4];
			break;
		default:
			keyText = "BASIC ";
			badgeToDisplay = badgeIcons[0];
			break;
		}
		//Debug.Log (1 / Time.deltaTime);
	}

	void OnGUI ()
	{
        if (mShowHUD)
        {
            if (playerAvatar.GetComponent<PlayerMovement>().NumberofShields == 0)
            {
                int levelNumber = Application.loadedLevel - 1;
                GUI.Box(new Rect(10, 10, Screen.width * 0.2f, Screen.height * 0.05f), "Current level: " + levelNumber
                                                    + "\nYou have the " + keyText + "key.");
            }
            else
            {
                int levelNumber = Application.loadedLevel + 1;
                GUI.Box(new Rect(10, 10, Screen.width * 0.2f, Screen.height * 0.075f), "Current level: " + levelNumber
                        + "\nYou have the " + keyText + "key."
                        + "\nYou are shielded.");
            }
            GUI.Box(new Rect(10, Screen.height * 0.1f, Screen.width * 0.075f, Screen.width * 0.1f), badgeToDisplay);
            if (playerDied)
			{
				//GUI.Box(new Rect(0, Screen.height * 0.4f, Screen.width, Screen.height * 0.2f), "YOU DIED!");
			}
               
            else if (vitalNPCDied)
			{
				//GUI.Box(new Rect(0, Screen.height * 0.4f, Screen.width, Screen.height * 0.2f), "YOUR COMPANION DIED! ");
			}

                
            //	GUI.Label(new Rect(310, 55, 200, 100), "FPS:" + (1 / Time.deltaTime));
        }
	}
}
