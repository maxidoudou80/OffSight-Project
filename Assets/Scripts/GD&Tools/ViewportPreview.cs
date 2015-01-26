using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ViewportPreview : MonoBehaviour {

    CameraManager camManager;
    public bool doPreview;

	// Use this for initialization
	void Start () {
        camManager = CameraManager.GetInstance();
	}
	
	// Update is called once per frame
	/*void Update () {

        if(doPreview)
        {
            camManager.GetCurrentMain().camera.rect = camManager.mainCamRect;

            camManager.LevelCameras[camManager.currentLinkedCameras[0]].camera.rect = camManager.linkedCamRect1;
            camManager.LevelCameras[camManager.currentLinkedCameras[1]].camera.rect = camManager.linkedCamRect2;
            camManager.LevelCameras[camManager.currentLinkedCameras[2]].camera.rect = camManager.linkedCamRect3;
        }
	
	}*/
}
