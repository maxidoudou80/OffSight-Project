using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ViewportInfo
{
    Escargot escargot;          //The "escargot" manage all viewportInfos
    CameraManager camManager;
    public int id;
    public int currentRank;          //Rank in interface viewports
    public CameraBehaviour currentCamera; //Current camera using this viewport

    public enum BackPanelPivot { UpRight, UpLeft, DownRight, DownLeft }
    public BackPanelPivot panelPivot;       // Need to check if accurate
    GameObject backPanel;
    RectTransform backPanelRect;
    Vector2 backPanelOffset;

    const float WORKINGMaxSize = 0.47f;
    public Vector2 offset;     //Base offset for interface "snail shell" design
    public Vector2 position;
    float size;        //Current width (and height) Screen ratio
    bool updateXAxis;
    bool updateYAxis;
    int aimedSizeMultiplicator; //multiplicator to switch between expanding/closing the viewport
    bool isChangingSize;
    float resizeSpeed;

	// Use this for initialization

    public ViewportInfo(int viewportId, Vector2 offs, bool moveOnX, bool moveOnY, GameObject backP,
        BackPanelPivot pivot, Vector2 panelOffset)
    {
        escargot = Escargot.GetInstance();
        camManager = CameraManager.GetInstance();
        id = viewportId;
        offset = offs;
        updateXAxis = moveOnX;
        updateYAxis = moveOnY;
        backPanel = backP;
        backPanelRect = backPanel.GetComponent<RectTransform>();
        panelPivot = pivot;
        backPanelOffset = panelOffset;
        UpdateCurrentRank();


        //On set la camera
        int cameraIndex;
        if (id != escargot.mainViewportId)
            cameraIndex = camManager.currentLinkedCameras[currentRank - 1];
        else
            cameraIndex = camManager.GetCurrentMain().id;

        currentCamera = camManager.LevelCameras[cameraIndex];
        currentCamera.gameObject.SetActive(true);

        size = escargot.rankScales[currentRank] * escargot.maxSize;
        UpdatePosition();
        UpdateCurrentCamera();
        UpdateBackPanelPosition();
        UpdateBackPanelSize();
        EnableBackPanelImage(false);
    }

	// Update is called once per frame
	public void UpdateViewport () {

        //UpdatePosition();
        UpdateCurrentCamera();
        if(isChangingSize)
        {
            UpdateSize();
            UpdatePosition();
            UpdateCurrentCamera();
            UpdateBackPanelPosition();
            UpdateBackPanelSize();
        }
	}

    void UpdateCurrentRank()
    {
        if (id >= escargot.mainViewportId)
            currentRank = id - escargot.mainViewportId;
        else
            currentRank = id - escargot.mainViewportId + escargot.GetNumberOfDisplayedCameras();
    }

    public void ResizeViewport()
    {
        UpdateCurrentRank();

        if (id == escargot.mainViewportId)
            ExpandViewport();
        else
            CloseViewport();
    }

    void UpdateSize()
    {
        if (aimedSizeMultiplicator == 1 && size >= escargot.rankScales[currentRank] * escargot.maxSize)
        {
            isChangingSize = false;
            size = escargot.rankScales[currentRank] * escargot.maxSize;
        }
        else if (aimedSizeMultiplicator == -1 && size <= 0)
        {
            size = 0;
            isChangingSize = false;
            escargot.HandleViewportClosed();
        }
        else
        {          
            size += escargot.rankSpeeds[currentRank] * aimedSizeMultiplicator * Time.deltaTime;
        }
    }

    void UpdatePosition()
    {
        if (updateXAxis && updateYAxis)
            position = new Vector2(offset.x + (escargot.maxSize - size), offset.y + (escargot.maxSize - size));
        else if (updateXAxis)
            position = new Vector2(offset.x + (escargot.maxSize - size), offset.y);
        else if (updateYAxis)
            position = new Vector2(offset.x, offset.y + (escargot.maxSize - size));
        else
            position = offset;

        //UpdateCurrentCamera();
    }

    void UpdateCurrentCamera()
    {
        currentCamera.camera.rect = new Rect(position.x, position.y, size, size);
    }

    public void LinkToNewCamera()
    {
        if (id != escargot.mainViewportId)
        {
            int cameraIndex = camManager.currentLinkedCameras[currentRank - 1];
            Debug.Log("rank :  " + currentRank + " cam : " + cameraIndex);
            //On attribue la nouvelle
            currentCamera = camManager.LevelCameras[cameraIndex];
            currentCamera.camera.rect = new Rect(0, 0, 0, 0);

            ExpandViewport();
        }
        else
        {
            currentCamera = camManager.GetCurrentMain();
        }
    }

    void ExpandViewport()
    {
        aimedSizeMultiplicator = 1;
        isChangingSize = true;
    }

    void CloseViewport()
    {
        aimedSizeMultiplicator = -1;
        isChangingSize = true;
    }

    void UpdateBackPanelPosition()
    {
        // Juste need to adjust the position depending on the pivot

        switch (panelPivot)
        {
            case BackPanelPivot.DownRight:
                backPanel.transform.position = new Vector3(position.x * Screen.width
                    + size * Screen.width + backPanelOffset.x * Screen.width //Add viewport width to position
                    , position.y * Screen.height - backPanelOffset.y * Screen.height, backPanel.transform.position.z);                
                break;
            case BackPanelPivot.UpRight:
                backPanel.transform.position = new Vector3(position.x * Screen.width
                    + size * Screen.width + backPanelOffset.x * Screen.width,
                    position.y * Screen.height + size * Screen.height + backPanelOffset.y * Screen.height, //Add viewport width and height to position
                    backPanel.transform.position.z); 
                break;
            case BackPanelPivot.UpLeft:
                backPanel.transform.position = new Vector3(position.x * Screen.width - backPanelOffset.x * Screen.width,
                    position.y * Screen.height + size * Screen.height + backPanelOffset.y * Screen.height, //Add viewport's Height to position
                    backPanel.transform.position.z); 
                break;
            case BackPanelPivot.DownLeft:
                backPanel.transform.position = new Vector3(position.x * Screen.width - backPanelOffset.x * Screen.width,
                    position.y * Screen.height - backPanelOffset.y * Screen.height, //Add nothing
                    backPanel.transform.position.z);
                break;
        }
    }

    void UpdateBackPanelSize()
    {
        backPanelRect.sizeDelta = new Vector2(size * Screen.width + backPanelOffset.x * Screen.width * 2,
            size * Screen.height + backPanelOffset.y * Screen.height * 2);

        /*backPanelRect.localScale = new Vector3(escargot.rankScales[currentRank],
            escargot.rankScales[currentRank], backPanelRect.localScale.z);
        backPanelRect.sizeDelta += new Vector2 (backPanelOffset.x, 0);*/
    }

    public void EnableBackPanelImage(bool b)
    {
        backPanel.GetComponent<Image>().enabled = b;
    }

    public void SetBackPanelSprite(Sprite spr)
    {
        backPanel.GetComponent<Image>().sprite = spr;
    }
}
