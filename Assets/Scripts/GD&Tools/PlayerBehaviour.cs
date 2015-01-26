using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerBehaviour : MonoBehaviour {

    public float walkSpeed;
    public float runSpeed;
    public float backwardSpeed;
    public float minRotationSpeed;
    public float maxRotationSpeed;
    public float quickRotationSpeed;
    public float interactionRange;
    public PickableObject currentObject;         // >>> METTRE EN PROPERTY APRèS LES TESTS

    public Animator avatarAnimator { get; set; }
    public bool moving { get; set; }
    public bool running { get; set; }
    public GameObject model { get; set; }
    public GameObject moveReference { get; set; }

    private int itemLayerMask;
    public Transform objectTransform { get; set; }
    public List<Transform> detectionPoints{ get; set; }

    void Awake()
    {
        moveReference = GameObject.FindGameObjectWithTag("MoveReference");
        detectionPoints = new List<Transform>();
        SearchInChildren(transform);
    }

	// Use this for initialization
	void Start () {
        itemLayerMask = 1 << LayerMask.NameToLayer("Item");
        moveReference.transform.eulerAngles = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
	}

    void SearchInChildren(Transform transf)
    {
        foreach (Transform child in transf)
        {
            switch(child.gameObject.tag)
            {
                case "Avatar" :
                    avatarAnimator = child.GetComponent<Animator>();
                    break;

                case "DetectionPoint" :
                    detectionPoints.Add(child.transform);
                    break;

                case "Model":
                    model = child.gameObject;
                    break;

                case "ObjectTransform":
                    objectTransform = child.transform;
                    break;
            }

            SearchInChildren(child);
        }
    }

    public void moove(Vector3 vec)
    {
        transform.Translate(vec);
    }


    //A gérer dans les inputs directement
    void UpdateAnimation()
    {
        if (moving)
            avatarAnimator.SetBool("Moving", true);
        else
            avatarAnimator.SetBool("Moving", false);
    }

    public void TestSurroundingObjects()
    {
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, interactionRange, itemLayerMask);
        if (surroundingColliders.Length == 0) return;

        GameObject closestItem;
        float distance = interactionRange + 1;
        closestItem = null;
        foreach(Collider item in surroundingColliders)
        {
            if (Vector3.Distance(item.transform.position, transform.position) < distance)
            {
                distance = Vector3.Distance(item.transform.position, transform.position);
                closestItem = item.gameObject;
            }   
        }

        Debug.Log("found : " + closestItem.gameObject.name);
        closestItem.GetComponent<InteractiveObject>().Use();
    }

    void OnBecameInvisible()
    {
        Debug.Log("Invisible");
    }
    void OnBecameVisible()
    {
        Debug.Log("Visible");
    }
}
