using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CamSelectionButton : MonoBehaviour {

    CameraSelectionScreen camSelect;
    ColorBlock linkedColorBlock;
    ColorBlock normalColorBlock;
    Button button;
    public int linkedCamId;
    public int pageId;

    void Start()
    {
        camSelect = CameraSelectionScreen.GetInstance();
        button = GetComponent<Button>();
        normalColorBlock = button.colors;
        linkedColorBlock = new ColorBlock();
        linkedColorBlock.normalColor = Color.cyan;
        linkedColorBlock.colorMultiplier = 1;
    }

    void Update()
    {
        if(camSelect.canvas.enabled && Input.anyKey)
            CheckIfLinked();
    }

    void OnMouseDown()
    {
        Debug.Log("cam swap : " + linkedCamId);
        CameraManager.GetInstance().SetMainCamera(linkedCamId);
    }

    void CheckIfLinked()
    {
        int currentSelectedCamID = camSelect.eventSystem.currentSelectedGameObject.GetComponent<CamSelectionButton>().linkedCamId;
        CameraBehaviour currentSelectedCam = CameraManager.GetInstance().LevelCameras[currentSelectedCamID];

        if (currentSelectedCam.linkedCameras.Contains(linkedCamId))
        {
            button.colors = linkedColorBlock;
        }
        else
        {
            button.colors = normalColorBlock;
        }
    }
}
