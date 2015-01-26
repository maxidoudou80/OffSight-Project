using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {

    private static Interface _interface = null;

    public static Interface GetInstance()
    {
        if (_interface == null)
        {
            GameObject managers = GameObject.FindGameObjectWithTag("Managers") as GameObject;

            if (managers.GetComponent<Interface>() == null)
                managers.AddComponent<Interface>();

            _interface = managers.GetComponent<Interface>();

            Debug.Log("Creating GM : " + _interface);
        }

        return _interface;
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
