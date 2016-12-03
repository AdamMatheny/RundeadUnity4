using UnityEngine;
using System.Collections;

public class LevelExit : MonoBehaviour 
{

	public bool levelDone = false;
	public int waitTime = 0;

	// Use this for initialization
	void Start () 
	{
	}


	// Update is called once per frame
	void Update () 
	{

	}


	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			levelDone = true;
			//Time.timeScale = 0;
			StartCoroutine(waitToChangeToDone());
		}

	}

	IEnumerator waitToChangeToDone() {
		yield return new WaitForSeconds(waitTime);
		changeToDone();
	}
	void changeToDone()
	{
			if (levelDone)
			{
				levelDone = false;
				Time.timeScale = 1;
				PlayerPrefs.DeleteKey("CheckPointReached");
				PlayerPrefs.DeleteKey("PlayerKey");
				if (Application.loadedLevel < Application.levelCount - 1)
				{
					Application.LoadLevel(Application.loadedLevel+1);
				}
				else
				{
					Application.LoadLevel("MainMenuScene");
				}
			}
	}
	void OnGUI()
	{
//		if (levelDone)
//		{
//			levelDone = false;
//			Time.timeScale = 1;
//			PlayerPrefs.DeleteKey("CheckPointReached");
//			PlayerPrefs.DeleteKey("PlayerKey");
//			if (Application.loadedLevel < Application.levelCount)
//			{
//				Application.LoadLevel(Application.loadedLevel+1);
//			}
//			else
//			{
//				Application.LoadLevel("MainMenuScene");
//			}

			/*GUI.Box(new Rect(Screen.width*0.3f, Screen.height*0.2f, Screen.width*0.4f, Screen.height*0.5f),"Level Complete!");
			if (GUI.Button(new Rect(Screen.width*0.45f, Screen.height*0.23f, Screen.width*0.1f, Screen.height*0.1f ), new GUIContent("Next Level", "Go to next level") ) )
			{
				levelDone = false;
				Time.timeScale = 1;
				PlayerPrefs.DeleteKey("CheckPointReached");
				PlayerPrefs.DeleteKey("PlayerKey");
				if (Application.loadedLevel < Application.levelCount)
					Application.LoadLevel(Application.loadedLevel+1);
				else
					Application.LoadLevel("MainMenuScene");

			}
			if (GUI.Button(new Rect(Screen.width*0.45f, Screen.height*0.34f, Screen.width*0.1f, Screen.height*0.1f ), new GUIContent("Restart Level", "Restart the Level") ) )
			{
				levelDone = false;
				Time.timeScale = 1;
				PlayerPrefs.DeleteKey("CheckPointReached");
				PlayerPrefs.DeleteKey("PlayerKey");
				Application.LoadLevel(Application.loadedLevel);
			}
			if (GUI.Button(new Rect(Screen.width*0.45f, Screen.height*0.45f, Screen.width*0.1f, Screen.height*0.1f ), new GUIContent("Exit to Main Menu", "Go to the Main Menu") ) )
			{
				levelDone = false;
				Time.timeScale = 1;
				PlayerPrefs.DeleteKey("CheckPointReached");
				PlayerPrefs.DeleteKey("PlayerKey");
				Application.LoadLevel("MainMenuScene");
			}
			if (GUI.Button(new Rect(Screen.width*0.45f, Screen.height*0.56f, Screen.width*0.1f, Screen.height*0.1f ), new GUIContent("Level Select", "Go to the Level Selection screen") ) )
			{
				levelDone = false;
				Time.timeScale = 1;
				PlayerPrefs.DeleteKey("CheckPointReached");
				PlayerPrefs.DeleteKey("PlayerKey");
				Application.LoadLevel("LevelSelectScene");
			}
			GameObject.Find("PlayerAvatar").GetComponent<PlayerMovement>().mouseOverGUI = true;*/
		//}
	}
}

