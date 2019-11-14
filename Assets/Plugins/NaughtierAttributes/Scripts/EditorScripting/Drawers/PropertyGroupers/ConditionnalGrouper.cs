#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    public abstract class BaseConditionalGrouper
    {
        private Color _previousBackgroundColor, _previousContentColor;
        
        public bool CanDraw(SerializedProperty property, string label)
        {
            var attribute = PropertyUtility.GetAttribute<BaseConditionalGroupAttribute>(property);
            var target = PropertyUtility.GetTargetObject(property);

            _previousBackgroundColor = GUI.backgroundColor;
            _previousContentColor = GUI.contentColor;

            GUI.backgroundColor = EditorDrawUtility.GetColor(target, attribute.BackgroundColorName, attribute.BackgroundColor, GUI.backgroundColor);
            GUI.contentColor = EditorDrawUtility.GetColor(target, attribute.ContentColorName, attribute.ContentColor, GUI.contentColor);

            return CanDrawImpl(property, label);
        }

        public void EndGroup(bool canDraw)
        {
            EndGroupImpl(canDraw);
            GUI.backgroundColor = _previousBackgroundColor;
            GUI.contentColor = _previousContentColor;
        }

        protected abstract bool CanDrawImpl(SerializedProperty property, string label);
        
        public abstract void BeginGroup();

        protected abstract void EndGroupImpl(bool canDraw);
    }
}
#endif