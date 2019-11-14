#if UNITY_EDITOR
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(ValueFromAttribute))]
    public class ValueFromDrawer : BaseAutoValueDrawer
    {
        protected override InitState InitPropertyImpl(ref SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<ValueFromAttribute>(property);
            var target = PropertyUtility.GetTargetObject(property);

            if (!SerializedPropertyUtility.InfoExist(target, attribute.ValueName))
                return new InitState(false, "No method or field named \"" + attribute.ValueName + "\" in this script");

            if (!SerializedPropertyUtility.InfoValid(target, attribute.ValueName, property.GetPropertyType()))
                return new InitState(false, "\"" + attribute.ValueName + "\" type is invalid");

            if (!property.isArray || property.propertyType == SerializedPropertyType.String)
                return SerializedPropertyUtility.SetGenericValue(target, property, attribute.ValueName, property.propertyType)
                    ? new InitState(true)
                    : new InitState(false, "\"" + property.displayName + "\" type is not serializable");

            var state = new InitState(true);
            var index = 0;

            object[] outArray;
            if (SerializedPropertyUtility.GetObjectValueFromTypeInfo(target, attribute.ValueName, out outArray) &&
                property.CompareArrays(outArray, target)) return state;

            if (property.arraySize == 0)
                property.InsertArrayElementAtIndex(0);

            if (property.GetArrayElementAtIndex(0).propertyType == SerializedPropertyType.Generic)
            {
                try
                {
                    var propertyType = property.GetPropertyType();
                    var propertyField = target.GetType().GetField(property.name);
                    var array = (IList) Array.CreateInstance(propertyType, outArray.Length);

                    for (var i = 0; i < outArray.Length; i++)
                        array[i] = outArray[i];

                    propertyField.SetValue(target, array);
                    return new InitState(true);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    property.DeleteArrayElementAtIndex(index);
                    return new InitState(false, "\"" + property.displayName + "\" type is not serializable");
                }
            }

            property.ClearArray();
            while (index < outArray.Length)
            {
                try
                {
                    property.InsertArrayElementAtIndex(index);
                    if (!(state = SerializedPropertyUtility.SetArrayElementGenericValue(target, property,
                        property.GetArrayElementAtIndex(index), attribute.ValueName, property.GetArrayElementAtIndex(index).propertyType,
                        index)
                        ? new InitState(true)
                        : new InitState(false, "\"" + property.displayName + "\" type is not serializable")).isOk)
                        return state;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    property.DeleteArrayElementAtIndex(index);
                    break;
                }

                index++;
            }

            return state;
        }
    }
}
#endif