using UnityEngine;
using System.Collections;

public class GlassBehaviour : MonoBehaviour {

    GameObject BrokenGlass;
    public float barmanReactionDelay;

	// Use this for initialization
	void Start () {
        SearchInChildren(transform);
        //BrokenGlass.SetActive(false);
	}

    void SearchInChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            switch (child.gameObject.tag)
            {
                case "BrokenGlass":
                    //BrokenGlass = child.gameObject;
                    break;
            }

            SearchInChildren(child.transform);
        }
    }

    public void TurnRedLightOff()
    {
        Invoke("TurnOffAfterDelay", barmanReactionDelay);
    }

    public void TurnOffAfterDelay()
    {
        UIElements.GetInstance().TurnYellowLightOff();
    }
}
