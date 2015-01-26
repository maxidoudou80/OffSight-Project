using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    private static InputManager _instance;

    public static InputManager GetInstance()
    {
        if (_instance == null)
        {
            GameObject managers = GameObject.FindGameObjectWithTag("Managers") as GameObject;

            if (managers.GetComponent<InputManager>() == null)
                managers.AddComponent<InputManager>();

            _instance = managers.GetComponent<InputManager>();

            Debug.Log("Creating IM : " + _instance);
        }

        return _instance;
    }

    #region Variables

    CameraManager camManager;
    Escargot escargot;
     
    //Player movement
    PlayerBehaviour player;

    //CONTROLS
    public enum Controls { Pad, Keyboard, Mouse_Keyboard}
    public Controls controls;

            // U-turn
    float newRot;
    float aimedRot;
    float rotToDo;
    bool doingFullRotation;
    float lastFullRotation;
    float stickDownStart;
    bool countingForU_Turn;

    public float minAxisToU_Turn;
    public float timeBeforeU_Turn;
    public float fullRotationCooldown;

    //Check if lockbutton is pressed is released without camera selection
    bool unlockCamera;

    //Stick Selection
    float rightStickH;
    float rightStickV;
    public float minMagnitudeSelection;
    public float rightStickReset;
    int previouslySelected;
        //Selection Reset
    bool checkingSelectionReset;
    float selectionResetCheckStart;
    public float selectionResetCountdown;

    #endregion

    // Use this for initialization
	void Start () {
        camManager = CameraManager.GetInstance();
        escargot = Escargot.GetInstance();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        lastFullRotation = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        switch(GameManager.GetInstance().gameState)
        {
            case GameManager.GameState.Playing:

                switch (controls)
                {
                    case Controls.Pad :
                        CheckPadMovement();
                    break;

                    //Chaining two "Case" works as the OR operator
                    case Controls.Keyboard :
                    case Controls.Mouse_Keyboard:
                        CheckKeyboardMovement();
                    break;
                }
                    
                if (doingFullRotation)
                    DoRotation();
                else
                {
                    CheckPadMovement();
                    CheckActions();


                    if (Input.GetButton("LockCamera"))
                    {
                        CheckCameraLock();
                    }
                    else
                    {
                        CheckViewportSelection();
                        CheckViewportSelectionPad();
                    }
                }
                
                //
                break;

            case GameManager.GameState.CamSelection:
                CheckActions();
                break;
        }
	}

    void CheckKeyboardMovement()
    {
        float moveH;
        float moveV;

        //Use Mouse or Keyboard to rotate
        if(controls == Controls.Mouse_Keyboard)
        {
            moveH = Input.GetAxis("Mouse X") * Time.deltaTime * player.maxRotationSpeed;

            if (Input.GetMouseButton(0))
                moveV = 1;
        }
        else
            moveH = Input.GetAxis("Horizontal") * Time.deltaTime * player.maxRotationSpeed;

        moveV = Input.GetAxis("Vertical");

        if(moveH > player.maxRotationSpeed)
        {
            moveH = player.maxRotationSpeed;
        }
        else if (moveH < player.minRotationSpeed && moveH > - player.minRotationSpeed)
        {
            moveH = 0;
        }


        if(moveV != 0)
        {
            if (moveV < 0)
                moveV *= player.backwardSpeed * Time.deltaTime;
            else if (moveV > 0)
            {
                if(Input.GetButton("Run"))
                {
                    player.running = true;
                    moveV *= player.runSpeed * Time.deltaTime;
                }
                else
                    moveV *= player.walkSpeed * Time.deltaTime;
            }
        }


        //Gestion de l'animation
        if(moveV != 0 || moveH != 0)
        {
            player.avatarAnimator.SetBool("Moving", true);
        }
        else
        {
            player.avatarAnimator.SetBool("Moving", false);
        }

        player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, player.transform.eulerAngles.y + moveH, player.transform.eulerAngles.z);
        player.transform.position += player.transform.forward * moveV;
    }

    

    
    void CheckPadMovement()
    {
        float rotate = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("UTurn"))
            CheckFullRotation();

        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > -minAxisToU_Turn && Input.GetAxis("Horizontal") < minAxisToU_Turn)
        {
            if (!doingFullRotation && countingForU_Turn && timeBeforeU_Turn + stickDownStart < Time.time)
            {
                countingForU_Turn = false;
                CheckFullRotation();
            }
            else if (!countingForU_Turn)
            {
                countingForU_Turn = true;
                stickDownStart = Time.time;
            }
        }
        else
        {
            countingForU_Turn = false;
        }

        if (rotate != 0)
        {
            player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x,
                                                        player.transform.eulerAngles.y + player.maxRotationSpeed * rotate * Time.deltaTime,
                                                        player.transform.eulerAngles.z);
        }

        if (Input.GetAxis("L") > 0)
        {
            float speed;

            if (Input.GetAxis("Run") > 1 || Input.GetKey(KeyCode.LeftShift))
            {
                player.running = true;
                speed = player.runSpeed;
            }
            else
            {
                player.running = false;
                speed = player.walkSpeed;
            }

            player.transform.position += player.transform.forward * speed * Time.deltaTime;
        }

    }

    void CheckFullRotation()
    {
        //Checking for full rotation
        if (lastFullRotation + fullRotationCooldown < Time.time)
        {
            aimedRot = (player.transform.eulerAngles.y + 180) % 360;
            rotToDo = Mathf.Abs(aimedRot - player.transform.eulerAngles.y);
            newRot = player.transform.eulerAngles.y;
            doingFullRotation = true;
        }
    }

    void DoRotation()
    {
        newRot = player.transform.eulerAngles.y + player.quickRotationSpeed * Time.deltaTime;
        rotToDo -= player.quickRotationSpeed * Time.deltaTime;
        player.transform.eulerAngles = new Vector3(transform.eulerAngles.x, newRot, transform.eulerAngles.z);

        if (rotToDo <= 0)
        {
            doingFullRotation = false;
            player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, aimedRot, player.transform.eulerAngles.z);
            lastFullRotation = Time.time;
        }
    }

    void CheckCameraLock()
    {
        if (Input.GetButtonUp("Viewport0"))
        {
            escargot.LockCamFromViewport(0);
            player.avatarAnimator.Play("CamChange1");
            unlockCamera = false;
        }
        else if (Input.GetButtonUp("Viewport1"))
        {
            escargot.LockCamFromViewport(1);
            player.avatarAnimator.Play("CamChange2");
            unlockCamera = false;
        }
        else if (Input.GetButtonUp("Viewport2"))
        {
            escargot.LockCamFromViewport(2);
            player.avatarAnimator.Play("CamChange2");
            unlockCamera = false;
        }
        else if (Input.GetButtonUp("Viewport3"))
        {
            escargot.LockCamFromViewport(3);
            player.avatarAnimator.Play("CamChange2");
            unlockCamera = false;
        }
    }

    void CheckViewportSelection()
    {
        if (Input.GetButtonUp("Viewport0"))
        {
            escargot.SetMainViewport(0);
            InfoCollector.GetInstance().WritePlaytestNote(player.transform.position, "InputManager : switch Viewport 1");
            player.avatarAnimator.Play("CamChange1");
            unlockCamera = false;
        }
        else if (Input.GetButtonUp("Viewport1"))
        {
            escargot.SetMainViewport(1);
            InfoCollector.GetInstance().WritePlaytestNote(player.transform.position, "InputManager : switch Viewport 2");
            player.avatarAnimator.Play("CamChange2");
            unlockCamera = false;
        }
        else if (Input.GetButtonUp("Viewport2"))
        {
            escargot.SetMainViewport(2);
            InfoCollector.GetInstance().WritePlaytestNote(player.transform.position, "InputManager : switch Viewport 3");
            player.avatarAnimator.Play("CamChange2");
            unlockCamera = false;
        }
        else if (Input.GetButtonUp("Viewport3"))
        {
            escargot.SetMainViewport(3);
            InfoCollector.GetInstance().WritePlaytestNote(player.transform.position, "InputManager : switch Viewport 4");
            player.avatarAnimator.Play("CamChange2");
            unlockCamera = false;
        }
    }

    void CheckViewportSelectionPad()
    {
        rightStickH = Input.GetAxisRaw("RightStickHorizontal");
        rightStickV = Input.GetAxisRaw("RightStickVertical");
        float inputMagnitude = new Vector2(rightStickH, rightStickV).magnitude;
        int currentSelected = -1;

        if (inputMagnitude < rightStickReset && escargot.currentSelectedRank != 0)
        {
            previouslySelected = -1;
            CheckSelectionReset();
        }
        else
        {
            checkingSelectionReset = false;
        }
        
        if(rightStickH < -minMagnitudeSelection)
        {
            if (rightStickV < -minMagnitudeSelection)
                currentSelected = 0;        //Stick is up left
            else if (rightStickV > minMagnitudeSelection)
                currentSelected = 1;        //Stick is down left
        }
        else if (rightStickH > minMagnitudeSelection)
        {
            if (rightStickV > minMagnitudeSelection)
                currentSelected = 2;         //Stick is down right
            else if (rightStickV < -minMagnitudeSelection)
                currentSelected = 3;         //Stick is up right
        }

        if (currentSelected != -1 && currentSelected != previouslySelected)
        {
            previouslySelected = currentSelected;
            escargot.SelectViewportById(currentSelected);
        }

    }

    void CheckSelectionReset()
    {
        if (!checkingSelectionReset)
        {
            checkingSelectionReset = true;
            selectionResetCheckStart = Time.time;
        }
        else if (selectionResetCheckStart + selectionResetCountdown < Time.time)
        {
            checkingSelectionReset = false;
            escargot.ResetSelection();
        }
    }

    void CheckActions()
    {
        //Use an Object
        if(Input.GetButtonDown("Use"))
        {
            player.TestSurroundingObjects();
        }

        //Unlock Camera
        if (Input.GetButtonDown("LockCamera"))
            unlockCamera = true;
        if (Input.GetButtonUp("LockCamera") && unlockCamera)
            camManager.UnlockCamera();

        //Open camera selection menu
        if(Input.GetButtonDown("SelectionMenu"))
        {
            CameraSelectionScreen.GetInstance().SwitchCanvasState();
        }

        if(Input.GetButtonDown("SelectNextViewport"))
        {
            escargot.SelectNextViewport();
        }

        if (Input.GetButtonDown("ValidateViewport") || Input.GetAxis("R") > 0)
        {
            escargot.SetSelectedViewportAsMain();
        }

        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheelInput > 0)
            escargot.SelectNextViewport();
        else if (scrollWheelInput < 0)
            escargot.SelectPreviousViewport();
    }
}
