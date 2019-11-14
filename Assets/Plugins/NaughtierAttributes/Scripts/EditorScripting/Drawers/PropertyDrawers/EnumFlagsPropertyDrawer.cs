#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [PropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsPropertyDrawer : BasePropertyDrawer, IArrayElementDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            if (property.isArray)
            {
                EditorDrawUtility.DrawArray(property, index => DrawElement(property, property.GetArrayElementAtIndex(index), index));
                return;
            }

            if (property.propertyType != SerializedPropertyType.Enum)
            {
                EditorDrawUtility.DrawPropertyField(property);
                EditorDrawUtility.DrawHelpBox("\"" + property.displayName + "\" type should be an enum",
                    MessageType.Warning, true, PropertyUtility.GetTargetObject(property));
                return;
            }

            property.intValue = EditorGUILayout.MaskField(property.displayName, property.intValue, property.enumNames);

            if (property.GetPropertyType().GetCustomAttributes(typeof(FlagsAttribute), true).Length == 0)
                EditorDrawUtility.DrawHelpBox("Enum \"" + property.displayName + "\" is not marked by [Flags] attribute",
                    MessageType.Warning, true, PropertyUtility.GetTargetObject(property));
        }

        public void DrawElement(SerializedProperty property, SerializedProperty element, int index)
        {
            if (element.propertyType != SerializedPropertyType.Enum)
            {
                EditorDrawUtility.DrawPropertyField(element);
                EditorDrawUtility.DrawHelpBox("\"" + element.displayName + "\" type should be an enum",
                    MessageType.Warning, true, PropertyUtility.GetTargetObject(property));
                return;
            }

            element.intValue = EditorGUILayout.MaskField(element.displayName, element.intValue, element.enumNames);

            if (property.GetPropertyType().GetCustomAttributes(typeof(FlagsAttribute), true).Length == 0)
                EditorDrawUtility.DrawHelpBox("Enum \"" + element.displayName + "\" is not marked by [Flags] attribute",
                    MessageType.Warning, true, PropertyUtility.GetTargetObject(property));
        }

        public void DrawElement(Rect rect, SerializedProperty property, SerializedProperty element, int index)
        {
            if (element.propertyType != SerializedPropertyType.Enum)
            {
                EditorDrawUtility.DrawPropertyField(rect, element);
                rect.y += 18;
                rect.height -= 24;
                EditorDrawUtility.DrawHelpBox(rect, "\"" + element.displayName + "\" type should be an enum",
                    MessageType.Warning, true, PropertyUtility.GetTargetObject(property));
                return;
            }

            element.intValue = EditorGUI.MaskField(rect, element.displayName, element.intValue, element.enumNames);

            if (property.GetPropertyType().GetCustomAttributes(typeof(FlagsAttribute), true).Length != 0) return;
            
            rect.y += 18;
            rect.height -= 24;
            EditorDrawUtility.DrawHelpBox(rect, "Enum \"" + element.displayName + "\" is not marked by [Flags] attribute",
                MessageType.Warning, true, PropertyUtility.GetTargetObject(property));
        }

        public float GetElementHeight(SerializedProperty property, SerializedProperty element)
        {
            return element.propertyType != SerializedPropertyType.Enum ||
                   property.GetPropertyType().GetCustomAttributes(typeof(FlagsAttribute), true).Length == 0
                ? 64
                : 20;
        }
    }
}
#endif