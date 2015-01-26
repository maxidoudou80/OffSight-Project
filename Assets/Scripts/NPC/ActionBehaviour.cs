using UnityEngine;
using System.Collections;

public class ActionBehaviour : MonoBehaviour {

    public int capacity;
    public int npcUsingAction { get; set; } 

	// Use this for initialization
	void Start () {
        EventsManager.GetInstance().actionEnd += NpcEndedAnimation;
	}

    public bool HasEnoughCapacity () 
    {
        if (npcUsingAction < capacity)
            return true;
        else
            return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "NPC")
        {
            NPCBehaviour npc = other.GetComponent<NPCBehaviour>();
            if (npc.aimedAction == this && !npc.actionDone)
            {
                npc.animator.Play(transform.name);
                if(gameObject.name != "Train")
                {
                    npc.navAgent.SetDestination(npc.transform.position);
                    npc.actionDone = true;
                }
            }
        }
    }

    public void NpcEndedAnimation(NPCGroupBehaviour group, NPCBehaviour npc)
    {
        if(npc.aimedAction == this)
            npcUsingAction--;
    }
}