#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace NaughtierAttributes.Editor
{
    [PropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarPropertyDrawer : BasePropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);

            if (property.propertyType != SerializedPropertyType.Float && property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUILayout.HelpBox("Field " + property.name + " is not a number", MessageType.Warning);
                return;
            }

            var value = property.propertyType == SerializedPropertyType.Integer ? property.intValue : property.floatValue;
            var valueFormatted = property.propertyType == SerializedPropertyType.Integer
                ? value.ToString()
                : String.Format("{0:0.00}", value);

            ProgressBarAttribute progressBarAttribute = PropertyUtility.GetAttribute<ProgressBarAttribute>(property);
            var position = EditorGUILayout.GetControlRect();
            var maxValue = progressBarAttribute.MaxValue;
            float lineHight = EditorGUIUtility.singleLineHeight;
            float padding = EditorGUIUtility.standardVerticalSpacing;
            var barPosition = new Rect(position.position.x, position.position.y, position.size.x, lineHight);

            var fillPercentage = value / maxValue;
            var barLabel = (!string.IsNullOrEmpty(progressBarAttribute.Name) ? "[" + progressBarAttribute.Name + "] " : "") +
                           valueFormatted + "/" + maxValue;

            var color = EditorDrawUtility.GetColor(PropertyUtility.GetTargetObject(property), progressBarAttribute.ColorName,
                progressBarAttribute.Color, Color.white);
            var color2 = Color.white;
            DrawBar(barPosition, Mathf.Clamp01(fillPercentage), barLabel, color, color2);
        }

        private void DrawBar(Rect position, float fillPercent, string label, Color barColor, Color labelColor)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            var fillRect = new Rect(position.x, position.y, position.width * fillPercent, position.height);

            EditorGUI.DrawRect(position, new Color(0.13f, 0.13f, 0.13f));
            EditorGUI.DrawRect(fillRect, barColor);

            // set alignment and cache the default
            var align = GUI.skin.label.alignment;
            GUI.skin.label.alignment = TextAnchor.UpperCenter;

            // set the color and cache the default
            var c = GUI.contentColor;
            GUI.contentColor = labelColor;

            // calculate the position
            var labelRect = new Rect(position.x, position.y - 2, position.width, position.height);

            // draw~
            EditorGUI.DropShadowLabel(labelRect, label);

            // reset color and alignment
            GUI.contentColor = c;
            GUI.skin.label.alignment = align;
        }
    }
}
#endif