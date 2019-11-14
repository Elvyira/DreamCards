#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [DecoratorDrawer(typeof(ColorAttribute))]
    public class ColorDecoratorDrawer : BaseDecoratorDrawer
    {
        private Color _previousBackgroundColor, _previousContentColor;

        public override void BeginDraw(SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<ColorAttribute>(property);
            var target = PropertyUtility.GetTargetObject(property);

            _previousBackgroundColor = GUI.backgroundColor;
            _previousContentColor = GUI.contentColor;

            GUI.backgroundColor = EditorDrawUtility.GetColor(target, attribute.BackgroundColorName, attribute.BackgroundColor, GUI.backgroundColor);
            GUI.contentColor = EditorDrawUtility.GetColor(target, attribute.ContentColorName, attribute.ContentColor, GUI.contentColor);
        }

        public override void EndDraw(SerializedProperty property)
        {
            GUI.backgroundColor = _previousBackgroundColor;
            GUI.contentColor = _previousContentColor;
        }
    }
}
#endif