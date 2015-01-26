using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CameraBehaviour : MonoBehaviour {

    public int id;
    public List<int> linkedCameras;
    public Sprite vignette;
    public bool TargetWatching { get; set; }
    public bool privateCamera;// { get; set; }

    // Camera Rotation
    public bool moving;
    public Vector3 rotationSpeed;
    public Vector3 maxRot;
    private Vector3 currentRot;

    // Camera Translation
    public Vector3 translateSpeed;
    public Vector3 maxMove;
    private Vector3 moved;

    string vignettePath;

    void Awake()
    {
        vignettePath = "Vignettes/" + Application.loadedLevelName + "/";
        vignette = Resources.Load<Sprite>(vignettePath + "VignetteCam" + id);

#if UNITY_EDITOR
        if (Directory.Exists("Assets/Resources/" + vignettePath))
        {
            if (File.Exists("Assets/Resources/" + vignettePath + "VignetteCam" + id + ".png"))
                vignette = Resources.Load<Sprite>(vignettePath + "VignetteCam" + id);
            else
                Debug.Log("No file named : " + "Assets\\Resources\\" + vignettePath + "VignetteCam" + id);
        }
        else
        {
            Debug.Log("No Vignette Folder For This Level");
        }
#else

        if(vignette == null)
        {
            Debug.LogError("Error when loading file : " + vignettePath + "VignetteCam" + id);
        }
#endif
    }

    void Start()
    {
        SetCameraPrivate(privateCamera);
    }

	// Update is called once per frame
	void Update () {
        if (moving)
            UpdateMovement();
	}

    public void SetTransform(Transform newTransform)
    {
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
    }

    void UpdateMovement()
    {

        ////Rotation
        if(Mathf.Abs(currentRot.x) >= maxRot.x)
        {
            rotationSpeed.x *= -1;
        }

        if (Mathf.Abs(currentRot.y) >= maxRot.y)
        {
            rotationSpeed.y *= -1;
        }

        if (Mathf.Abs(currentRot.z) >= maxRot.z)
        {
            rotationSpeed.z *= -1;
        }

        transform.eulerAngles += rotationSpeed * Time.deltaTime;
        currentRot += rotationSpeed * Time.deltaTime;

        ////Translation
        if (Mathf.Abs(moved.x) >= maxMove.x)
        {
            translateSpeed.x *= -1;
            moved.x = 0;
        }
        if (Mathf.Abs(moved.y) >= maxMove.y)
        {
            translateSpeed.y *= -1;
            moved.y = 0;
        }
        if (Mathf.Abs(moved.z) >= maxMove.z)
        {
            translateSpeed.z *= -1;
            moved.z = 0;
        }

        transform.position += translateSpeed * Time.deltaTime;
        moved += translateSpeed * Time.deltaTime; 

    }

    public void SetCameraPrivate(bool b)
    {
        privateCamera = b;

        if (b)
            camera.cullingMask = CameraManager.GetInstance().nothingLayer;
        else
            camera.cullingMask = CameraManager.GetInstance().everythingLayer;
    }
}
