#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Reflection;

namespace NaughtierAttributes.Editor
{
    public static class PropertyUtility
    {
        public static T GetAttribute<T>(SerializedProperty property) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(property);
            return attributes != null && attributes.Length > 0 ? attributes[0] : null;
        }

        public static T[] GetAttributes<T>(SerializedProperty property) where T : Attribute
        {
            FieldInfo fieldInfo = ReflectionUtility.GetField(GetTargetObject(property), property.name);

            return fieldInfo != null ? (T[]) fieldInfo.GetCustomAttributes(typeof(T), true) : null;
        }

        public static UnityEngine.Object GetTargetObject(SerializedProperty property)
        {
            return property.serializedObject.targetObject;
        }
    }
}
#endif