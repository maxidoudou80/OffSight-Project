using UnityEngine;
using System.Collections;

public class PickableObject : InteractiveObject
{
    public string objectName;
    public float scaleMultiplicator;
    public bool followHand{get;set;}

    public override void Use()
    {
            
        if (player.currentObject != null)
        {
            player.currentObject.transform.localScale /= player.currentObject.scaleMultiplicator;
            player.currentObject.transform.position = transform.position;
            player.currentObject.transform.rotation = transform.rotation;
            player.currentObject.followHand = false;
            player.currentObject.gameObject.layer = LayerMask.NameToLayer("Item");

        }

        player.currentObject = this;
        gameObject.layer = LayerMask.NameToLayer("Default");
        transform.localScale *= scaleMultiplicator;
        followHand = true;
    }

    void InitObject(Transform transf)
    {
        transform.localScale /= scaleMultiplicator;
    }

    void Update()
    {
        if(followHand)
        {
            transform.position = player.objectTransform.position;
            transform.rotation = player.objectTransform.rotation;
        }
    }
}
