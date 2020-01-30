#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    [DrawerTarget(typeof(BoxGroupAttribute), typeof(BoxAttribute))]
    public class BoxGroupPropertyGrouper : BasePropertyGrouper
    {
        public override void BeginDrawGroup(string label, bool drawName, int indentLevel)
        {
            GUILayout.BeginVertical(GUIStyleUtility.LightBox(indentLevel));
            EditorGUI.indentLevel++;
            if (drawName && !string.IsNullOrEmpty(label)) EditorGUILayout.LabelField(label, GUIStyleUtility.BoxGroupLabel);
        }

        public override void EndDrawGroup(int indentLevel)
        {
            EditorGUI.indentLevel = indentLevel;
            GUILayout.EndVertical();
        }
    }

    [DrawerTarget(typeof(DarkBoxGroupAttribute), typeof(DarkBoxAttribute))]
    public class DarkBoxGroupPropertyGrouper : BasePropertyGrouper
    {
        public override void BeginDrawGroup(string label, bool drawName, int indentLevel)
        {
            GUILayout.BeginVertical(GUIStyleUtility.DarkBox(indentLevel));
            EditorGUI.indentLevel++;
            if (drawName && !string.IsNullOrEmpty(label)) EditorGUILayout.LabelField(label, GUIStyleUtility.BoxGroupLabel);
        }

        public override void EndDrawGroup(int indentLevel)
        {
            EditorGUI.indentLevel = indentLevel;
            GUILayout.EndVertical();
        }
    }
}
#endif