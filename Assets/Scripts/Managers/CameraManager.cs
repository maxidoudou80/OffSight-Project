using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class CameraManager : MonoBehaviour {

    private const int MAX_SELECTED_CAMERAS = 4;
    private const int MAX_LINKED_CAMERAS = 3;

    private static CameraManager _instance;
    public static CameraManager GetInstance()
    {
        return _instance;
    }

    public enum State { FreeCircuit, ClosedCircuit, LockedCam }
    public State state { get; set; }

    InfoCollector info;
    Escargot escargot;
    public int startingCamera;
    List<CameraBehaviour> sortingList;
    public Dictionary<int, CameraBehaviour> LevelCameras { get; set; }
    private CameraBehaviour currentMainCamera;
    public List<int> currentLinkedCameras{ get; set; }
    public List<int> selectedCameras{ get; set; }
    
    public int currentLockedCamId { get; set; }
    public List<int> LockedCamLinks { get; set;}

    public GUIStyle style;

    //culling
    public LayerMask nothingLayer;
    public LayerMask everythingLayer;

    //Vignettes
    bool updateVignettes;
    List<CameraBehaviour> camList;          // Created to go through all cams without needing following indexes
    int vignetteIndex;
    string vignettesPath;

    void Awake()
    {
        _instance = this;
        sortingList = new List<CameraBehaviour>();
        LevelCameras = new Dictionary<int, CameraBehaviour>();
        currentLinkedCameras = new List<int>();
        SearchInChildren(transform);
        sortingList = sortingList.OrderBy(c => c.id).ToList();
        sortingList.ForEach(c => LevelCameras.Add(c.id, c));
        currentMainCamera = LevelCameras[startingCamera];   
        UpdateLinkedCameras();
    }

	// Use this for initialization
	void Start () {
        
        state = State.FreeCircuit;
        info = InfoCollector.GetInstance();
        escargot = Escargot.GetInstance();
        selectedCameras = new List<int>();
        LockedCamLinks = new List<int>();
        HideAllCams();
        ShowCurrentCameras();
        vignetteIndex = 0;

        //ScreenCaptures for vignettes
        camList = new List<CameraBehaviour>();
        foreach (CameraBehaviour cam in LevelCameras.Values)
        {
            camList.Add(cam);
        }

        vignettesPath = "Assets\\Resources\\Vignettes\\" + Application.loadedLevelName + "\\";
        if (!Directory.Exists(vignettesPath))
            Directory.CreateDirectory(vignettesPath);
        updateVignettes = false;
	}

    void Update()
    {
        if (updateVignettes)
            CaptureVignettes();
    }

    void SearchInChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            switch (child.gameObject.tag)
            {
                case "Camera" :
                    CameraBehaviour camera = child.GetComponent<CameraBehaviour>();
                    sortingList.Add(camera);
                    break;
            }

            SearchInChildren(child.transform);
        }
    }

    public void SetMainCamera(int index)
    {
        if (currentMainCamera != null)
            currentMainCamera.gameObject.SetActive(false);

        if (LevelCameras.TryGetValue(index, out currentMainCamera))
        {
            foreach (int camId in currentLinkedCameras)
            {
                if (camId != currentMainCamera.id)
                    LevelCameras[camId].gameObject.SetActive(false);
            }

            UpdateLinkedCameras();
            ShowCurrentCameras();

            //UIElements.GetInstance().UpdateYellowLightState(currentMainCamera);
            //if (Time.time > 2)
            //  EventsManager.GetInstance().FireCameraUpdateEvent(currentMainCamera);

            //EventsManager.GetInstance().HandleCameraEvent(currentMainCamera);

            //UIElements.GetInstance().UpdateYellowLightState(currentMainCamera);
        }
    }

    public void HideAllCams()
    {
        foreach(CameraBehaviour cam in LevelCameras.Values)
        {
            cam.gameObject.SetActive(false);
        }
    }

    void ShowCurrentCameras()
    {
        currentMainCamera.gameObject.SetActive(true);

        foreach (int camId in currentLinkedCameras)
        {
            LevelCameras[camId].gameObject.SetActive(true);
        }
    }

    public void AddCameraToSelection(int camId)
    {
        if (selectedCameras.Count < MAX_SELECTED_CAMERAS)
        {
            if(selectedCameras.Count < 1)
            {
                currentMainCamera = LevelCameras[camId];
            }

            selectedCameras.Add(camId);
        }
    }


    public void LockCamera(int id)
    {
        Debug.Log("Lock Camera : " + id);
        currentLockedCamId = id;
        state = State.LockedCam;
        UpdateLinkedCameras();

        if (id == currentMainCamera.id)
            escargot.SetMainViewport(currentLinkedCameras[0], true);
        else
            escargot.SetMainViewport(escargot.mainViewportId, true);
    }

    public void UnlockCamera()
    {
        if(state == State.LockedCam)
        {
            state = State.FreeCircuit;
            UpdateLinkedCameras();
            escargot.SetMainViewport(escargot.mainViewportId, true);
        }
    }

    public void UpdateLinkedCameras()
    {
        currentLinkedCameras.Clear();

        switch(state)
        {
            case State.FreeCircuit:

                currentMainCamera.linkedCameras.ForEach(c => AddToLinkedCameras(c));

                break;
            
            case State.ClosedCircuit :

                selectedCameras.FindAll(c => c != currentMainCamera.id).ForEach(c => AddToLinkedCameras(c));

                break;

            case State.LockedCam:

                AddToLinkedCameras(currentLockedCamId);
                currentMainCamera.linkedCameras.FindAll(c => c != currentLockedCamId).ForEach(c => AddToLinkedCameras(c));
                
                break;
        }
    }

    public void AddToLinkedCameras(int camId)
    {
        if (currentLinkedCameras.Count < MAX_LINKED_CAMERAS)
        {
            currentLinkedCameras.Add(camId);
        }
        else
        {
            Debug.Log("No More Room");
        }
    }

    public CameraBehaviour GetCurrentMain()
    {
        return currentMainCamera;
    }

    public void BeginCapture()
    {
        updateVignettes = true;
    }

    public void CaptureVignettes()
    {
        if(vignetteIndex < camList.Count)
        {
            HideAllCams();
            camList[vignetteIndex].gameObject.SetActive(true);
            camList[vignetteIndex].camera.rect = new Rect(0, 0, 1, 1);
            Application.CaptureScreenshot(vignettesPath + "VignetteCam" + camList[vignetteIndex].GetComponent<CameraBehaviour>().id + ".png");
            Debug.Log("capture taken : " + "VignetteCam" + camList[vignetteIndex].GetComponent<CameraBehaviour>().id + ".png");
            vignetteIndex++;
        }
        else
        {
            updateVignettes = false;
        }
    }
}
