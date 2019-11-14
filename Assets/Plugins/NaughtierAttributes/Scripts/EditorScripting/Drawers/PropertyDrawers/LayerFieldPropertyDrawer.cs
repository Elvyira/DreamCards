#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [PropertyDrawer(typeof(LayerFieldAttribute))]
    public class LayerFieldPropertyDrawer : BasePropertyDrawer, IArrayElementDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            if (property.isArray)
            {
                EditorDrawUtility.DrawArray(property, index => DrawElement(property, property.GetArrayElementAtIndex(index), index));
                return;
            }
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                EditorDrawUtility.DrawPropertyField(property);
                EditorDrawUtility.DrawHelpBox("\"" + property.displayName + "\" should be of type int",
                    MessageType.Warning, true, PropertyUtility.GetTargetObject(property));
                return;
            }

            property.intValue = EditorGUILayout.LayerField(property.displayName, property.intValue);
        }

        public void DrawElement(SerializedProperty property, SerializedProperty element, int index)
        {            
            if (element.propertyType != SerializedPropertyType.Integer)
            {
                EditorDrawUtility.DrawPropertyField(element);
                EditorDrawUtility.DrawHelpBox("\"" + element.displayName + "\" should be of type int",
                    MessageType.Warning, true, PropertyUtility.GetTargetObject(property));
                return;
            }

            element.intValue = EditorGUILayout.LayerField(element.displayName, element.intValue);
        }

        public void DrawElement(Rect rect, SerializedProperty property, SerializedProperty element, int index)
        {
            if (element.propertyType != SerializedPropertyType.Integer)
            {
                EditorDrawUtility.DrawPropertyField(rect, element);
                rect.y += 18;
                rect.height -= 24;
                EditorDrawUtility.DrawHelpBox(rect, "\"" + element.displayName + "\" should be of type int",
                    MessageType.Warning, true, PropertyUtility.GetTargetObject(property));
                return;
            }

            element.intValue = EditorGUI.LayerField(rect, element.displayName, element.intValue);
        }

        public float GetElementHeight(SerializedProperty property, SerializedProperty element)
        {            
            return element.propertyType != SerializedPropertyType.Integer
                ? 64
                : 20;
        }
    }
}
#endif