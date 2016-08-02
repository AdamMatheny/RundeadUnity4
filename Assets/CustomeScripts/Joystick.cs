using UnityEngine;
using System.Collections;

public class Joystick : MonoBehaviour 
{
    public static Vector2 mVector;
    public static Vector2 mNormals;
    public static bool mDoubleTapped;
    public Color mActiveColor;
    public Color mInactiveColor;
    public Texture2D mJoystickTexture;
    public Texture2D mJoystickBackground;

    private GameObject mBackgroundObject;
    private GUITexture mJoystickGUI;
    private GUITexture mBackgroundGUI;
    private Vector2 mOrigin;
    private Vector2 mPosition;
    private int mSize;
    private float mLength;
    private bool mGotPosition;
    private int mFingerID;
    private int mLastID;
    private float mTapTimer;
    private bool mEnabled;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (PlayerMovement.useJoystick)
		{
			mEnabled = true;
		}
		else
		{
			mEnabled = false;
		}
	    if (mTapTimer > 0) 
        {
            mTapTimer -= Time.deltaTime;
        }
        if (!Input.GetMouseButton(0)) 
        {
            ResetJoystick();
        }
        if (mEnabled == true) 
        {
            if (Input.GetMouseButton(0) && !mGotPosition) 
            {
                GetPosition(); 
                GetConstraints();
            }
            if (Input.GetMouseButton(0) && mGotPosition == true) 
            {
				mPosition = Input.mousePosition;
				mPosition = GetRadius(mOrigin,mPosition,mLength);
				mVector = GetControls(mPosition,mOrigin);
				mNormals = new Vector2(mVector.x/mLength,mVector.y/mLength);
				if (Input.mousePosition.x > ((Screen.width) + mBackgroundGUI.pixelInset.width)
				    || Input.mousePosition.y > ((Screen.height) + mBackgroundGUI.pixelInset.height))
				{
					ResetJoystick();
				}
            }
            if (!mGotPosition && !(Input.GetMouseButton(0)) && (mTapTimer <= 0)) 
            {
                if (mBackgroundGUI.color != mInactiveColor) 
                {
                    mBackgroundGUI.color = mInactiveColor;
                }
            }
            mBackgroundGUI.pixelInset = new Rect(mOrigin.x-(mBackgroundGUI.pixelInset.width/2), mOrigin.y-(mBackgroundGUI.pixelInset.height/2), mSize/5, mSize/5);
            mJoystickGUI.pixelInset = new Rect(mPosition.x-(mJoystickGUI.pixelInset.width/2), mPosition.y-(mJoystickGUI.pixelInset.height/2), mSize/11, mSize/11);
			
        } 
        else if (mBackgroundGUI.pixelInset.width > 0) 
        {
            mBackgroundGUI.pixelInset = new Rect(0,0,0,0); 
            mJoystickGUI.pixelInset = new Rect(0,0,0,0);
			

        }

	}//End of Update()

    public void ToggleJoystickEnabled()
    {
        mEnabled = !mEnabled;
        ResetJoystick();
    }

    private void ResetJoystick()
    {
        //mVector = new Vector2(0, 0);
        //mNormals = mVector;
        //mLastID = mFingerID;
        //mFingerID = -1;
        //mTapTimer = 0.150f;
        mJoystickGUI.color = mInactiveColor;
        mPosition = mOrigin;
        //mGotPosition = false;
    }

    private Vector2 GetRadius(Vector2 midPoint, Vector2 endPoint, float maxDistance)
    {
        Vector2 distance = endPoint;
        if (Vector2.Distance(midPoint, endPoint) > maxDistance)
        {
            distance = endPoint - midPoint;
            distance.Normalize();
            return (distance * maxDistance) + midPoint;
        }
        return distance;
    }

    private void GetPosition()
    {
       // mFingerID = touch.fingerId;
        if (Input.GetMouseButton(0))
		{
			if (Input.mousePosition.x < Screen.width/3  &&
			    (Input.mousePosition.y < Screen.height/3))//  &&
//				(Input.GetTouch(mFingerID).phase == TouchPhase.Began))
			{
				mPosition = Input.mousePosition;
				mOrigin = mPosition;
				mJoystickGUI.texture = mJoystickTexture;
				mJoystickGUI.color = mActiveColor;
				mBackgroundGUI.texture = mJoystickBackground;
				mBackgroundGUI.color = mActiveColor;

//				if (mFingerID == mLastID && (mTapTimer > 0))
//				{
//					mDoubleTapped = true;
//				}

				mGotPosition = true;
			}
		}
    }//End of GetPosition()

    private void GetConstraints()
    {
        if (mOrigin.x < ((mBackgroundGUI.pixelInset.width / 2) + 25)) 
        { 
            mOrigin.x = ((mBackgroundGUI.pixelInset.width / 2) + 25); 
        }
        if (mOrigin.y < ((mBackgroundGUI.pixelInset.height / 2) + 25)) 
        {
            mOrigin.y = ((mBackgroundGUI.pixelInset.height / 2) + 25);
        }
        if (mOrigin.x > (Screen.width / 3)) 
        { 
            mOrigin.x = (Screen.width / 3); 
        }
        if (mOrigin.y > (Screen.height / 3)) 
        { 
            mOrigin.y = (Screen.height / 3); 
        }
    }//End of GetConstraints

    private Vector2 GetControls(Vector2 position, Vector2 origin)
    {
        Vector2 controlsVector = new Vector2();
        if (Vector2.Distance(position, origin) > 0) 
        { 
            controlsVector = new Vector2(position.x - origin.x, position.y - origin.y); 
        }
        return controlsVector;
    }

    void Awake()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        gameObject.transform.position = new Vector3(0, 0, 999);
        if (Screen.width > Screen.height) 
        { 
            mSize = Screen.height; 
        } 
        else 
        { 
            mSize = Screen.width; 
        } 
        mVector = new Vector2(0, 0);
        mJoystickGUI = gameObject.AddComponent("GUITexture") as GUITexture;
        mJoystickGUI.texture = mJoystickTexture; 
        mJoystickGUI.color = mInactiveColor;
        mBackgroundObject = new GameObject("VJR-Joystick Back");
        mBackgroundObject.transform.localScale = new Vector3(0, 0, 0);
        mBackgroundGUI = mBackgroundObject.AddComponent("GUITexture") as GUITexture;
        mBackgroundGUI.texture = mJoystickBackground; 
        mBackgroundGUI.color = mInactiveColor;
        mFingerID = -1; 
        mLastID = -1; 
        mDoubleTapped = false; 
        mTapTimer = 0; 
        mLength = 50;
        mPosition = new Vector2((Screen.width / 3) / 2, (Screen.height / 3) / 2); 
        mOrigin = mPosition;
        mGotPosition = false;
        ToggleJoystickEnabled(); 
        mEnabled = true;
    }//End of Awake()
}
