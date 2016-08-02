/*
 * Metrics
 * Static class that will handle our data tracking needs
 * Authors: Sean Lambdin, Kyle Weeks
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

/* First week testing goals
 * Play Time - Accomplished through timers dictionairy
 * Total number of deaths - Accomplished by summing all zombie id's in mData
 * Which zombies killed you - Accomplished by walking mData for distinct zombies
 * Total number of doors opened - Accomplished by summing all door id's in mData
 * Number of time you opened each door - Accomplished by walking mData for distinct doors
*/

namespace Assets.CustomeScripts
{
	static class Metrics
	{
		
		static string metricPath = Directory.GetCurrentDirectory() + "\\..\\Documents\\MetricsLog";		// Main file path we are using to store metrics
		static Dictionary<string, Stopwatch> mTimers = new Dictionary<string, Stopwatch>();				//Assortment of timers to using in tracking metrics
        static Dictionary<string, int> mData = new Dictionary<string, int>();							//Generic pairing of identifiers and number of times they are interacted with
		static Dictionary<string, int> mRestarts = new Dictionary<string, int>();						//Pairing level names to number of times the level was started 

        /*
         * AddMetric
         * Adds a metric to the list of metrics to be printed to file
         * @param - the descriptive title
         * @param - the data which will be the key into the mData dictionary
         */
        private static void AddMetric(string title, string data)
        {
			string dataString = data;// +mData[data].ToString();

			if (mData.ContainsKey(dataString))
            {
				mData[dataString]++;
            }
            else
            {
				mData.Add(dataString, 1);
            }
        }
		/*
		 * FormattedDateTime
		 * Removes all unnecessary or harmful characters from the current Date and time
		 * @return compressed string of date time ready for file naming
		 */
		private static string FormattedDateTime()
		{
			string time = DateTime.Now.ToShortDateString() + '-' + DateTime.Now.ToShortTimeString();

			time = time.Replace(' ', '-');
			time = time.Replace(':', '-');
			time = time.Replace('/', '-');

			return time;
		} 
		/*
         * PrintMetrics 
         * Prints all the metrics to the newly created file, and then clears the containers 
         */
		private static void PrintMetrics()
        {
			//Ensure the file path is ready
            if (!Directory.Exists(metricPath))
            {
                Directory.CreateDirectory(metricPath);
            }
			
			//Generate specific file name
			string path = metricPath;
			string fileID = "Metrics";
			
			path += "\\" + fileID + "-" + FormattedDateTime() + ".txt";

			//Open or create file and fill it in
            using (System.IO.StreamWriter file = System.IO.File.CreateText(path))
            {
				//Number of times a level was tried
				file.WriteLine("[Levels Attempted: \t Times tried]");
				foreach(var level in mRestarts)
				{
					file.WriteLine(level.Key + "\t" + level.Value);
				}
				//Report all the timers that we had running
				file.WriteLine("[Time on each Level: \thh:mm:ss:ms]");
				foreach(var timer in mTimers)
				{
					file.WriteLine(timer.Key + "\t" + timer.Value.Elapsed);
				}
				//All the monsters that killed the player
				file.WriteLine("[Monster ID: \tTimes killed player]");
				foreach (var metric in mData)
				{
					file.WriteLine(metric.Key + "\t" + metric.Value);
				}

                file.Close();
            }
        }
		/*
		 * ClearContainers
		 * Incase we want to dump metrics at any time / organizing code
		 */
		private static void ClearContainers()
		{
            mData.Clear();
            mRestarts.Clear();
            mTimers.Clear();
		}
		/*
		 * StartTimer
		 * Given a timer name either create or use existing timer
		 * @param name - name of the timer to be used
		 */
		private static void StartLevelTimer(string name)
		{
			if(!mTimers.ContainsKey(name))
			{
				mTimers.Add(name, new Stopwatch());
			}

			mTimers[name].Start();
			
		}
		/*
		 * PauseTimer
		 * Given a timer name either pause or resume the timer if it exists
		 * @param name - name of the timer to be used
		 * @return whether it was successful in pausing the timer
		 */
		private static bool PauseLevelTimer(string name)
		{
			bool wasSuccesful = false;
			if(mTimers.ContainsKey(name))
			{
				if(mTimers[name].IsRunning)
				{
					mTimers[name].Stop();
				}
				else
				{
					mTimers[name].Start();
				}
				wasSuccesful = true;
			}
			return wasSuccesful;
		}
		/*
		 * StopTimer
		 * Given a timer name stop it if it exists
		 * @param name - name of the timer to be used
		 * @return whether it was successful in stopping the timer
		 */
		private static bool StopLevelTimer(string name)
		{
			bool wasSuccesful = false;
			if (mTimers.ContainsKey(name))
			{
				mTimers[name].Stop();
				wasSuccesful = true;
			}
			return wasSuccesful;
		}
		/*
		 * StopAllTimers
		 * Iterate and stop all running timers
		 */
		private static void StopAllTimers()
		{
			foreach(var timer in mTimers)
			{
				timer.Value.Stop();
			}
		}
		private static void StartLevel(string name)
		{
			if (mRestarts.ContainsKey(name))
			{
				mRestarts[name]++;
			}
			else
			{
				mRestarts.Add(name, 1);
			}
		}
	}
}
