using UnityEngine;
using System.Collections;

public abstract class InteractiveObject : MonoBehaviour
{
    protected PlayerBehaviour player;

	void Start () {
        player = GameManager.GetInstance().player;
	}

    public virtual void Use(){}
}
