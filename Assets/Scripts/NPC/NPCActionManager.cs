using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCActionManager : MonoBehaviour {

    private static NPCActionManager _instance;

    void Awake()
    {
        _instance = this;
    }

    public static NPCActionManager GetInstance()
    {
        return _instance;
    }

    static List<ActionBehaviour> actions;

	// Use this for initialization
	void Start () {
        actions = new List<ActionBehaviour>();
        SearchInChildren(transform);
	}

    void SearchInChildren(Transform transf)
    {
        foreach (Transform child in transf)
        {
            switch (child.gameObject.tag)
            {
                case "Action":
                    actions.Add(child.GetComponent<ActionBehaviour>());
                    break;
            }

            SearchInChildren(child);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DoAction(NPCBehaviour npc, NPCGroupBehaviour group)
    {
        int rand = Random.Range(0, actions.Count);
        if(actions[rand].HasEnoughCapacity())
        {
            switch(actions[rand].gameObject.name)
            {
                case "Train" :
                    TrainBehaviour train = actions[rand].GetComponent<TrainBehaviour>();

                    if(train.state != TrainBehaviour.TrainState.WaitingForPassengers)
                    {
                        train.passengersBoarding.Add(npc);
                        npc.aimedAction = actions[rand];
                        actions[rand].npcUsingAction++;
                        group.ActionAccepted(npc);
                    }
                    break;

                default :
                    npc.navAgent.SetDestination(actions[rand].transform.position);
                    npc.aimedAction = actions[rand];
                    actions[rand].npcUsingAction++;
                    group.ActionAccepted(npc);
                    break;
            }
        }
    }
}