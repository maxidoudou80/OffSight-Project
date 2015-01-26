using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CameraManager cameraManagerScript = (CameraManager)target;

        if(GUILayout.Button("Capture Vignettes"))
        {
            cameraManagerScript.BeginCapture();
        }
    }
}
