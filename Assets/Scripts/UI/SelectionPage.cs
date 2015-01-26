using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionPage : MonoBehaviour {

    public int numberOfCam;
    public int lineNumber;
    public RectTransform rectTrans;

	// Use this for initialization
	void Awake () {
        rectTrans = GetComponent<RectTransform>();
	}

    void Start()
    {
    }

    public void Initialize(int pageNumber, float gap, Vector2 startPos)
    {
        rectTrans.anchorMin = new Vector2(0, 0);
        rectTrans.anchorMax = new Vector2(1, 1);
        rectTrans.pivot = new Vector2(0, 1);
        rectTrans.anchoredPosition = new Vector2(gap * pageNumber + startPos.x, startPos.y);
    }
}
