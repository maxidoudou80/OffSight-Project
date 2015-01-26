using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraSelectionScreen : MonoBehaviour {

    static private CameraSelectionScreen _instance;
    static public CameraSelectionScreen GetInstance() {return _instance;}

    public Canvas canvas { get; set; }
    Escargot escargot;
    CameraManager camManager;
    public List<SelectionPage> pages { get; set; }
    public List<GameObject> camButtons { get; set; }
    public int currentPage { get; set; }
    int currentCamInPage;
    int currentlyDisplayedPage;
    Vector2 previousCamPosition;
    public UnityEngine.EventSystems.EventSystem eventSystem;
    bool pageSwapping;
    int aimedPage;
    float move;
    int moveDifference;
    Transform group;

    public Vector2 pageStart;
    public float pageMoveSpeed;
    public int maxCamPerPage;
    public int maxCamPerLine;
    public float pageXGap;
    public Vector2 camGap;
    public Vector2 vignettesSize;
    public GameObject pagePrefab;

	// Use this for initialization
	void Awake () {
        _instance = this;
        canvas = GetComponentInChildren<Canvas>();
        pages = new List<SelectionPage>();
        camButtons = new List<GameObject>();
        GameObject eventSystemGO = GameObject.FindGameObjectWithTag("EventSystem") as GameObject;
        eventSystem = eventSystemGO.GetComponent<EventSystem>();
	}

    void Start()
    {
        escargot = Escargot.GetInstance();
        camManager = CameraManager.GetInstance();
        SearchInChildren(transform);
        CreatePage();
        CreateSelectionMenu();
        canvas.enabled = false;
    }

    void SearchInChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            switch (child.gameObject.tag)
            {
                case "Group":
                    group = child;
                    break;
            }

            SearchInChildren(child.transform);
        }
    }
	// Update is called once per frame
	void Update () {
        if (pageSwapping)
            MovePages();
        else if(canvas.enabled)
            CheckDisplayedPage(true);


	}

    void CreateSelectionMenu()
    {
        foreach (CameraBehaviour cam in camManager.LevelCameras.Values)
        {
            GameObject button = new GameObject("buttonCam" + cam.id);
            button.AddComponent<CamSelectionButton>().linkedCamId = cam.id;
            button.AddComponent<Image>().sprite = cam.vignette;

            button.AddComponent<Button>();
            Button buttonComp = button.GetComponent<Button>();
            buttonComp.onClick.AddListener(() => ButtonReaction(button.GetComponent<CamSelectionButton>().linkedCamId));
            buttonComp.targetGraphic = button.GetComponent<Image>();
            ColorBlock cb = buttonComp.colors;
            cb.highlightedColor = Color.red;
            buttonComp.colors = cb;

            RectTransform transf = button.GetComponent<RectTransform>();
            button.transform.localScale = new Vector3(vignettesSize.x * Screen.width, vignettesSize.y * Screen.height, 1);
            transf.anchorMin = new Vector2(0, 1);
            transf.anchorMax = new Vector2(0, 1);

            if (pages[currentPage].numberOfCam >= maxCamPerPage)
            {
                if (currentPage < pages.Count)
                    currentPage++;
                CreatePage();
            }
            button.GetComponent<CamSelectionButton>().pageId = currentPage;
            transf.SetParent(pages[currentPage].transform);
            transf.localPosition = GetNextCamPosition();
            transf.pivot = new Vector2(0, 1);
            previousCamPosition = transf.anchoredPosition;
            pages[currentPage].numberOfCam++;
            camButtons.Add(button);
        }
    }

    Vector2 GetNextCamPosition()
    {
        Vector2 nextPosition = new Vector2(0, 0);

        if (pages[currentPage].numberOfCam > 0 && pages[currentPage].numberOfCam % maxCamPerLine == 0)
        {
            pages[currentPage].lineNumber++;
            nextPosition = new Vector2(0, pages[currentPage].lineNumber * (camGap.y * Screen.height));
        }
        else if (pages[currentPage].numberOfCam > 0)
        {
            nextPosition = new Vector2(previousCamPosition.x + (camGap.x * Screen.width), previousCamPosition.y);
        }
        return nextPosition;
    }

    public void CreatePage()
    {
        GameObject page = GameObject.Instantiate(pagePrefab) as GameObject;
        SelectionPage pageScript = page.GetComponent<SelectionPage>();
        page.name = "Page " + pages.Count;
        page.transform.SetParent(group);
        pageScript.Initialize(currentPage, pageXGap * Screen.width, pageStart);
        pages.Add(pageScript);
    }

    public void UpdateCurrentPage(bool doMove)
    {
        moveDifference = currentlyDisplayedPage - aimedPage;
        move = Mathf.Abs(pageXGap * moveDifference * Screen.width);

        if(doMove)
            pageSwapping = true;
        else
        {
            group.position = new Vector3(group.position.x + move * Mathf.Sign(moveDifference), group.position.y, group.position.z);
            currentlyDisplayedPage = aimedPage;
        }
    }

    public void MovePages()
    {
        if (move > 0)
        {
            group.position = new Vector3(group.position.x + pageMoveSpeed * Mathf.Sign(moveDifference) * Time.deltaTime, group.position.y, group.position.z);

            move -= pageMoveSpeed * Time.deltaTime;
        }
        else
        {
            pageSwapping = false;
            currentlyDisplayedPage = aimedPage;
        }
    }

    public void ButtonReaction(int id)
    {
        camManager.SetMainCamera(id);
        escargot.InitializeViewports();
        GameManager.GetInstance().gameState = GameManager.GameState.Playing;
        canvas.enabled = false;
    }

    public void SwitchCanvasState()
    {
        canvas.enabled = !canvas.enabled;

        if(canvas.enabled)
        {
            eventSystem.SetSelectedGameObject(camButtons[camManager.GetCurrentMain().id]);
            GameManager.GetInstance().gameState = GameManager.GameState.CamSelection;

            CheckDisplayedPage(false);
        }
        else
        {
            GameManager.GetInstance().gameState = GameManager.GameState.Playing;
        }
    }

    public void CheckDisplayedPage(bool doMove)
    {
        if (eventSystem.currentSelectedGameObject.GetComponent<CamSelectionButton>().pageId != currentlyDisplayedPage)
        {
            aimedPage = eventSystem.currentSelectedGameObject.GetComponent<CamSelectionButton>().pageId;
            UpdateCurrentPage(doMove);
        }
    }
}