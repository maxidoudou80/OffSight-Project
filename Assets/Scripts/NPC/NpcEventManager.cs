using UnityEngine;
using System.Collections;

public class NpcEventManager : MonoBehaviour {

    NPCBehaviour parentObject;

	// Use this for initialization
	void Start () {
        parentObject = transform.parent.GetComponent<NPCBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void animationEnd()
    {
        parentObject.actionDone = true;
        parentObject.navAgent.SetDestination(parentObject.InitialPosition);
        GetComponent<Animator>().SetBool("AnimationDone", true);
    }
}
