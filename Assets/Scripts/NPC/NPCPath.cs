using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCPath : MonoBehaviour {

    NavMeshAgent navAgent;
    int count;
    public float breakStart { get; set; }
    bool moving;
    PlayerBehaviour player;
    public bool readyToMove { get; set; }

    public List<Vector3> destinations;
    public List<float> breaks;
    public float stopDistance;
    public float detectionRange;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        count = 0;

        player = GameManager.GetInstance().player;
	}
	
	// Update is called once per frame
	void Update () {
        if (moving)
            isDestinationReached();
        else if (readyToMove)
            isBreakOver();

        if (Vector3.Distance(player.transform.position, transform.position) < detectionRange)
        {
            ReactToPlayer();
        }
	}

    void isDestinationReached()
    {
        if (Vector3.Distance(transform.position, navAgent.destination) < stopDistance)
        {
            breakStart = Time.time;
            moving = false;
        }
    }

    void isBreakOver()
    {
        if (breaks.Count > count && Time.time > breakStart + breaks[count])
            SetNextDestination();
    }

    void SetNextDestination()
    {
        if (count < destinations.Count || count == 0)
        {
            navAgent.SetDestination(destinations[count]);
            moving = true;
            count++;
        }
    }

    void ReactToPlayer()
    {
        Debug.Log("Player detected");
    }
}
