#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [PropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderPropertyDrawer : BasePropertyDrawer, IArrayElementDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            if (property.isArray)
            {
                EditorDrawUtility.DrawArray(property, index => DrawElement(property, property.GetArrayElementAtIndex(index), index));
                return;
            }

            EditorDrawUtility.DrawHeader(property);

            MinMaxSliderAttribute minMaxSliderAttribute = PropertyUtility.GetAttribute<MinMaxSliderAttribute>(property);

            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                Draw(EditorGUILayout.GetControlRect(), property, minMaxSliderAttribute);
            }
            else
            {
                EditorDrawUtility.DrawPropertyField(property);
                string warning = minMaxSliderAttribute.GetType().Name + " can be used only on Vector2 fields";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));
            }
        }

        public void DrawElement(SerializedProperty property, SerializedProperty element, int index)
        {
            EditorDrawUtility.DrawHeader(property);

            MinMaxSliderAttribute minMaxSliderAttribute = PropertyUtility.GetAttribute<MinMaxSliderAttribute>(property);

            if (element.propertyType == SerializedPropertyType.Vector2)
            {
                Draw(EditorGUILayout.GetControlRect(), element, minMaxSliderAttribute);
            }
            else
            {
                EditorDrawUtility.DrawPropertyField(element);
                string warning = minMaxSliderAttribute.GetType().Name + " can be used only on Vector2 fields";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));
            }
        }

        public void DrawElement(Rect rect, SerializedProperty property, SerializedProperty element, int index)
        {
            EditorDrawUtility.DrawHeader(rect, property);

            MinMaxSliderAttribute minMaxSliderAttribute = PropertyUtility.GetAttribute<MinMaxSliderAttribute>(property);

            if (element.propertyType == SerializedPropertyType.Vector2)
            {
                Draw(new Rect(rect.x, rect.y, rect.width, rect.height - 5), element, minMaxSliderAttribute);
            }
            else
            {
                EditorDrawUtility.DrawPropertyField(rect, element);
                rect.y += 18;
                rect.height -= 26;
                string warning = minMaxSliderAttribute.GetType().Name + " can be used only on Vector2 fields";
                EditorDrawUtility.DrawHelpBox(rect, warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));
            }
        }

        public float GetElementHeight(SerializedProperty property, SerializedProperty element)
        {
            return element.propertyType != SerializedPropertyType.Vector2
                ? 66
                : 22;
        }

        private void Draw(Rect rect, SerializedProperty property, MinMaxSliderAttribute minMaxSliderAttribute)
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            float floatFieldWidth = EditorGUIUtility.fieldWidth;
            float sliderWidth = rect.width - labelWidth - 2f * floatFieldWidth;
            float sliderPadding = 5f;

            Rect labelRect = new Rect(
                rect.x,
                rect.y,
                labelWidth,
                rect.height);

            Rect sliderRect = new Rect(
                rect.x + labelWidth + floatFieldWidth + sliderPadding,
                rect.y,
                sliderWidth - 2f * sliderPadding,
                rect.height);

            Rect minFloatFieldRect = new Rect(
                rect.x + labelWidth,
                rect.y,
                floatFieldWidth,
                rect.height);

            Rect maxFloatFieldRect = new Rect(
                rect.x + labelWidth + floatFieldWidth + sliderWidth,
                rect.y,
                floatFieldWidth,
                rect.height);

            // Draw the label
            EditorGUI.LabelField(labelRect, property.displayName);

            // Draw the slider
            EditorGUI.BeginChangeCheck();

            Vector2 sliderValue = property.vector2Value;
            EditorGUI.MinMaxSlider(sliderRect, ref sliderValue.x, ref sliderValue.y, minMaxSliderAttribute.MinValue,
                minMaxSliderAttribute.MaxValue);

            sliderValue.x = EditorGUI.FloatField(minFloatFieldRect, sliderValue.x);
            sliderValue.x = Mathf.Clamp(sliderValue.x, minMaxSliderAttribute.MinValue,
                Mathf.Min(minMaxSliderAttribute.MaxValue, sliderValue.y));

            sliderValue.y = EditorGUI.FloatField(maxFloatFieldRect, sliderValue.y);
            sliderValue.y = Mathf.Clamp(sliderValue.y, Mathf.Max(minMaxSliderAttribute.MinValue, sliderValue.x),
                minMaxSliderAttribute.MaxValue);

            if (EditorGUI.EndChangeCheck())
            {
                property.vector2Value = sliderValue;
            }
        }
    }
}
#endif