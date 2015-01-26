using UnityEngine;
using System.Collections;

public class NPCBehaviour : MonoBehaviour {

    public NavMeshAgent navAgent{ get; set; }
    public Animator animator{ get; set; }
    public Vector3 InitialPosition { get; set; }
    public NPCGroupBehaviour parent { get; set; }
    public ActionBehaviour aimedAction { get; set; }
    public bool actionDone { get; set; }
    public Renderer rend { get; set; }

    public string StartAnimationName;
    public bool ManagedByGroup;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if(ManagedByGroup)
            parent = transform.parent.GetComponent<NPCGroupBehaviour>();        //Need the NPC to be directly in a NPCGroup
        rend = GetComponentInChildren<Renderer>();

        actionDone = false;
        InitialPosition = transform.position;
        animator.Play(StartAnimationName);
	}

    void Update()
    {
        if(actionDone)
            CheckReturnedToPosition();
    }

    void CheckReturnedToPosition()
    {
        if (Vector3.Distance(transform.position, InitialPosition) < 5f && actionDone)
        {
            EventsManager.GetInstance().FireActionEnd(parent, this);
            actionDone = false;
            aimedAction = null;
            animator.SetBool("AnimationDone", false);
        }
    }
}
