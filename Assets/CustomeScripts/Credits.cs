using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour 
{
    private float mOffset;
    public float mSpeed = 9.0f;
    public GUIStyle mStyle;
    public Rect mViewArea;
    public TextAsset mCreditsText;
	// Use this for initialization
	void Start () 
    {
        mViewArea = new Rect(0, 0, Screen.width, Screen.height);
        mOffset = this.mViewArea.height;
	}
	
	// Update is called once per frame
	void Update () 
    {
        mViewArea = new Rect(0, 0, Screen.width, Screen.height*2f);

        //scrolls text upward based time step
        mOffset -= Time.deltaTime * this.mSpeed;
	}

    private void OnGUI()
    {
        GUI.BeginGroup(this.mViewArea);

        Rect position = new Rect(0, mOffset, this.mViewArea.width, this.mViewArea.height*2f);

        
        GUI.Label(position, mCreditsText.text, this.mStyle);
        
 
        GUI.EndGroup();
    }
    
}
