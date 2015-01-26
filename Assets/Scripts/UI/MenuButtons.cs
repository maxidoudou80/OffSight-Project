using UnityEngine;
using System.Collections;

public class MenuButtons : MonoBehaviour {

	public void ChangeLevel(string levelToLoad)
	{
		Application.LoadLevel (levelToLoad);
	}

	public void CloseGame ()
	{
		Application.Quit ();
	}
}
