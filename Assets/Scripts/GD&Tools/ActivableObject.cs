using UnityEngine;
using System.Collections;

public class ActivableObject : InteractiveObject
{
    // Does it need a key a particular state
    public ActivationType activationType;
    public enum ActivationType { Self_Activated, Key_Activated, State_Activated };

    //What does it need and where will it be placed
    public string objectNeeded;
    public Transform keyTransform;

    // Play an animation
    public string animationName;

    //Which object does it call ?
    public RemoteActivated[] remoteActivatedObjects;


    //Obsolete ?
    private UIElements uiElements;

    void Start()
    {
        uiElements = UIElements.GetInstance();
    }
    // // // // //


    public override void Use()
    {
        Debug.Log("used : " + name);
        switch(activationType)
        {
            case ActivationType.Key_Activated:

                if (player.currentObject && objectNeeded == player.currentObject.objectName)
                {
                    player.currentObject.transform.SetParent(null);
                    player.currentObject.followHand = false;
                    player.currentObject.transform.localScale /= player.currentObject.scaleMultiplicator;
                    player.currentObject.gameObject.transform.position = keyTransform.position;
                    player.currentObject.gameObject.transform.rotation = keyTransform.rotation;
                    player.currentObject.gameObject.layer = LayerMask.NameToLayer("Default");
                    player.currentObject = null;
                    gameObject.layer = LayerMask.NameToLayer("Default");
                    DoAction();
                }

                break;

            case ActivationType.Self_Activated:
                DoAction();

                break;
        }

        //return false;
    }

    void DoAction()
    {
        if (animationName != "")
            GetComponent<Animator>().Play(animationName);

        foreach(RemoteActivated target in remoteActivatedObjects)
        {
            target.Activate();
        }
    }
}