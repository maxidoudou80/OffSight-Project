using UnityEngine;
using System.Collections;

public class HowToButton : MonoBehaviour {

	bool activeState;


	// Use this for initialization
	void Start () 
	{
		activeState = false;
	}
	
	// Update is called once per frame
	public void ChangeStatus () 
	{
		if (gameObject.activeSelf)
			gameObject.SetActive(false);
		else
			gameObject.SetActive(true);
	}
}
