#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [PropertyDrawer(typeof(SliderAttribute))]
    public class SliderPropertyDrawer : BasePropertyDrawer, IArrayElementDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            if (property.isArray)
            {
                EditorDrawUtility.DrawArray(property, index => DrawElement(property, property.GetArrayElementAtIndex(index), index));
                return;
            }

            EditorDrawUtility.DrawHeader(property);

            SliderAttribute sliderAttribute = PropertyUtility.GetAttribute<SliderAttribute>(property);

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                EditorGUILayout.IntSlider(property, (int) sliderAttribute.MinValue, (int) sliderAttribute.MaxValue);
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                EditorGUILayout.Slider(property, sliderAttribute.MinValue, sliderAttribute.MaxValue);
            }
            else
            {
                EditorDrawUtility.DrawPropertyField(property);
                string warning = sliderAttribute.GetType().Name + " can be used only on int or float fields";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));
            }
        }

        public void DrawElement(SerializedProperty property, SerializedProperty element, int index)
        {
            EditorDrawUtility.DrawHeader(property);

            SliderAttribute sliderAttribute = PropertyUtility.GetAttribute<SliderAttribute>(property);

            if (element.propertyType == SerializedPropertyType.Integer)
            {
                EditorGUILayout.IntSlider(element, (int) sliderAttribute.MinValue, (int) sliderAttribute.MaxValue);
            }
            else if (element.propertyType == SerializedPropertyType.Float)
            {
                EditorGUILayout.Slider(element, sliderAttribute.MinValue, sliderAttribute.MaxValue);
            }
            else
            {
                EditorDrawUtility.DrawPropertyField(element);
                string warning = sliderAttribute.GetType().Name + " can be used only on int or float fields";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));
            }
        }

        public void DrawElement(Rect rect, SerializedProperty property, SerializedProperty element, int index)
        {
            EditorDrawUtility.DrawHeader(rect, property);

            SliderAttribute sliderAttribute = PropertyUtility.GetAttribute<SliderAttribute>(property);

            if (element.propertyType == SerializedPropertyType.Integer)
            {
                EditorGUI.IntSlider(rect, element, (int) sliderAttribute.MinValue, (int) sliderAttribute.MaxValue);
            }
            else if (element.propertyType == SerializedPropertyType.Float)
            {
                EditorGUI.Slider(rect, element, sliderAttribute.MinValue, sliderAttribute.MaxValue);
            }
            else
            {
                EditorDrawUtility.DrawPropertyField(rect, element);
                rect.y += 18;
                rect.height -= 24;
                string warning = sliderAttribute.GetType().Name + " can be used only on int or float fields";
                EditorDrawUtility.DrawHelpBox(rect, warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));
            }
        }

        public float GetElementHeight(SerializedProperty property, SerializedProperty element)
        {
            return element.propertyType != SerializedPropertyType.Integer && element.propertyType != SerializedPropertyType.Float
                ? 64
                : 20;
        }
    }
}
#endif