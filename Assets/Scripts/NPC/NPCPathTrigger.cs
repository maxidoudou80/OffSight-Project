using UnityEngine;
using System.Collections;

public class NPCPathTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            NPCPath script = transform.root.GetComponentInChildren<NPCPath>();
            script.readyToMove = true;
            script.breakStart = Time.time;
        }
    }
}
