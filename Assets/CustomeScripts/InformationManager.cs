using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
namespace Assets.CustomeScripts
{
    static class InformationManager
    {
        static Dictionary<string, bool> sAudioCues = new Dictionary<string, bool>();
        static string sCurrentLevel = "";
        static string sPreviousLevel = "";


        public static void AddTextBoxSummoner(string title)
        {
            if (!sAudioCues.ContainsKey(title))
            {
                sAudioCues.Add(title, false);
            }
        }

        public static void UpdateTextBoxSummonerState(string title)
        {
            if (sAudioCues.ContainsKey(title))
            {
                sAudioCues[title] = true;
            }
        }

        public static bool RetrieveTextBoxSummonerState(string title)
        {
            bool returnValue = false;
            if (sAudioCues.ContainsKey(title))
            {
                returnValue = sAudioCues[title];
            }
           
            return returnValue;
        }

        public static void SetCurrentLevel(string level)
        {
            sPreviousLevel = sCurrentLevel;
            sCurrentLevel = level;

            if (sCurrentLevel != sPreviousLevel)
            {
                sAudioCues.Clear();
            }
        }
    }
}

