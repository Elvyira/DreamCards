#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [PropertyDrawer(typeof(ResizableTextAreaAttribute))]
    public class ResizableTextAreaPropertyDrawer : BasePropertyDrawer, IArrayElementDrawer
    {
        private float Height(SerializedProperty property)
        {
            var linesCount = property.stringValue.Split('\n').Length;
            return (linesCount > 3 ? linesCount : 3) * 13 + 3;
        }

        public override void DrawProperty(SerializedProperty property)
        {
            if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                EditorDrawUtility.DrawArray(property, index => DrawElement(property, property.GetArrayElementAtIndex(index), index));
                return;
            }

            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUILayout.LabelField(property.displayName);

                EditorGUI.BeginChangeCheck();

                string textAreaValue =
                    EditorGUILayout.TextArea(property.stringValue, GUILayout.MinHeight(Height(property)));

                if (EditorGUI.EndChangeCheck())
                    property.stringValue = textAreaValue;
            }
            else
            {
                EditorDrawUtility.DrawPropertyField(property);
                string warning = PropertyUtility.GetAttribute<ResizableTextAreaAttribute>(property).GetType().Name +
                                 " can only be used on string fields";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));
            }
        }

        public void DrawElement(SerializedProperty property, SerializedProperty element, int index)
        {
            if (element.propertyType == SerializedPropertyType.String)
            {
                EditorGUILayout.LabelField(element.displayName);

                EditorGUI.BeginChangeCheck();

                string textAreaValue =
                    EditorGUILayout.TextArea(element.stringValue, GUILayout.MinHeight(Height(element)));

                if (EditorGUI.EndChangeCheck())
                    element.stringValue = textAreaValue;
            }
            else
            {
                EditorDrawUtility.DrawPropertyField(element);
                string warning = PropertyUtility.GetAttribute<ResizableTextAreaAttribute>(property).GetType().Name +
                                 " can only be used on string fields";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));
            }
        }

        public void DrawElement(Rect rect, SerializedProperty property, SerializedProperty element, int index)
        {
            if (element.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.LabelField(rect, element.displayName);

                EditorGUI.BeginChangeCheck();

                string textAreaValue = EditorGUI.TextArea(new Rect(rect.x, rect.y + 18, rect.width, Height(element)), element.stringValue);

                if (EditorGUI.EndChangeCheck())
                    element.stringValue = textAreaValue;
            }
            else
            {
                EditorDrawUtility.DrawPropertyField(rect, element);
                rect.y += 18;
                rect.height -= 24;
                string warning = PropertyUtility.GetAttribute<ResizableTextAreaAttribute>(property).GetType().Name +
                                 " can only be used on string fields";
                EditorDrawUtility.DrawHelpBox(rect, warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));
            }
        }

        public float GetElementHeight(SerializedProperty property, SerializedProperty element)
        {
            return element.propertyType != SerializedPropertyType.String
                ? 64
                : Height(element) + 24;
        }
    }
}
#endif