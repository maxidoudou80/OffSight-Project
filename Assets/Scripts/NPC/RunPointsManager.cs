using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunPointsManager : MonoBehaviour {

    public static List<Vector3> runPoints;

	// Use this for initialization
	void Awake() {
        runPoints = new List<Vector3>();
        SearchInChildren(transform);
	}

    void SearchInChildren(Transform transf)
    {
        foreach (Transform child in transf)
        {
            if (child.gameObject.tag == "RunPoint")
            {
                runPoints.Add(child.transform.position);
            }

            SearchInChildren(child);
        }
    }

    public static IEnumerator FindShortest(NavMeshAgent agent)
    {
        Vector3 closestRunPoint = Vector3.zero;
        float pathLength = 0;

        foreach (Vector3 runP in runPoints)
        {
            agent.SetDestination(runP);
            yield return new WaitForSeconds(0.05f); ;

            if (agent.remainingDistance < pathLength || pathLength == 0)
            {

                pathLength = agent.remainingDistance;
                closestRunPoint = runP;
            }
        }

        agent.SetDestination(closestRunPoint);

        yield break;
    }
}
