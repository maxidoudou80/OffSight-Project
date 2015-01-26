using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class NPCGroupBehaviour : MonoBehaviour {

    List<NPCBehaviour> children;
    public enum PanicBehaviour { run, fear };
    public PanicBehaviour panicBehaviour;
    public float NPCActionDelay;
    private float lastNPCAction;
    public Material freeMat;
    public Material usedMat;

    List<NPCBehaviour> freeNpcs;  
    
    
    // Use this for initialization
	void Start () {
        children = new List<NPCBehaviour>();
        freeNpcs = new List<NPCBehaviour>();
        SearchInChildren(transform);
        lastNPCAction = -NPCActionDelay;

        EventsManager.GetInstance().actionEnd += HandleNPCActionEnd;
	}

    void SearchInChildren(Transform transf)
    {
        foreach (Transform child in transf)
        {
            switch (child.gameObject.tag)
            {
                case "NPC":
                    children.Add(child.GetComponent<NPCBehaviour>());
                    freeNpcs.Add(child.GetComponent<NPCBehaviour>());
                    break;
            }

            SearchInChildren(child);
        }
    }

	void Update () {
        if (Input.GetKeyDown("space"))
        {
            StartPanic();
        }

        if(lastNPCAction + NPCActionDelay < Time.time && freeNpcs.Count > 0)
        {
            lastNPCAction = Time.time;
            PickForAction();
        }
	}

    public void StartPanic()
    {
        switch(panicBehaviour)
        {
            case PanicBehaviour.run:
                children.ForEach(c => StartCoroutine(RunPointsManager.FindShortest(c.navAgent)));
                break;

            case PanicBehaviour.fear:
                children.ForEach(c => c.animator.Play("NPCFear"));
            break;
        }
    }

    public void PickForAction()
    {
        int rand = Random.Range(0, freeNpcs.Count -1);
        NPCActionManager.GetInstance().DoAction(freeNpcs[rand], this);
    }

    public void ActionAccepted(NPCBehaviour npc)
    {
        npc.rend.material = usedMat;
        freeNpcs.Remove(npc);
    }

    public void HandleNPCActionEnd(NPCGroupBehaviour npcGroup, NPCBehaviour npc)
    {
        if(npcGroup == this)
        {
            freeNpcs.Add(npc);
            npc.rend.material = freeMat;
        }
    }
}
