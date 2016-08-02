using UnityEngine;
using System;
using System.Collections;
using Assets.CustomeScripts;
/*
 * MetricsManager
 * Handles tracking player posiiton for checkpoints and eventual player analytics
 * Authors: Sean Lambdin, Kyle Weeks
 */
public class MetricsManager : MonoBehaviour 
{

	/*
	* OnApplicationQuit 
	* After the application is closed, prints the metrics to the file
	*/
	void OnApplicationQuit()
		{
		//Will transmit the metrics to the database 

		//Metrics.StopAllTimers();
		//Metrics.ClearContainers();
		PlayerPrefs.DeleteKey("CheckPointReached");
		PlayerPrefs.DeleteKey("PlayerKey");
		PlayerPrefs.DeleteKey("CompanionPresent");
		PlayerPrefs.DeleteKey("CheckPointName");
		PlayerPrefs.DeleteKey("DropOffLocation");
		PlayerPrefs.DeleteKey("CompanionX");
		PlayerPrefs.DeleteKey("CompanionY");
		PlayerPrefs.DeleteKey("CompanionZ");
	}
	/*
	* OnLevelWasLoaded
	* After a level is loaded, adds to the metrics which level we entered
	*/
	void OnLevelWasLoaded(int level)
	{
		if (PlayerPrefs.HasKey("LevelName"))
		{
			if(PlayerPrefs.GetString("LevelName") != Application.loadedLevelName)
			{
				PlayerPrefs.DeleteKey("CheckPointReached");
				PlayerPrefs.DeleteKey("PlayerKey");
				PlayerPrefs.DeleteKey("CompanionPresent");
				PlayerPrefs.DeleteKey("CheckPointName");
				PlayerPrefs.DeleteKey("DropOffLocation");
				PlayerPrefs.DeleteKey("CompanionX");
				PlayerPrefs.DeleteKey("CompanionY");
				PlayerPrefs.DeleteKey("CompanionZ");
				PlayerPrefs.SetString("LevelName", Application.loadedLevelName);
			}
		}
		else
		{
			PlayerPrefs.SetString("LevelName", Application.loadedLevelName);
		}
	}

	/*private void LogData()
	{
		WWWForm form = new WWWForm();
		form.AddField("tablename", "Kyle");
		WWW db = new WWW(mUrl, form);
		StartCoroutine(TestConnection(db));
	}

	IEnumerator TestConnection(WWW web)
	{

		yield return web;

		if(web.error != null)
		{
			Debug.Log("Testconnection error: " + web.error);
		}
		else
		{
			Debug.Log(web.text);
		}		
	}*/
}
