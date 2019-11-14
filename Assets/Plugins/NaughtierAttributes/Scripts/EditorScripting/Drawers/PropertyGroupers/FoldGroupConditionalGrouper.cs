#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [ConditionalGrouper(typeof(FoldGroupAttribute))]
    public class FoldGroupConditionalGrouper : BaseConditionalGrouper
    {
        private readonly Dictionary<string, bool> m_unfoldCache = new Dictionary<string, bool>();

        protected override bool CanDrawImpl(SerializedProperty property, string label)
        {
            EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.button)
            {
                padding = new RectOffset(15, 0, 0, 0)
            });
            if (!m_unfoldCache.ContainsKey(property.propertyPath))
                m_unfoldCache.Add(property.propertyPath, false);

            m_unfoldCache[property.propertyPath] = EditorGUILayout.Foldout(m_unfoldCache[property.propertyPath], label, true,
                new GUIStyle(EditorStyles.foldout)
                {
                    fontStyle = FontStyle.Bold
                });
            if (m_unfoldCache[property.propertyPath]) return true;
            EditorGUILayout.EndVertical();
            return false;
        }

        public override void BeginGroup()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.textField)
            {
                margin = new RectOffset(0, 1, 0, 0),
                padding = new RectOffset(15, 0, 5, 3)
            });
        }

        protected override void EndGroupImpl(bool canDraw)
        {
            if (!canDraw) return;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif