#if UNITY_EDITOR
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

namespace NaughtierAttributes.Editor
{
    public static class EditorDrawUtility
    {
        public static string DrawPrettyName(string name) => Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");
        
        public static void DrawHeader(string header)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        }

        public static void DrawHeader(Rect rect, string header)
        {
            EditorGUI.IndentedRect(rect);
            EditorGUI.LabelField(rect, header, EditorStyles.boldLabel);
        }

        public static bool DrawHeader(SerializedProperty property)
        {
            HeaderAttribute headerAttr = PropertyUtility.GetAttribute<HeaderAttribute>(property);
            if (headerAttr != null)
            {
                DrawHeader(headerAttr.header);
                return true;
            }

            return false;
        }

        public static bool DrawHeader(Rect rect, SerializedProperty property)
        {
            HeaderAttribute headerAttr = PropertyUtility.GetAttribute<HeaderAttribute>(property);
            if (headerAttr != null)
            {
                DrawHeader(rect, headerAttr.header);
                return true;
            }

            return false;
        }

        public static void DrawHelpBox(string message, MessageType type, bool logToConsole = false, Object context = null)
        {
            EditorGUILayout.HelpBox(message, type);

            if (logToConsole)
            {
                switch (type)
                {
                    case MessageType.None:
                    case MessageType.Info:
                        Debug.Log(message, context);
                        break;
                    case MessageType.Warning:
                        Debug.LogWarning(message, context);
                        break;
                    case MessageType.Error:
                        Debug.LogError(message, context);
                        break;
                }
            }
        }

        public static void DrawHelpBox(Rect rect, string message, MessageType type, bool logToConsole = false,
            Object context = null)
        {
            EditorGUI.HelpBox(rect, message, type);

            if (logToConsole)
            {
                switch (type)
                {
                    case MessageType.None:
                    case MessageType.Info:
                        Debug.Log(message, context);
                        break;
                    case MessageType.Warning:
                        Debug.LogWarning(message, context);
                        break;
                    case MessageType.Error:
                        Debug.LogError(message, context);
                        break;
                }
            }
        }

        public static Color GetColor(Object target, string colorName, ColorValue colorValue, Color defaultColor)
        {
            return GetColor(target, colorName, GetColor(colorValue, defaultColor));
        }

        public static Color GetColor(Object target, string colorName, Color defaultColor)
        {
            Color color;
            if (SerializedPropertyUtility.GetValueFromTypeInfo(target, colorName, out color) ||
                ColorUtility.TryParseHtmlString(colorName, out color))
            {
                return color;
            }

            return defaultColor;
        }

        public static Color GetColor(ColorValue color, Color defaultColor)
        {
            switch (color)
            {
                case ColorValue.Red:
                    return new Color32(255, 0, 63, 255);
                case ColorValue.Pink:
                    return new Color32(255, 152, 203, 255);
                case ColorValue.Orange:
                    return new Color32(255, 128, 0, 255);
                case ColorValue.Yellow:
                    return new Color32(255, 211, 0, 255);
                case ColorValue.Green:
                    return new Color32(102, 255, 0, 255);
                case ColorValue.Blue:
                    return new Color32(0, 135, 189, 255);
                case ColorValue.Indigo:
                    return new Color32(75, 0, 130, 255);
                case ColorValue.Violet:
                    return new Color32(127, 0, 255, 255);
                case ColorValue.White:
                    return Color.white;
                default:
                    return defaultColor;
            }
        }


        public static void DrawWithAlign(Align align, Action drawCallback)
        {
            GUILayout.BeginHorizontal();
            switch (align)
            {
                case Align.Left:
                    GUILayout.Space(EditorGUI.indentLevel * 20);
                    drawCallback.Invoke();
                    break;
                case Align.Center:
                    GUILayout.FlexibleSpace();
                    drawCallback.Invoke();
                    GUILayout.FlexibleSpace();
                    break;
                case Align.Right:
                    GUILayout.FlexibleSpace();
                    drawCallback.Invoke();
                    break;
            }

            GUILayout.EndHorizontal();
        }

        public static void BeginDrawAlign(Align align)
        {
            GUILayout.BeginHorizontal();
            switch (align)
            {
                case Align.Left:
                    GUILayout.Space(EditorGUI.indentLevel * 20);
                    break;
                case Align.Center:
                    GUILayout.FlexibleSpace();
                    break;
                case Align.Right:
                    GUILayout.FlexibleSpace();
                    break;
            }
        }

