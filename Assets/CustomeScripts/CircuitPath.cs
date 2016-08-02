using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;

/*
 * CircuitPath
 * Visual add to indicate to the player the connect between switches 
 * and the object they interact with as well as remaining timer if it's needed
 * @verison 9/2/14 
 * @author Kyle
 */
public class SortCircuits : IComparer<CircuitJoint>
{
	public int Compare(CircuitJoint x, CircuitJoint y)
	{
		return x.ID.CompareTo(y.ID);
	}
}
public class CircuitPath : MonoBehaviour 
{
	/* Visible Variables */

	public GameObject theTarget;									//The destination object being affected by the switch
	public Color drainedColor = Color.red;
	public Color filledColor  = Color.green;

	private List<CircuitJoint> thePath = new List<CircuitJoint>();	//Placeholders allowing us to bend and affect the circuit's path
	private LineRenderer theLine;									//The actual path the player sees
	[HideInInspector] public bool bIsTimed = false;					//Whether or not this circuit needs to 'drain' of power over time
	[HideInInspector] public float mTimerLength = 0;				//Length of the timer
	[HideInInspector] public float mTimeRemaining = 0f;
	[HideInInspector] public bool bTimerRunning = false;
	private Color startColor;
	private Color endColor;
	
	void Start()
	{
		theLine = GetComponent<LineRenderer>();
		bIsTimed = transform.parent.GetComponent<SwitchToggler>().GetTimedSwitch(out mTimerLength);
		startColor = drainedColor;
		endColor = drainedColor;
		
		for (int i = 0; i < gameObject.transform.childCount; i++ )
		{
			thePath.Add(transform.GetChild(i).GetComponent<CircuitJoint>());
		}

		//Go through and assign each joint that proper ID
		foreach (CircuitJoint joint in thePath)
		{
			joint.Identify();
		}

		IComparer<CircuitJoint> sortMethod = new SortCircuits();
		thePath.Sort(sortMethod);
		SnapToSurfaces();

		//Generate the path
		if(thePath.Count > 0)
		{
			CreatePath();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (thePath.Count > 0)
		{
			UpdateColors();
		}
	}
	//Generates the circuit path
	private void CreatePath()
	{
		theLine.material = (Material)Resources.Load("Material/CircuitMat", typeof(Material));

		//Connect the dots between the joints of the path
		theLine.SetVertexCount(thePath.Count + 2);

		//The first and last points are known from the beginning
		Vector3 pathPoint = transform.position;
		
		pathPoint.y = thePath[0].transform.position.y;
		theLine.SetPosition(0, pathPoint);
		theLine.SetPosition(thePath.Count + 1, theTarget.transform.position);

		//Position every joint in the path
		for (int i = 1; i <= thePath.Count; i++)
		{
			theLine.SetPosition(i, thePath[i - 1].transform.position);
		}

		//Assign material and width
		theLine.SetColors(startColor, startColor);
		theLine.SetWidth(0.2f, 0.2f);
	}
	//Handles making adjusting the colors of the path as needed
	private void UpdateColors()
	{
		SwitchToggler origin = transform.parent.GetComponent<SwitchToggler>();
		if (!bIsTimed)					//If the door relies on a timer
		{			
			if(origin.IsActive)
			{
				startColor = filledColor;
				endColor = filledColor;
			}
			else
			{
				startColor = drainedColor;
				endColor = drainedColor;
			}
		}
		else
		{
			if (bTimerRunning)		//While the door timer is counting down, slowly bleed out the colors
			{
				mTimeRemaining = origin.GetTimeRemaining();
				startColor = drainedColor;
				endColor = filledColor;
				BlendColors(ref startColor, ref endColor, mTimeRemaining);
			}
			else
			{
				if(origin.IsActive)
				{
					startColor = filledColor;
					endColor = filledColor;
				}
				else
				{
					startColor = drainedColor;
					endColor = drainedColor;
				}
			}	
		}

		theLine.SetColors(startColor, endColor);
	}

	private void BlendColors(ref Color origin, ref Color destination, float elapsedTime)
	{
		//Get blend Amount
		if (elapsedTime > mTimerLength)
		{
			elapsedTime = mTimerLength;
		}
		float blendAmount = elapsedTime / mTimerLength;

		destination.r *= blendAmount;
		destination.g *= blendAmount;
		destination.b *= blendAmount;

		origin.r *= (1 - blendAmount);
		origin.g *= (1 - blendAmount);
		origin.b *= (1 - blendAmount);
	}

	private void SnapToSurfaces()
	{
		RaycastHit jointHit;
		foreach(CircuitJoint joint in thePath)
		{
			if(joint.GetComponent<MeshRenderer>() != null)
			{
				if (joint.GetComponent<CircuitJoint>().SnapToSurface)
				{
					if (Physics.Raycast(joint.transform.position, -joint.transform.forward, out jointHit, 5f))
					{
						joint.transform.position = jointHit.point;
					}
				}

				joint.GetComponent<MeshRenderer>().enabled = false;
			}
		}
	}

	public void SpawnJoint()
	{
#if UNITY_EDITOR
		CircuitJoint newJoint;

		//Starting rotation of the joint

		newJoint = (CircuitJoint)PrefabUtility.InstantiatePrefab(Resources.Load<CircuitJoint>("Prefabs/CircuitJoint"));
		if(thePath.Count == 0)
		{
			newJoint.transform.position = transform.position;
		}
		else
		{
			newJoint.transform.position = thePath[thePath.Count - 1].transform.position;
		}
		newJoint.transform.rotation = Quaternion.Euler(-90, 0, 0);
		newJoint.transform.parent = transform;
		newJoint.name = gameObject.name + "Joint" + gameObject.transform.childCount.ToString();
		
#endif
	}
}
