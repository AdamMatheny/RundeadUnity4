using UnityEngine;
using System.Collections;

//public class TextBoxSummoner : MonoBehaviour {

//    //bool telling whether to display the message
//    private bool mDisplayMessage = false;
//    //bool telling whether the message has already been displayed
//    public bool mDisplayedMessageAlready = false;
//    //Id of this TextBox
//    public string mTextBoxId;
//    //bool telling whether to stop the player while message is playing 
//    public bool mStopPlayer = false;
//    //bool telling whether an audioclip should play once or not
//    public bool mPlayOnce = true;
//    //bool telling whether an audiclip should play more than once in a level or not
//    public bool mPlayMultipleInALevel = false;
//    public GUIStyle mStyle;
//    //Array of Messages to Display
//    [SerializeField]
//    private string[] mMessages;
//    //Array of Audio to Play
//    [SerializeField]
//    private AudioClip[] mAudioClips;
//    //the message to be displayed
//    [SerializeField]
//    private string mMessage;
//    //the width of the message
//    [SerializeField]
//    private int mWidthPercent = 80;
//    //the height of the message
//    [SerializeField]
//    private int mHeightPercent = 10;
//    //Use this for initialization
//    AudioManager mAudioManager;
//    //This counter tells us where we are in the Audio and Messages arrays
//    private int mIndex;
//    //bool declaring if we should zoom in when the Audio and Messages are playing
//    public bool mZoom = false;
//    //bool declaring if an textboxsummoner is waiting in line or not
//    public bool mWaiting = false;

//    private float audioStartTime;
//    private float audioRunTime;


//    void Start () 
//    {
//        mAudioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
//        if (mAudioManager)
//        {
//            int index = 0;
//            foreach(AudioClip clip in mAudioClips)
//            {
//                string title = mTextBoxId;
//                title = title + index;
//                mAudioManager.AddAudioClip(title, clip);
//                index++;
//            }
//        }
//        mIndex = 0;
//        Assets.CustomeScripts.InformationManager.SetCurrentLevel(Application.loadedLevelName);
//        Assets.CustomeScripts.InformationManager.AddTextBoxSummoner(mTextBoxId);
//    }
	
//    void Awake()
//    {
//        mDisplayedMessageAlready = Assets.CustomeScripts.InformationManager.RetrieveTextBoxSummonerState(mTextBoxId);
//    }

//    // Update is called once per frame
//    void Update () 
//    {
//        if ((mDisplayMessage == true) && (mDisplayedMessageAlready == false))
//        {
//            decreaseTimeRemaining();
//        }
//        if (mWaiting && !mAudioManager.mIsPlaying)
//        {
//            mDisplayMessage = true;
//            mMessage = mMessages[mIndex];
//            string title = mTextBoxId;
//            title = title + mIndex;
//            mAudioManager.PlayAudioClip(title);
//            mAudioManager.gameObject.transform.position = this.gameObject.transform.position;
//            audioStartTime = Time.time;
//            audioRunTime = mAudioClips[mIndex].length;
//            mWaiting = false;
//        }
//    }
//    // On collision with player, set displaymessage to true
//    void OnTriggerEnter(Collider other)
//    {
//        if ((other.tag == "Player") && (!mDisplayedMessageAlready))
//        {
//            if (!mAudioManager.mIsPlaying)
//            {
//                mDisplayMessage = true;
//                mMessage = mMessages[mIndex];
//                string title = mTextBoxId;
//                title = title + mIndex;
//                mAudioManager.PlayAudioClip(title);
//                mAudioManager.gameObject.transform.position = this.gameObject.transform.position;
//                audioStartTime = Time.time;
//                audioRunTime = mAudioClips[mIndex].length;
//                if (mStopPlayer)
//                {
//                    other.gameObject.GetComponent<Stunable>().IsStunned = true;
//                    other.gameObject.GetComponent<Animator>().SetFloat("Speed", 0.0f);
//                }
//                if (mZoom)
//                {
//                    GetComponent<ZoomArea>().enabled = true;
//                }
//            }
//            else if (!mWaiting)
//            {
//                mWaiting = true;
//            }
//        }
        
        
//    }
//    // OnGui for displaying the message to the screen
//    void OnGUI()
//    {
//        if ((mDisplayMessage == true) && (mDisplayedMessageAlready == false))
//        {
//            GUI.Box(new Rect(Screen.width / 50, Screen.height-Screen.height*0.01f*mHeightPercent, Screen.width*0.01f*mWidthPercent, Screen.height*0.01f*mHeightPercent), mMessage, mStyle);
//        }
//    }
//    // Decrease the timer allowing the message to display on screen
//    void decreaseTimeRemaining()
//    {
 
