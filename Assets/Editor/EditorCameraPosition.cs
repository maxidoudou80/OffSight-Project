using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CameraBehaviour))]
public class EditorCameraPosition : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CameraBehaviour cameraScript = (CameraBehaviour)target;

        if(GUILayout.Button("Copy Editor View"))
        {
            cameraScript.SetTransform(SceneView.currentDrawingSceneView.camera.transform);
        }
    }
}
