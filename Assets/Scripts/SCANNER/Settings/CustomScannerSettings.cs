using System.Linq;
using MightyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class CustomScannerSettings : MonoBehaviour
{
    [CustomDrawer("WebcamNameDrawer")] public string webcamName;

    public void Init()
    {
#if !UNITY_EDITOR
        webcamName = WebCamTexture.devices[0].name;
#endif
    }

#if UNITY_EDITOR
    private void WebcamNameDrawer(SerializedProperty property)
    {
        var devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            property.stringValue = "";
            return;
        }

        var devicesNames = devices.Select(t => t.name).ToArray();

        var index = 0;
        for (var i = 0; i < devicesNames.Length; i++)
            if (devicesNames[i] == property.stringValue)
                index = i;


        index = EditorGUILayout.Popup("Webcam", index, devicesNames);

        property.stringValue = devicesNames[index];
    }
#endif
}