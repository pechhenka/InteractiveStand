using UnityEngine;
using UnityEditor;

namespace Stand
{
    // Позволяет сделать скриншот с камеры при помощи соответствующей кнопке на компоненте Camera
    [CustomEditor(typeof(Camera))]
    public class CameraScreenshot : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Camera myScript = (Camera)target;
            if (GUILayout.Button("Screenshot"))
                ScreenCapture.CaptureScreenshot("Screenshot.png");
        }
    }
}