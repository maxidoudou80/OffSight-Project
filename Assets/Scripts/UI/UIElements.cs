using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIElements : MonoBehaviour {

    private static UIElements _instance;
    public static UIElements GetInstance()
    {
        return _instance;
    }

    public Text InstructionTitle { get; set; }
    public Text Instructions { get; set; }
    public Text Location { get; set; }
    public Text CamerasCount { get; set; }
    public Text ConnectedNumber { get; set; }

    public GameObject RedLight { get; set; }
    public GameObject YellowLight { get; set; }

    void Awake()
    {
        _instance = this;
        SearchInChildren(transform);
    }

    void Start()
    {
        EventsManager.GetInstance().cameraSwitch += UpdateYellowLightState;
        TurnRedLightOff();
        TurnYellowLightOff();
    }

    void SearchInChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            switch (child.gameObject.name)
            {
                case "InstructionTitle" :
                    InstructionTitle = child.GetComponent<Text>();
                    break;

                case "Instructions":
                    Instructions = child.GetComponent<Text>();
                    break;

                case "Location":
                    Location = child.GetComponent<Text>();
                    break;

                case "CamerasCount":
                    CamerasCount = child.GetComponent<Text>();
                    break;

                case "ConnectedNumber":
                    ConnectedNumber = child.GetComponent<Text>();
                    break;

                case "RedLight":
                    RedLight = child.gameObject;
                    break;

                case "YellowLight":
                    YellowLight = child.gameObject;
                    break;
            }

            SearchInChildren(child.transform);
        }
    }

    public void UpdateYellowLightState(CameraBehaviour mainCam)
    {
        if (mainCam.TargetWatching)
            TurnYellowLightOn();
        else
            YellowLight.gameObject.SetActive(false);
    }

    public void TurnRedLightOff()
    {
        RedLight.SetActive(false);
    }

    public void TurnRedLightOn()
    {
        RedLight.SetActive(true);
    }

    public void InverseRedLight()
    {
        RedLight.SetActive(!RedLight.activeSelf);
    }

    public void TurnYellowLightOn()
    {
        Debug.Log("On");
        YellowLight.SetActive(true);
    }

    public void TurnYellowLightOff()
    {
        YellowLight.SetActive(false);
    }

}
