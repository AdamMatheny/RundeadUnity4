using UnityEngine;
using System.Collections;
using UnityEditor;
using Assets.CustomScripts;

[CustomEditor(typeof(SwitchToggler))]
public class SwitchEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		SwitchToggler myScript = (SwitchToggler)target;
		if (GUILayout.Button("Add Path"))
		{
			if(myScript.Paths.Count < myScript.LinkedObjects.Length)
			{
				myScript.AddPath();
			}

		}
		//SwitchToggler myScript = (SwitchToggler)target;
		//myScript.Type = (Assets.CustomScripts.SwitchType)EditorGUILayout.EnumPopup("Type", (System.Enum)myScript.Type);
		//switch(myScript.Type)
		//{
		//	case SwitchType.Toggle:
		//		ToggleSelected();
		//		break;
		//	case SwitchType.Timed:
		//		TimedSelected();
		//		break;
		//	case SwitchType.Pressure:
		//		PressureSelected();
		//		break;
		//	default:
		//		Debug.Log("Error: Switch CustomEditor has more selected than needed");
		//		break;
		//}
	}

	private void ToggleSelected()
	{
	}
	private void TimedSelected()
	{
	}
	private void PressureSelected()
	{
	}
}