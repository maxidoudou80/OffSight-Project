using UnityEngine;
using System.Collections;

public class EventsManager : MonoBehaviour {


    private static EventsManager _instance;

    void Awake()
    {
        _instance = this;
    }

    public static EventsManager GetInstance()
    {
        //if (_instance == null)
        //{
        //    _instance = this;
        //}

        return _instance;
    }

    //Inform NPCGroup and Actions that a NPC action is over
    public delegate void NpcActionEnd(NPCGroupBehaviour npcGroup, NPCBehaviour npc);
    public event NpcActionEnd actionEnd;
    public void FireActionEnd(NPCGroupBehaviour npcGroup, NPCBehaviour npc) { actionEnd(npcGroup, npc); }


    //Write a note about an event in game
    public delegate void PlaytestNote(string note);
    public event PlaytestNote playtestNote;
    public void WritePlaytestNote(string note) { playtestNote(note); }

    //Handle Camera Switch
    public delegate void CameraSwitch(CameraBehaviour mainCamera);
    public event CameraSwitch cameraSwitch;
    public void HandleCameraEvent(CameraBehaviour mainCamera) { cameraSwitch(mainCamera); }

    //Handle Camera Switch
    public delegate void NewCameraAvailable();
    public event NewCameraAvailable newCamAvailable;
    public void CamAvailableEvent() { newCamAvailable(); }
}