        public static void EndDrawAlign(Align align)
        {
            if (align == Align.Center)
                GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
        }


        public static bool DrawArrayHeader(SerializedProperty property)
        {
            if (!property.isArray) return false;

            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, property.displayName, true);
            return property.isExpanded;
        }

        public static bool DrawArrayHeader(Rect rect, SerializedProperty property)
        {
            if (!property.isArray) return false;

            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, property.displayName, true);
            return property.isExpanded;
        }

        public delegate void ElementDrawer(int index);

        public static void DrawArraySizeField(SerializedProperty property)
        {
            if (property.isArray && property.isExpanded)
                property.arraySize = Mathf.Max(0, EditorGUILayout.DelayedIntField("Size", property.arraySize));
        }

        public static void DrawArraySizeField(Rect rect, SerializedProperty property)
        {
            if (property.isArray && property.isExpanded)
                property.arraySize = Mathf.Max(0, EditorGUI.DelayedIntField(rect, "Size", property.arraySize));
        }

        public static void DrawArrayBody(SerializedProperty property, ElementDrawer elementDrawer)
        {
            if (!property.isArray || !property.isExpanded) return;

            for (var i = 0; i < property.arraySize; i++)
                elementDrawer(i);
        }

        public static void DrawArrayBody(Rect rect, SerializedProperty property, ElementDrawer elementDrawer)
        {
            if (!property.isArray || !property.isExpanded) return;

            for (var i = 0; i < property.arraySize; i++)
                elementDrawer(i);
        }

        public static void DrawArray(SerializedProperty property, ElementDrawer elementDrawer)
        {
            if (!property.isArray) return;

            if (!DrawArrayHeader(property)) return;

            EditorGUI.indentLevel++;
            DrawArraySizeField(property);
            DrawArrayBody(property, elementDrawer);
            EditorGUI.indentLevel--;
        }

        public static void DrawArray(Rect rect, SerializedProperty property, ElementDrawer elementDrawer)
        {
            if (!property.isArray) return;

            if (!DrawArrayHeader(rect, property)) return;

            rect.x += 15;
            rect.width -= 15;
            DrawArraySizeField(rect, property);
            DrawArrayBody(rect, property, elementDrawer);
        }

        public static void DrawPropertyField(SerializedProperty property, bool includeChildren = true)
        {
            EditorGUILayout.PropertyField(property, includeChildren);
        }

        public static void DrawPropertyField(Rect rect, SerializedProperty property, bool includeChildren = true)
        {
            EditorGUI.PropertyField(rect, property, includeChildren);
        }

        public static bool DrawLayoutField(object value, string label)
        {
            GUI.enabled = false;

            bool isDrawn = true;
            Type valueType = value.GetType();

            if (valueType == typeof(bool))
            {
                EditorGUILayout.Toggle(label, (bool) value);
            }
            else if (valueType == typeof(int))
            {
                EditorGUILayout.IntField(label, (int) value);
            }
            else if (valueType == typeof(long))
            {
                EditorGUILayout.LongField(label, (long) value);
            }
            else if (valueType == typeof(float))
            {
                EditorGUILayout.FloatField(label, (float) value);
            }
            else if (valueType == typeof(double))
            {
                EditorGUILayout.DoubleField(label, (double) value);
            }
            else if (valueType == typeof(string))
            {
                EditorGUILayout.TextField(label, (string) value);
            }
            else if (valueType == typeof(Vector2))
            {
                EditorGUILayout.Vector2Field(label, (Vector2) value);
            }
            else if (valueType == typeof(Vector3))
            {
                EditorGUILayout.Vector3Field(label, (Vector3) value);
            }
            else if (valueType == typeof(Vector4))
            {
                EditorGUILayout.Vector4Field(label, (Vector4) value);
            }
            else if (valueType == typeof(Color))
            {
                EditorGUILayout.ColorField(label, (Color) value);
            }
            else if (valueType == typeof(Bounds))
            {
                EditorGUILayout.BoundsField(label, (Bounds) value);
            }
            else if (valueType == typeof(Rect))
            {
                EditorGUILayout.RectField(label, (Rect) value);
            }
            else if (typeof(Object).IsAssignableFrom(valueType))
            {
                EditorGUILayout.ObjectField(label, (Object) value, valueType, true);
            }
            else
            {
                isDrawn = false;
            }

            GUI.enabled = true;

            return isDrawn;
        }
    }
}
#endif