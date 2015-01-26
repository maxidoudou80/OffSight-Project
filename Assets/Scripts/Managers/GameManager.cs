using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static GameManager _gameManager;
    private CameraManager camManager;
    private float lastPlayerDetectionCheck;
    private float lastVisibleTime;
    public bool visible { get; set; }

    public enum GameState { StartMenu, Playing, Pause, Death, Win, Loading, CamSelection };
    public GameState gameState { get; set; }
    public int currentGameState;
    public int playerAppearance;
    public float timeUnseenBeforeDeath;
    public float detectionCheckDelay;

    public static GameManager GetInstance()
    {
        return _gameManager;
    }

    public PlayerBehaviour player { get; set; }

	// Use this for initialization
	void Awake () {
        _gameManager = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
	}

    void Start()
    {
        Screen.showCursor = false;      //HIDE CURSOR
        camManager = CameraManager.GetInstance();
        gameState = GameState.Playing;
        lastVisibleTime = Time.time;
        lastPlayerDetectionCheck = 0;
    }
	
	// Update is called once per frame
	void Update () {
        switch(gameState)
        {
            case GameState.Playing:

                if(lastPlayerDetectionCheck + detectionCheckDelay < Time.time)
                {
                    //UpdateVisibleStatus();
                }
                break;
        }
	}

    #region Player Visibility
    void UpdateVisibleStatus()
    {
        switch(visible)
        {
            case true :
                if(!IsPlayerVisible())
                {
                    lastVisibleTime = Time.time;
                    visible = false;

                    Debug.Log("Starting delay to death, you have " + timeUnseenBeforeDeath + " seconds");
                }
                break;

            case false :
                if(IsPlayerVisible())
                {
                    visible = true;
                }
                else if (lastVisibleTime + timeUnseenBeforeDeath < Time.time)
                {
                    //Debug.Log("Player Dead");
                }

                //Debug.Log("Remaining : " + (lastVisibleTime + timeUnseenBeforeDeath - Time.time));

                break;
        }
    }

    bool IsPlayerVisible()
    {
        if (CheckPlayerInCamera(camManager.GetCurrentMain().transform.position))
            return true;

        foreach(int camId in camManager.currentLinkedCameras)
        {
            if(CheckPlayerInCamera(camManager.LevelCameras[camId].transform.position))
                return true;
        }

        return false;
    }


    bool CheckPlayerInCamera(Vector3 camPosition)
    {
        RaycastHit hit;
        foreach (Transform detectionPoint in player.detectionPoints)
        {
            //Debug.DrawRay(camPosition, detectionPoint.position - camPosition);
            if (Physics.Raycast(camPosition, detectionPoint.position - camPosition, out hit))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion

    public void CallLoadLevel(string level)
    {
        Application.LoadLevel(level);
    }
}