#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace NaughtierAttributes.Editor
{
    [PropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownPropertyDrawer : BasePropertyDrawer, IArrayElementDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            if (property.isArray)
            {
                EditorDrawUtility.DrawArray(property, index => DrawElement(property, property.GetArrayElementAtIndex(index), index));
                return;
            }
            
            EditorDrawUtility.DrawHeader(property);

            DropdownAttribute dropdownAttribute = PropertyUtility.GetAttribute<DropdownAttribute>(property);
            Object target = PropertyUtility.GetTargetObject(property);

            FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);
            FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, dropdownAttribute.ValuesFieldName);

            var value = Draw(valuesFieldInfo, fieldInfo, property, fieldInfo.GetValue(target), dropdownAttribute, target);
            if (value == null) return;
            fieldInfo.SetValue(target, value);
        }

        private object Draw(FieldInfo valuesFieldInfo, FieldInfo fieldInfo, SerializedProperty property, object selectedValue,
            DropdownAttribute dropdownAttribute, Object target)
        {
            if (valuesFieldInfo == null)
            {
                EditorDrawUtility.DrawPropertyField(property);
                EditorDrawUtility.DrawHelpBox(string.Format("{0} cannot find a values field with name \"{1}\"",
                    dropdownAttribute.GetType().Name, dropdownAttribute.ValuesFieldName), MessageType.Warning, true, target);
                return null;
            }

            int selectedValueIndex;
            object[] values;
            string[] displayOptions;
            if (GetDisplayOptions(valuesFieldInfo, fieldInfo, target, selectedValue, out displayOptions, out values, out selectedValueIndex))
                return DrawDropdown(target, fieldInfo, property.displayName, selectedValueIndex, values, displayOptions);

            EditorDrawUtility.DrawPropertyField(property);
            EditorDrawUtility.DrawHelpBox(
                typeof(DropdownAttribute).Name + " works only when the type of the field is equal to the element type of the array",
                MessageType.Warning, true, target);

            return null;
        }


        private object Draw(Rect rect, FieldInfo valuesFieldInfo, FieldInfo fieldInfo, SerializedProperty property, object selectedValue,
            DropdownAttribute dropdownAttribute, Object target)
        {
            if (valuesFieldInfo == null)
            {
                EditorDrawUtility.DrawPropertyField(rect, property);
                rect.y += 18;
                rect.height -= 24;
                EditorDrawUtility.DrawHelpBox(rect, string.Format("{0} cannot find a values field with name \"{1}\"",
                    dropdownAttribute.GetType().Name, dropdownAttribute.ValuesFieldName), MessageType.Warning, true, target);
                return null;
            }

            int selectedValueIndex;
            object[] values;
            string[] displayOptions;

            if (GetDisplayOptions(valuesFieldInfo, fieldInfo, target, selectedValue, out displayOptions, out values,
                out selectedValueIndex))
                return DrawDropdown(rect, target, fieldInfo, property.displayName, selectedValueIndex, values, displayOptions);

            EditorDrawUtility.DrawPropertyField(rect, property);
            rect.y += 18;
            rect.height -= 24;
            EditorDrawUtility.DrawHelpBox(rect,
                string.Format("{0} works only when the type of the field is equal to the element type of the array",
                    typeof(DropdownAttribute).Name), MessageType.Warning, true, target);

            return null;
        }

        private bool GetDisplayOptions(FieldInfo valuesFieldInfo, FieldInfo fieldInfo, Object target, object selectedValue,
            out string[] displayOptions,
            out object[] values, out int selectedValueIndex)
        {
            selectedValueIndex = -1;
            values = null;
            displayOptions = null;

            if (valuesFieldInfo.GetValue(target) is IList &&
                (fieldInfo.FieldType == GetElementType(valuesFieldInfo) ||
                 fieldInfo.FieldType.GetElementType() == GetElementType(valuesFieldInfo)))
            {
                displayOptions = GetDisplayOptions((IList) valuesFieldInfo.GetValue(target), selectedValue, out values,
                    out selectedValueIndex);
                return true;
            }

            if (valuesFieldInfo.GetValue(target) is IDropdownList)
            {
                displayOptions = GetDisplayOptions(((IDropdownList) valuesFieldInfo.GetValue(target)).GetEnumerator(),
                    fieldInfo.GetValue(target), out values, out selectedValueIndex);
                return true;
            }

            return false;
        }

        private string[] GetDisplayOptions(IList valuesList, object selectedValue, out object[] values, out int selectedValueIndex)
        {
            values = new object[valuesList.Count];
            string[] displayOptions = new string[valuesList.Count];

            for (int i = 0; i < values.Length; i++)
            {
                object value = valuesList[i];
                values[i] = value;
                displayOptions[i] = value.ToString();
            }

            // Selected value index
            selectedValueIndex = Array.IndexOf(values, selectedValue);
            if (selectedValueIndex < 0)
                selectedValueIndex = 0;

            return displayOptions;
        }

        private string[] GetDisplayOptions(IEnumerator<KeyValuePair<string, object>> dropdownEnumerator, object selectedValue,
            out object[] values, out int selectedValueIndex)
        {
            selectedValueIndex = -1;
            int index = -1;
            List<object> valuesList = new List<object>();
            List<string> displayOptions = new List<string>();

            while (dropdownEnumerator.MoveNext())
            {
                index++;

                KeyValuePair<string, object> current = dropdownEnumerator.Current;
                if (current.Value.Equals(selectedValue))
                    selectedValueIndex = index;

                valuesList.Add(current.Value);
                displayOptions.Add(current.Key);
            }

            if (selectedValueIndex < 0)
                selectedValueIndex = 0;
            values = valuesList.ToArray();
            return displayOptions.ToArray();
        }

        private Type GetElementType(FieldInfo listFieldInfo)
        {
            return listFieldInfo.FieldType.IsGenericType
                ? listFieldInfo.FieldType.GetGenericArguments()[0]
                : listFieldInfo.FieldType.GetElementType();
        }

        private object DrawDropdown(Object target, FieldInfo fieldInfo, string label, int selectedValueIndex, object[] values,
            string[] displayOptions)
        {
            EditorGUI.BeginChangeCheck();

            int index = EditorGUILayout.Popup(label, selectedValueIndex, displayOptions);

            if (!EditorGUI.EndChangeCheck()) return null;

            Undo.RecordObject(target, "Dropdown");
            return values[index];
        }

        private object DrawDropdown(Rect rect, Object target, FieldInfo fieldInfo, string label, int selectedValueIndex, object[] values,
            string[] displayOptions)
        {
            EditorGUI.BeginChangeCheck();

            int index = EditorGUI.Popup(rect, label, selectedValueIndex, displayOptions);

            if (!EditorGUI.EndChangeCheck()) return null;

            Undo.RecordObject(target, "Dropdown");
            return values[index];
        }

        public void DrawElement(SerializedProperty property, SerializedProperty element, int index)
        {
            EditorDrawUtility.DrawHeader(element);

            DropdownAttribute dropdownAttribute = PropertyUtility.GetAttribute<DropdownAttribute>(property);
            Object target = PropertyUtility.GetTargetObject(property);

            FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);
            FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, dropdownAttribute.ValuesFieldName);

            var array = fieldInfo.GetValue(target) as IList;
            if (array == null) return;
            
            var value = Draw(valuesFieldInfo, fieldInfo, element, array[index], dropdownAttribute, target);
            if (value == null) return;
            for (var i = 0; i < array.Count; i++)
            {
                if (i != index) continue;

                array[i] = value;
                break;
            }

            fieldInfo.SetValue(target, array);
        }

        public void DrawElement(Rect rect, SerializedProperty property, SerializedProperty element, int index)
        {
            EditorDrawUtility.DrawHeader(element);

            DropdownAttribute dropdownAttribute = PropertyUtility.GetAttribute<DropdownAttribute>(property);
            Object target = PropertyUtility.GetTargetObject(property);

            FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);
            FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, dropdownAttribute.ValuesFieldName);

            var array = fieldInfo.GetValue(target) as IList;
            if (array == null) return;
            
            var value = Draw(rect, valuesFieldInfo, fieldInfo, element, array[index], dropdownAttribute, target);
            if (value == null) return;
            for (var i = 0; i < array.Count; i++)
            {
                if (i != index) continue;

                array[i] = value;
                break;
            }

            fieldInfo.SetValue(target, array);
        }

        public float GetElementHeight(SerializedProperty property, SerializedProperty element)
        {
            DropdownAttribute dropdownAttribute = PropertyUtility.GetAttribute<DropdownAttribute>(property);
            Object target = PropertyUtility.GetTargetObject(property);
            FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);
            FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, dropdownAttribute.ValuesFieldName);

            return valuesFieldInfo == null ||
                   !(valuesFieldInfo.GetValue(target) is IList || fieldInfo.FieldType != GetElementType(valuesFieldInfo) &&
                     fieldInfo.FieldType.GetElementType() != GetElementType(valuesFieldInfo)) &&
                   !(valuesFieldInfo.GetValue(target) is IDropdownList)
                ? 64
                : 20;
        }
    }
}
#endif