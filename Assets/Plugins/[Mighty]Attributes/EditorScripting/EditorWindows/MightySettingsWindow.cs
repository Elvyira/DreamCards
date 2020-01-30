#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class MightySettingsWindow : EditorWindow
    {
        [MenuItem("[Mighty]Attributes/Settings", priority = 3)]
        private static void Init() => GetWindow<MightySettingsWindow>().Show();

        private void OnGUI()
        {
            titleContent = new GUIContent(EditorDrawUtility.DrawIcon(IconName.SETTINGS))
            {
                text = " Mighty Settings"
            };

            minSize = new Vector2(300, 120);

            GUILayout.Space(10);
            MightySettingsServices.Activated = EditorGUILayout.Toggle("Activated", MightySettingsServices.Activated);

            GUILayout.Space(10);
            MightySettingsServices.AutoValuesOnPlay =
                EditorGUILayout.Toggle("Auto Values On Play", MightySettingsServices.AutoValuesOnPlay);
            
            GUILayout.Space(10);
            MightySettingsServices.AutoValuesOnPlay =
                EditorGUILayout.Toggle("Auto Values On Build", MightySettingsServices.AutoValuesOnBuild);

            GUILayout.Space(10);
            if (GUILayout.Button("Apply Auto Values")) MightyAutoValues.ApplyAutoValuesAsync();
        }
    }
}
#endif