//        if (Time.time > audioStartTime + audioRunTime) 
//        {
//            mIndex++;
//            if (mIndex < mAudioClips.Length)
//            {
//                mMessage = mMessages[mIndex];
//                string title = mTextBoxId;
//                title = title + mIndex;
//                mAudioManager.PlayAudioClip(title);
//                audioStartTime = Time.time;
//                audioRunTime = mAudioClips[mIndex].length;
//            }
//            else
//            {
//                mDisplayedMessageAlready = true;
//                if (mZoom)
//                {
//                    Camera.main.GetComponent<TopDownCamera>().ZoomOut();
//                    mZoom = false;
//                    GetComponent<ZoomArea>().enabled = false;
//                }
//                PlayerMovement player = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;
//                if (player)
//                {
//                    player.gameObject.GetComponent<Stunable>().IsStunned = false;
//                }
//                if (mPlayOnce)
//                {
//                    Assets.CustomeScripts.InformationManager.UpdateTextBoxSummonerState(mTextBoxId);
//                }
//            }

//        }
//    }
//}
public class TextBoxSummoner : MonoBehaviour
{

    //bool telling whether to display the message
    private bool mDisplayMessage = false;
    //bool telling whether the message has already been displayed
    public bool mDisplayedMessageAlready = false;
    //Id of this TextBox
    public string mTextBoxId;
    //bool telling whether to stop the player while message is playing 
    public bool mStopPlayer = false;
    //bool telling whether an audioclip should play once or not
    public bool mPlayOnce = true;
    //bool telling whether an audiclip should play more than once in a level or not
    public bool mPlayMultipleInALevel = false;
    public GUIStyle mStyle;
    //Array of Messages to Display
    [SerializeField]
    private string[] mMessages;
    //Array of Audio to Play
    [SerializeField]
    private AudioClip[] mAudioClips;
    [SerializeField]
    private Texture[] mSpeakers;
    //the message to be displayed
    [SerializeField]
    private string mMessage;
    [SerializeField]
    private Texture mSpeaker;
    //the width of the message
    [SerializeField]
    private int mWidthPercent = 80;
    //the height of the message
    [SerializeField]
    private int mHeightPercent = 10;
    //Use this for initialization
    AudioManager mAudioManager;
    //This counter tells us where we are in the Audio and Messages arrays
    private int mIndex;
    //bool declaring if we should zoom in when the Audio and Messages are playing
    public bool mZoom = false;

    public Texture2D mMovieBarTexture;
    public PlayerMovement mPlayer;
    public PauseButton mPauseButton;
    public LevelHUD mLevelHUD;

    private float audioStartTime;
    private float audioRunTime;


    void Start()
    {
        mAudioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
        mLevelHUD = FindObjectOfType(typeof(LevelHUD)) as LevelHUD;
        if (mAudioManager)
        {
            int index = 0;
            foreach (AudioClip clip in mAudioClips)
            {
                string title = mTextBoxId;
                title = title + index;
                mAudioManager.AddAudioClip(title, clip);
                index++;
            }
        }
        mIndex = 0;
        Assets.CustomeScripts.InformationManager.SetCurrentLevel(Application.loadedLevelName);
        Assets.CustomeScripts.InformationManager.AddTextBoxSummoner(mTextBoxId);
    }

