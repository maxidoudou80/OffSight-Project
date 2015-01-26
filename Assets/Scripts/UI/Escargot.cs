using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Escargot : MonoBehaviour
{
    private static Escargot _instance = null;
    public static Escargot GetInstance()
    {
        return _instance;
    }

    const int NUMBER_OF_DISPLAYED_CAMERAS = 4;
    const int NUMBER_OF_LINKED_CAMERAS = 3;
    private CameraManager camManager;
    public List<ViewportInfo> viewports { get; set; }
    public int currentSelectedRank { get; set; }       //identifie le viewport séléectionné par son rang
    public int mainViewportId { get; set; }
    public int ViewportClosed { get; set; }
    public float maxSize;
    public List<float> rankSpeeds;      //Vitesse à laquelle s'ouvrent et se ferment les panels
    public List<float> rankScales;       //Largeur des caméras en fonction du rang

    //Viewports properties
    public Vector2 vp0Offset;
    public Vector2 vp1Offset;
    public Vector2 vp2Offset;
    public Vector2 vp3Offset;

    //BackPanels properties
    public float backPanelScaleOffset;
    public GameObject backPanel0;
    public GameObject backPanel1;
    public GameObject backPanel2;
    public GameObject backPanel3;
    public Vector2 bpOffset0;
    public Vector2 bpOffset1;
    public Vector2 bpOffset2;
    public Vector2 bpOffset3;
    public Sprite bpSelected;
    public Sprite bpCameraNotAvailable;

    //Curves
    public AnimationCurve xTransition;
    public AnimationCurve yTransition;
    public float transitionSpeed;
    float transitionTime;

    public Vector2[] anim;
    Vector2 basePos;
    float animationTime;

    public int GetNumberOfDisplayedCameras()
    {
        return NUMBER_OF_DISPLAYED_CAMERAS;
    }

    void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {
        camManager = CameraManager.GetInstance();
        viewports = new List<ViewportInfo>();
        //SearchInChildren(transform);
        InitializeViewports();
        viewports = viewports.OrderBy(v => v.id).ToList();      //On s'assure que la liste est organisée par id croissant

        UpdateBackPanelsSprites();
        UpdateBackPanelsSelectionStatus();
    }

    public void InitializeViewports()
    {
        viewports.Clear();
        mainViewportId = 0;

        ViewportInfo viewp0 = new ViewportInfo(0, vp0Offset, true, false, backPanel0,
            ViewportInfo.BackPanelPivot.DownRight, bpOffset0);
        ViewportInfo viewp1 = new ViewportInfo(1, vp1Offset, true, true, backPanel1,
            ViewportInfo.BackPanelPivot.UpRight, bpOffset1);
        ViewportInfo viewp2 = new ViewportInfo(2, vp2Offset, false, true, backPanel2,
            ViewportInfo.BackPanelPivot.UpLeft, bpOffset2);
        ViewportInfo viewp3 = new ViewportInfo(3, vp3Offset, false, false, backPanel3,
            ViewportInfo.BackPanelPivot.DownLeft, bpOffset3);
        viewports.Add(viewp0);
        viewports.Add(viewp1);
        viewports.Add(viewp2);
        viewports.Add(viewp3);
    }

    void SearchInChildren(Transform transf)
    {
        foreach (Transform child in transf)
        {
            switch (child.gameObject.tag)
            {
            }

            SearchInChildren(child);
        }
    }

    public void SetMainViewport(int newMainViewportlId, bool forceReset = false)
    {
        //on ne swap pas si c'est déjà le viewport principal || si c'est la camera bloquée
        if ((newMainViewportlId != mainViewportId
            && (camManager.state != CameraManager.State.LockedCam
                || viewports[newMainViewportlId].currentCamera.id != camManager.currentLockedCamId))
            || forceReset)
        {
            mainViewportId = newMainViewportlId;
            ViewportClosed = 0;
            viewports.ForEach(v => v.ResizeViewport());
        }
    }

    // Update is called once per frame
    void Update()
    {
        viewports.ForEach(v => v.UpdateViewport());

        //viewports[0].position.x = vp0Offset.x + xTransition.Evaluate(transitionTime) * 0.2f;
        //viewports[0].position.y = vp0Offset.y + yTransition.Evaluate(transitionTime) * 0.2f;
        //transitionTime += transitionSpeed;

        /*viewports[0].offset = vp0Offset;
        viewports[1].offset = vp1Offset;
        viewports[2].offset = vp2Offset;
        viewports[3].offset = vp3Offset;*/
    }

    public void HandleViewportClosed()
    {
        ViewportClosed++;

        if (ViewportClosed >= NUMBER_OF_LINKED_CAMERAS)
        {
            UpdateCameraManager();
            UpdateBackPanelsSprites();
            UpdateBackPanelsSelectionStatus();
        }
    }

    public void UpdateCameraManager()
    {
        camManager.SetMainCamera(viewports[mainViewportId].currentCamera.id);
        viewports.ForEach(v => v.LinkToNewCamera());
    }

    public CameraBehaviour GetCameraFromViewport(int viewportId)
    {
        return viewports[viewportId].currentCamera;
    }

    public void LockCamFromViewport(int id)
    {
        if (id != GetCameraFromViewport(id).id)
            camManager.LockCamera(GetCameraFromViewport(id).id);
    }

    public void SelectNextViewport()
    {
        if (currentSelectedRank == 3)
            currentSelectedRank = 0;
        else
        {
            currentSelectedRank++;

            //Si la caméra est privée on re-entre dans la boucle
            if (GetCameraFromRank(currentSelectedRank).privateCamera)
            {
                SelectNextViewport();
            }
        }

        UpdateBackPanelsSelectionStatus();
    }

    public void SelectPreviousViewport()
    {
        if (currentSelectedRank == 0)
            currentSelectedRank = 3;
        else
            currentSelectedRank--;

        if (viewports[currentSelectedRank].currentCamera.privateCamera)
            SelectPreviousViewport();

        UpdateBackPanelsSelectionStatus();
    }

    public void SelectViewportById(int selectedId)
    {
        currentSelectedRank = viewports[selectedId].currentRank;

        UpdateBackPanelsSelectionStatus();
    }

    public void UpdateBackPanelsSelectionStatus()
    {
        foreach (ViewportInfo v in viewports)
        {
            if (v.currentCamera.privateCamera || (v.currentRank == currentSelectedRank && currentSelectedRank != 0))
            {
                v.EnableBackPanelImage(true);
            }
            else
            {
                v.EnableBackPanelImage(false);
            }
        }
    }

    void UpdateBackPanelsSprites()
    {
        foreach (ViewportInfo v in viewports)
        {
            if (v.currentCamera.privateCamera)
                v.SetBackPanelSprite(bpCameraNotAvailable);
            else
                v.SetBackPanelSprite(bpSelected);
        }
    }

    public void SetSelectedViewportAsMain()
    {

        foreach (ViewportInfo v in viewports)
        {
            if (v.currentRank == currentSelectedRank && !v.currentCamera.privateCamera)
            {
                SetMainViewport(v.id);
                break;
            }
        }

        currentSelectedRank = 0;
        UpdateBackPanelsSelectionStatus();
    }

    public void ResetSelection()
    {
        if (currentSelectedRank != 0)
        {
            currentSelectedRank = 0;
            UpdateBackPanelsSelectionStatus();
        }
    }

    CameraBehaviour GetCameraFromRank(int rank)
    {
        int index = camManager.currentLinkedCameras[rank - 1];
        return camManager.LevelCameras[index];
    }
}
