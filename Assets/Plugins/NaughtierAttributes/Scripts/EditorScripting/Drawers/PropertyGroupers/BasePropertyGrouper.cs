#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    public abstract class BasePropertyGrouper
    {
        private Color _previousBackgroundColor, _previousContentColor;

        public void BeginGroup(SerializedProperty property, string label)
        {            
            var attribute = PropertyUtility.GetAttribute<BaseGroupAttribute>(property);
            var target = PropertyUtility.GetTargetObject(property);
            
            _previousBackgroundColor = GUI.backgroundColor;
            _previousContentColor = GUI.contentColor;
            
            GUI.backgroundColor = EditorDrawUtility.GetColor(target, attribute.BackgroundColorName, attribute.BackgroundColor, GUI.backgroundColor);
            GUI.contentColor = EditorDrawUtility.GetColor(target, attribute.ContentColorName, attribute.ContentColor, GUI.contentColor);

            BeginDrawGroup(label);
        }

        public void EndGroup()
        {
            EndDrawGroup();
            GUI.backgroundColor = _previousBackgroundColor;
            GUI.contentColor = _previousContentColor;
        }

        public abstract void BeginDrawGroup(string label);

        public abstract void EndDrawGroup();
    }
}
#endif