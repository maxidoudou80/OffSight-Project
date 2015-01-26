using UnityEngine;
using System.Collections;

public class RemoteActivated : MonoBehaviour {

    //Play an animation
    public string animationName;

    //Change this object position
    public bool changePosition;
    public Vector3 aimedPosition;

    //Change its rotation
    public bool changeRotation;
    public Vector3 aimedRotation;

    //Make this object usable or pickable by the player
    public bool setActivable;

    //Make this character start moving
    public bool startNPCPath;

    //Draw or not this object
    public bool changeState;
    public bool aimedState;

    public void Activate()
    {
        //Launch Animation
        if (animationName != "")
            GetComponent<Animator>().Play(animationName);

        //Transform Modification
        if (changePosition)
            transform.localPosition = aimedPosition;

        if (changeRotation)
            transform.eulerAngles = aimedRotation;

        if (changeState)
        {
            GetComponent<MeshRenderer>().enabled = aimedState;
        }

        //Make item pickable / usable
        if (setActivable)
        {
            gameObject.layer = LayerMask.NameToLayer("Item");
        }

        //Make NPC move
        if(startNPCPath)
        {
            NPCPath script = GetComponent<NPCPath>();
            script.readyToMove = true;
            script.breakStart = Time.time;
        }
    }
}
