using UnityEngine;
using System.Collections;

public class LevelEndCollider : MonoBehaviour {

    public string nextLevel;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            Application.LoadLevel(nextLevel);
    }
}