    void Awake()
    {
        mDisplayedMessageAlready = Assets.CustomeScripts.InformationManager.RetrieveTextBoxSummonerState(mTextBoxId);
        if (mDisplayedMessageAlready)
        {
            collider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((mDisplayMessage == true) && (mDisplayedMessageAlready == false))
        {
            decreaseTimeRemaining();
        }
    }
    // On collision with player, set displaymessage to true
    void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") && mDisplayedMessageAlready == false)
        {
            mPlayer = other.gameObject.GetComponent<PlayerMovement>();
            mPauseButton = other.gameObject.GetComponent<PauseButton>();
            mDisplayMessage = true;
            mMessage = mMessages[mIndex];
            if (mSpeakers.Length > 0)
            {
                mSpeaker = mSpeakers[mIndex];
            }
            string title = mTextBoxId;
            title = title + mIndex;
            mAudioManager.PlayAudioClip(title);
            mAudioManager.gameObject.transform.position = this.gameObject.transform.position;
            audioStartTime = Time.time;
            audioRunTime = mAudioClips[mIndex].length;
            if (mStopPlayer)
            {
                other.gameObject.GetComponent<Stunable>().IsStunned = true;
                other.gameObject.GetComponent<Animator>().SetFloat("Speed", 0.0f);
            }
            if (mZoom)
            {
                GetComponent<ZoomArea>().enabled = true;
                if (mPlayer)
                {
                    mPlayer.mShowGUI = false;
					FindObjectOfType<ClickToMovePointer>().Hide();
                }
                if (mPauseButton)
                {
                    mPauseButton.mShowGUI = false;
                }
                if (mLevelHUD)
                {
                    mLevelHUD.mShowHUD = false;
                }
            }
            else
            {
                collider.enabled = false;
            }
            
        }
        else if (mDisplayedMessageAlready)
        {
            Camera.main.GetComponent<TopDownCamera>().ZoomOut();
            mZoom = false;
            GetComponent<ZoomArea>().enabled = false;
            mLevelHUD.mShowHUD = true;
            mPlayer.mShowGUI = true;
            mPauseButton.mShowGUI = true;
        }

    }
    // OnGui for displaying the message to the screen
    void OnGUI()
    {
        //if ((mDisplayMessage == true) && (mDisplayedMessageAlready == false))
        //{
        //    GUI.Box(new Rect(Screen.width / 50, Screen.height - Screen.height * 0.01f * mHeightPercent, Screen.width * 0.01f * mWidthPercent, Screen.height * 0.01f * mHeightPercent), mMessage, mStyle);
        //}

        //if (mZoom && mDisplayMessage && !mDisplayedMessageAlready)
        //{
        //    GUI.DrawTexture(new Rect(0, 0, Screen.width, 40), mMovieBarTexture);
        //    GUI.DrawTexture(new Rect(0, Screen.height - 40, Screen.width, Screen.height), mMovieBarTexture);
        //}

        if (mDisplayMessage && !mDisplayedMessageAlready)
        {
            float zoomBarHeight = (Screen.height * 0.01f * (mHeightPercent * 0.50f));
			mStyle.fontSize = Mathf.Min(Screen.width, Screen.height) / 20;
			mStyle.padding.top = (int)(mStyle.fontSize*.9);
			mStyle.padding.left = (int)(mStyle.fontSize *2);
			mStyle.padding.right = (int)(mStyle.fontSize *2);


            if (mZoom)
            {
				GUI.Box(new Rect(Screen.height * 0.01f * mHeightPercent, Screen.height - (Screen.height * 0.01f * mHeightPercent)-zoomBarHeight, Screen.width * 0.01f*mWidthPercent, (Screen.height * 0.01f * mHeightPercent)), mMessage, mStyle);
                GUI.DrawTexture(new Rect(0, (Screen.height - (Screen.height * 0.01f * mHeightPercent) - zoomBarHeight), Screen.width * 0.15f, Screen.height * 0.01f * mHeightPercent), mSpeaker);
                GUI.DrawTexture(new Rect(0, 0, Screen.width, zoomBarHeight), mMovieBarTexture);
                GUI.DrawTexture(new Rect(0, Screen.height - zoomBarHeight, Screen.width, zoomBarHeight), mMovieBarTexture);
            }
            else
            {
				GUI.Box(new Rect(Screen.height * 0.01f * mHeightPercent, Screen.height - (Screen.height * 0.01f * mHeightPercent), Screen.width * 0.01f*mWidthPercent, (Screen.height * 0.01f * mHeightPercent)), mMessage, mStyle);
                GUI.DrawTexture(new Rect(0, Screen.height - (Screen.height * 0.01f * mHeightPercent), Screen.width * 0.15f, (Screen.height * 0.01f * mHeightPercent)), mSpeaker);
            }
        }
    }
    // Decrease the timer allowing the message to display on screen
    void decreaseTimeRemaining()
    {

        if (Time.time > audioStartTime + audioRunTime)
        {
            mIndex++;
            if (mIndex < mAudioClips.Length)
            {
                mMessage = mMessages[mIndex];
                if (mSpeakers.Length > 0)
                {
                    mSpeaker = mSpeakers[mIndex];
                }
                string title = mTextBoxId;
                title = title + mIndex;
                mAudioManager.PlayAudioClip(title);
                audioStartTime = Time.time;
                audioRunTime = mAudioClips[mIndex].length;
            }
            else
            {
                mDisplayedMessageAlready = true;
                if (mZoom)
                {
                    Camera.main.GetComponent<TopDownCamera>().ZoomOut();
                    mZoom = false;
                    GetComponent<ZoomArea>().enabled = false;
                    mPlayer.mShowGUI = true;
                    mPauseButton.mShowGUI = true;
                    mLevelHUD.mShowHUD = true;
                    collider.enabled = false;
                }
                else
                {
                    mPlayer.gameObject.GetComponent<Stunable>().IsStunned = false;
                }
                if (mPlayOnce)
                {
                    Assets.CustomeScripts.InformationManager.UpdateTextBoxSummonerState(mTextBoxId);
                }
            }

        }
    }
}

