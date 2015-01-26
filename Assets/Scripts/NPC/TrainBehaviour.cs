using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TrainBehaviour : MonoBehaviour {

    public float trainStopPos;
    public float leavingStationPos;
    public float departureTime;
    public float stopLength;
    public float waitAfterFull;
    public float travelLength;
    public float timeBeforeRespawn;
    public List<NPCBehaviour> passengersBoarding { get; set; }
    public List<NPCBehaviour> passengersOnBoard { get; set; }
    public List<Transform> seats { get; set; }

    Vector3 initialPos;
    float stopTime;
    float newX;
    float aimedPos;
    float arrivingTime;
    float leavingTime;
    float fullTime;
    bool destinationSet;

    public enum TrainState { departure, ToStation, WaitingForPassengers ,LeavingStation}
    public TrainState state{get; set;}

	// Use this for initialization
	void Start () {
        newX = transform.position.x;
        initialPos = transform.position;
        aimedPos = trainStopPos;
        passengersBoarding = new List<NPCBehaviour>();
        passengersOnBoard = new List<NPCBehaviour>();
        seats = new List<Transform>();
        destinationSet = false;
        fullTime = 0;

        state = TrainState.departure;
        SearchInChildren(transform);
	}

    void SearchInChildren(Transform transf)
    {
        foreach (Transform child in transf)
        {
            switch (child.gameObject.tag)
            {
                case "TrainSeat":
                    seats.Add(child.transform);
                    break;
            }

            SearchInChildren(child);
        }
    }
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case TrainState.departure :
                if(Time.time >= departureTime)
                {
                    state = TrainState.ToStation;
                }
                break;

            case TrainState.ToStation:
                LerpMove();
                if (Mathf.Abs(transform.position.x - aimedPos) < 0.5f)
                {
                    state = TrainState.WaitingForPassengers;
                    arrivingTime = Time.time;
                }

                break;

            case TrainState.WaitingForPassengers:

                if (!destinationSet)
                {
                    for (int i = 0; i < passengersBoarding.Count; i++)
                    {
                        passengersBoarding[i].navAgent.SetDestination(seats[i].position);
                    }

                    for (int i = 0; i < passengersOnBoard.Count; i++)
                    {
                        passengersOnBoard[i].navAgent.enabled = true;
                        passengersOnBoard[i].transform.parent = passengersOnBoard[i].parent.transform;
                        passengersOnBoard[i].navAgent.SetDestination(passengersOnBoard[i].InitialPosition);
                    }

                    passengersOnBoard.Clear();  //dangerous if npc reach during the loop

                    destinationSet = true;
                }

                if (Time.time > arrivingTime + stopLength && passengersOnBoard.Count >= passengersBoarding.Count)
                {
                    if(Time.time > fullTime + waitAfterFull)
                    {
                        aimedPos = leavingStationPos;
                        leavingTime = Time.time;
                        state = TrainState.LeavingStation;
                    }
                }
                break;

            case TrainState.LeavingStation:
                passengersOnBoard.ForEach(c => c.navAgent.enabled = false);

                LerpMove();

                if (leavingTime + timeBeforeRespawn < Time.time)
                {
                    Respawn();
                }
                break;

        }
	}

    void LerpMove()
    {
        newX = Mathf.Lerp(newX, aimedPos, travelLength * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void Respawn()
    {
        transform.position = initialPos;
        newX = transform.position.x;
        aimedPos = trainStopPos;
        destinationSet = false;

        /*for (int i = 0; i < passengersOnBoard.Count; i++)
        {
            passengersOnBoard[i].transform.position = seats[i].position;
        }*/

        state = TrainState.departure;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            NPCBehaviour npc = other.GetComponent<NPCBehaviour>();
            if(npc.aimedAction.gameObject.name == "Train" && !npc.actionDone)
            {
                other.GetComponent<NPCBehaviour>().actionDone = true;
                other.gameObject.transform.parent = transform;
                passengersBoarding.Remove(npc);
                passengersOnBoard.Add(npc);
            }
        }
    }
}
