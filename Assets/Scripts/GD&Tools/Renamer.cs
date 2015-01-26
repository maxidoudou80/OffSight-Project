using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class Renamer : MonoBehaviour {

    public bool ClickToRename;
    public string prefix;
    public string suffix;
    public bool AddId;
    public bool Addprefix;
    public bool AddSuffix;
    public bool PrefixBeforeId;

    public bool ApplyNewName;
    public string NewName;

	void Update () {
        if(ClickToRename)
        {
            Debug.Log("Renaming");
            SearchAndRename(transform);
            ClickToRename = false;
        }
	}

    void SearchAndRename(Transform transform)
    {
        foreach (Transform child in transform)
            {
                if (child.gameObject.tag == "Camera")
                {
                    if (ApplyNewName)
                    {
                        child.name = NewName;
                    }

                    else
                    {
                        if (PrefixBeforeId)
                        {
                            if (AddId)
                            {
                                child.name = child.GetComponent<CameraBehaviour>().id + "-" + child.name;
                            }

                            if (Addprefix)
                            {
                                child.name = prefix + child.name;
                            }
                        }
                        else
                        {
                            if (Addprefix)
                            {
                                child.name = prefix + child.name;
                            }

                            if (AddId)
                            {
                                child.name = child.GetComponent<CameraBehaviour>().id + "-" + child.name;
                            }
                        }

                        if (AddSuffix)
                        {
                            child.name = child.name + suffix;
                        }
                    }
                }

                SearchAndRename(child.transform);
            }
    }
}
