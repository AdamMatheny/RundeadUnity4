using UnityEngine;
using System.Collections;

public class CircuitJoint : MonoBehaviour 
{
	public bool SnapToSurface = true;
	public int ID = -1; //Id that is used for sorting the path

	/* Used for keeping the circuit paths in their intended order by
	 * extracting the number value from their name and storing it
	 * removing issue with alphabetical sort
	 */
	public void Identify()
	{
		if(ID == -1)
		{
			int index = name.IndexOf("Joint");
			int.TryParse(name.Substring(index+5),out ID);
		}
	}

}
