#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public abstract class BaseEditorWindow : EditorWindow
{
    protected void DrawScriptFeatures()
    {
        GUILayout.Space(5);
        DrawUtility.DrawVertical(() =>
        {
            GUILayout.Space(5);
            DrawUtility.DrawHorizontal(() =>
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Script features", EditorStyles.boldLabel);
            });
            GUILayout.Space(5);
            DrawUtility.DrawHorizontal(() =>
            {
                GUILayout.Space(10);
                var script = MonoScript.FromScriptableObject(this);
                DrawUtility.DrawButton(IconName.EYE, "Focus Script", () => EditorGUIUtility.PingObject(script), 
                    GUILayout.Width(110), GUILayout.Height(20));
                DrawUtility.DrawButton(IconName.CS_SCRIPT_ICON, "Open Script", () => AssetDatabase.OpenAsset(script), 
                    GUILayout.Width(110), GUILayout.Height(20));
                DrawUtility.DrawButton(IconName.REFRESH, "Refresh Window", InitWindow, GUILayout.Width(130), GUILayout.Height(20));
                GUILayout.FlexibleSpace();
                GUILayout.Space(10);
            });
            GUILayout.Space(10);
        }, GUI.skin.textField);
    }

    protected abstract void InitWindow();
}
#endif