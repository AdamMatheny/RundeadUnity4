using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CircuitPath))]
public class CircuitPathEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		CircuitPath myScript = (CircuitPath)target;
		
	//EditorGUILayout.IntField("Number of Joints", target.thePath)

		if(GUILayout.Button("Add Joint"))
		{
			myScript.SpawnJoint();
		}
	}
}

