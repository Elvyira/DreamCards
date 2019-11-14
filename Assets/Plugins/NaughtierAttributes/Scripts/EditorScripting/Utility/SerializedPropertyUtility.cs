#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace NaughtierAttributes.Editor
{
    public static class SerializedPropertyUtility
    {
        #region Getters

        public static GameObject[] GetRootGameObjects()
        {
            var activeScene = SceneManager.GetActiveScene();
            return activeScene.isLoaded ? activeScene.GetRootGameObjects() : new GameObject[0];
        }

        public static Type GetPropertyType(this SerializedProperty property)
        {
            var targetType = property.serializedObject.targetObject.GetType();
            FieldInfo fieldInfo = null;
            while (targetType != null)
            {
                fieldInfo = targetType.GetField(property.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fieldInfo != null) break;
                targetType = targetType.BaseType;
            }

            if (fieldInfo == null) return null;

            if (fieldInfo.FieldType.IsArray)
                return fieldInfo.FieldType.GetElementType();
            if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType) && fieldInfo.FieldType.IsGenericType)
                return fieldInfo.FieldType.GetGenericArguments()[0];
            return fieldInfo.FieldType;
        }

        public static GameObject GetGameObject(this SerializedProperty property)
        {
            return ((MonoBehaviour) property.serializedObject.targetObject).gameObject;
        }

        #endregion

        #region FindObject

        public static Object FindObject(this Type type, string str, bool includeInactive,
            Func<GameObject, Type, bool, string, bool> comparer)
        {
            return GetRootGameObjects().SelectMany(o => o.GetComponentsInChildren(type, includeInactive))
                .FirstOrDefault(c => comparer(c.gameObject, type, includeInactive, str));

//            var result = GetRootGameObjects().FirstOrDefault(x => comparer(x, type, includeInactive, str));
//            return result != null ? result.GetComponentInChildren(type, true).gameObject : null;
        }

        public static Object[] FindObjects(this Type type, string str, bool includeInactive,
            Func<GameObject, Type, bool, string, bool> comparer)
        {
            var list = new List<Object>();
            foreach (var o in GetRootGameObjects())
                list.AddRange(o.GetComponentsInChildren(type, includeInactive)
                    .Where(c => comparer(c.gameObject, type, includeInactive, str)));
            return list.ToArray();

//            var array = GetRootGameObjects().Where(x => comparer(x, type, includeInactive, str)).ToArray();
//            return array;
        }

        public static Object FindObject(this SerializedProperty property, string str, bool includeInactive,
            Func<GameObject, Type, bool, string, bool> comparer) =>
            property.GetPropertyType().FindObject(str, includeInactive, comparer);

        public static Object[] FindObjects(this SerializedProperty property, string str, bool includeInactive,
            Func<GameObject, Type, bool, string, bool> comparer) =>
            property.GetPropertyType().FindObjects(str, includeInactive, comparer);

        public static Object FindObjectWithTag(this SerializedProperty property, string tag = null, bool includeInactive = false) =>
            property.FindObject(tag, includeInactive, CompareTypeAndTag);

        public static Object[] FindObjectsWithTag(this SerializedProperty property, string tag = null, bool includeInactive = false) =>
            property.FindObjects(tag, includeInactive, CompareTypeAndTag).ToArray();

        public static Object FindObjectWithName(this SerializedProperty property, string name = null, bool includeInactive = false) =>
            property.FindObject(name, includeInactive, CompareTypeAndName);

        public static Object[] FindObjectsWithName(this SerializedProperty property, string name = null, bool includeInactive = false) =>
            property.FindObjects(name, includeInactive, CompareTypeAndName).ToArray();

        public static Object FindObjectWithLayer(this SerializedProperty property, string layer = null, bool includeInactive = false) =>
            property.FindObject(layer, includeInactive, CompareTypeAndLayer);

        public static Object[] FindObjectsWithLayer(this SerializedProperty property, string layer = null, bool includeInactive = false) =>
            property.FindObjects(layer, includeInactive, CompareTypeAndLayer).ToArray();

        #endregion

        #region FindAsset

        public static Object FindAssetOfType(this Type type, string name = null, string[] folders = null)
        {
            return FindAssetsOfType(type, name, folders).FirstOrDefault();
        }

        public static Object[] FindAssetsOfType(this Type type, string name = null, string[] folders = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (folders != null)
                {
                    return AssetDatabase.FindAssets(string.Format("{0} t:{1}", name, type).Replace("UnityEngine.", ""), folders)
                        .Select(AssetDatabase.GUIDToAssetPath).Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type))
                        .Where(asset => asset != null).ToArray();
                }

                return AssetDatabase.FindAssets(string.Format("{0} t:{1}", name, type).Replace("UnityEngine.", ""))
                    .Select(AssetDatabase.GUIDToAssetPath).Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type))
                    .Where(asset => asset != null).ToArray();
            }

            if (folders != null)
            {
                return AssetDatabase.FindAssets(string.Format("t:{0}", type).Replace("UnityEngine.", ""), folders)
                    .Select(AssetDatabase.GUIDToAssetPath).Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type))
                    .Where(asset => asset != null).ToArray();
            }

            return AssetDatabase.FindAssets(string.Format("t:{0}", type).Replace("UnityEngine.", "")).Select(AssetDatabase.GUIDToAssetPath)
                .Select((s, i) => AssetDatabase.LoadAssetAtPath(s, type)).Where(asset => asset != null).ToArray();
        }

        public static Object FindAssetWithName(this SerializedProperty property, string name) =>
            property.GetPropertyType().FindAssetOfType(name);

        public static Object[] FindAssetsWithName(this SerializedProperty property, string name) =>
            property.GetPropertyType().FindAssetsOfType(name);

        public static Object FindAssetInFolders(this SerializedProperty property, string name, string[] folders) =>
            property.GetPropertyType().FindAssetOfType(name, folders);

        public static Object[] FindAssetsInFolders(this SerializedProperty property, string name, string[] folders) =>
            property.GetPropertyType().FindAssetsOfType(name, folders);

        #endregion

        #region GetInChildren

        public static Component GetComponentInChildren(this GameObject gameObject, Type type, string str, bool includeInactive,
            Func<GameObject, string, bool> comparer) =>
            string.IsNullOrEmpty(str)
                ? gameObject.GetComponentInChildren(type, includeInactive)
                : gameObject.GetComponentsInChildren(type, includeInactive).FirstOrDefault(item => comparer(item.gameObject, str));

        public static IEnumerable<Component> GetComponentsInChildren(this GameObject gameObject, Type type, string str,
            bool includeInactive,
            Func<GameObject, string, bool> comparer) =>
            string.IsNullOrEmpty(str)
                ? gameObject.GetComponentsInChildren(type, includeInactive)
                : gameObject.GetComponentsInChildren(type, includeInactive).Where(item => comparer(item.gameObject, str));

        public static Component GetComponentInChildren(this SerializedProperty property, Type type, string str, bool includeInactive,
            Func<GameObject, string, bool> comparer) =>
            property.GetGameObject().GetComponentInChildren(property.GetPropertyType(), str, includeInactive, comparer);

        public static IEnumerable<Component> GetComponentsInChildren(this SerializedProperty property, Type type, string str,
            bool includeInactive, Func<GameObject, string, bool> comparer) =>
            property.GetGameObject().GetComponentsInChildren(property.GetPropertyType(), str, includeInactive, comparer);

        public static Component GetComponentInChildrenWithTag(this SerializedProperty property, string tag = null,
            bool includeInactive = false) =>
            property.GetComponentInChildren(property.GetPropertyType(), tag, includeInactive, TagComparer);

        public static IEnumerable<Component> GetComponentsInChildrenWithTag(this SerializedProperty property, string tag = null,
            bool includeInactive = false) =>
            property.GetComponentsInChildren(property.GetPropertyType(), tag, includeInactive, TagComparer);

        public static Component GetComponentInChildrenWithName(this SerializedProperty property, string name = null,
            bool includeInactive = false) =>
            property.GetComponentInChildren(property.GetPropertyType(), name, includeInactive, NameComparer);

        public static IEnumerable<Component> GetComponentsInChildrenWithName(this SerializedProperty property, string layer = null,
            bool includeInactive = false) =>
            property.GetComponentsInChildren(property.GetPropertyType(), layer, includeInactive, NameComparer);

        public static Component GetComponentInChildrenWithLayer(this SerializedProperty property, string name = null,
            bool includeInactive = false) =>
            property.GetComponentInChildren(property.GetPropertyType(), name, includeInactive, LayerComparer);

        public static IEnumerable<Component> GetComponentsInChildrenWithLayer(this SerializedProperty property, string layer = null,
            bool includeInactive = false) =>
            property.GetComponentsInChildren(property.GetPropertyType(), layer, includeInactive, LayerComparer);

        #endregion

        #region Comparer

        public static bool CompareArrays(this SerializedProperty property, Object[] array)
        {
            if (!property.isArray) return false;
            if (property.arraySize != array.Length) return false;

            var propertyArray = new Object[property.arraySize];
            for (var i = 0; i < propertyArray.Length; i++)
                propertyArray[i] = property.GetArrayElementAtIndex(i).objectReferenceValue;

            return new HashSet<Object>(propertyArray).SetEquals(array);
        }

        public static bool CompareArrays(this SerializedProperty property, object[] array, Object target)
        {
            if (!property.isArray) return false;
            if (property.arraySize != array.Length) return false;
            if (property.arraySize < 1) return true;
            var propertyType = property.GetArrayElementAtIndex(0).propertyType;
            var propertyArray = SetArrayGenericValue(property, propertyType, target);

            return propertyArray != null && (propertyType == SerializedPropertyType.Generic
                       ? new HashSet<object>(propertyArray, new GenericComparer<object>()).SetEquals(
                           new HashSet<object>(propertyArray, new GenericComparer<object>()))
                       : new HashSet<object>(propertyArray).SetEquals(array));
        }

        public static bool CompareArrays(this SerializedProperty property, SerializedProperty other, Object target)
        {
            if (!property.isArray || !other.isArray) return false;
            if (property.arraySize != other.arraySize) return false;
            if (property.propertyType != other.propertyType) return false;
            if (property.arraySize < 1) return true;

            var propertyType = property.GetArrayElementAtIndex(0).propertyType;
            var propertyArray = SetArrayGenericValue(property, propertyType, target);
            if (propertyArray == null) return false;
            var otherArray = SetArrayGenericValue(other, propertyType, target);
            if (otherArray == null) return false;

            return propertyType == SerializedPropertyType.Generic
                ? new HashSet<object>(propertyArray, new GenericComparer<object>()).SetEquals(
                    new HashSet<object>(propertyArray, new GenericComparer<object>()))
                : new HashSet<object>(propertyArray).SetEquals(otherArray);
        }

        private class GenericComparer<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y) => x == null && y == null || x != null && y != null && x.Equals(y);

            public int GetHashCode(T obj) => obj.GetHashCode();
        }

        public static bool CompareTypeAndTag(this GameObject go, Type type, bool includeInactive, string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return go.GetComponentInChildren(type, includeInactive) != null;
            var t = go.GetComponentInChildren(type, includeInactive);
            return t != null && t.gameObject.TagComparer(tag);
        }

        public static bool CompareTypeAndName(this GameObject go, Type type, bool includeInactive, string name)
        {
            if (string.IsNullOrEmpty(name))
                return go.GetComponentInChildren(type, includeInactive) != null;
            var t = go.GetComponentInChildren(type, includeInactive);
            return t != null && t.gameObject.NameComparer(name);
        }

        public static bool CompareTypeAndLayer(this GameObject go, Type type, bool includeInactive, string layer)
        {
            if (string.IsNullOrEmpty(layer))
                return go.GetComponentInChildren(type, includeInactive) != null;
            var t = go.GetComponentInChildren(type, includeInactive);
            return t != null && t.gameObject.LayerComparer(layer);
        }

        public static bool TagComparer(this GameObject gameObject, string tag) => gameObject.CompareTag(tag);

        public static bool NameComparer(this GameObject gameObject, string name) => gameObject.name == name;

        public static bool LayerComparer(this GameObject gameObject, string layer) => gameObject.layer == LayerMask.NameToLayer(layer);

        #endregion

        #region FieldInfo

        public static bool InfoExist(Object target, string infoName) =>
            IsFieldInfoValid(ReflectionUtility.GetField(target, infoName)) ||
            IsMethodInfoValid(ReflectionUtility.GetMethod(target, infoName));

        public static bool InfoValid(Object target, string infoName, Type type) =>
            IsFieldInfoValid(ReflectionUtility.GetField(target, infoName), type) ||
            IsMethodInfoValid(ReflectionUtility.GetMethod(target, infoName), type);

        public static bool IsFieldInfoValid(FieldInfo fieldInfo, Type type = null)
        {
            if (fieldInfo == null) return false;
            var fieldType = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;
            if (fieldType == null) return false;
            return type == null || fieldType == type || fieldType.IsSubclassOf(type);
        }

        public static bool IsMethodInfoValid(MethodInfo methodInfo, Type type = null)
        {
            if (methodInfo == null) return false;
            var returnType = methodInfo.ReturnType.IsArray ? methodInfo.ReturnType.GetElementType() : methodInfo.ReturnType;
            if (returnType == null) return false;
            return type == null || returnType == type || returnType.IsSubclassOf(type) && methodInfo.GetParameters().Length == 0;
        }

        public static bool GetValueFromTypeInfo<T>(Object target, string infoName, out T outValue)
        {
            var fieldInfo = ReflectionUtility.GetField(target, infoName);
            if (IsFieldInfoValid(fieldInfo, typeof(T)))
            {
                outValue = (T) fieldInfo.GetValue(target);
                return true;
            }

            var methodInfo = ReflectionUtility.GetMethod(target, infoName);
            if (IsMethodInfoValid(methodInfo, typeof(T)))
            {
                outValue = (T) methodInfo.Invoke(target, null);
                return true;
            }

            outValue = default(T);
            return false;
        }

        public static bool GetObjectValueFromTypeInfo(Object target, string infoName, out object[] outValue)
        {
            var fieldInfo = ReflectionUtility.GetField(target, infoName);
            if (IsFieldInfoValid(fieldInfo))
            {
                if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType))
                {
                    var list = (IList) fieldInfo.GetValue(target);
                    outValue = new object[list.Count];
                    for (var i = 0; i < list.Count; i++)
                        outValue[i] = list[i];
                    return true;
                }

                outValue = null;
                return false;
            }

            var methodInfo = ReflectionUtility.GetMethod(target, infoName);
            if (IsMethodInfoValid(methodInfo))
            {
                if (typeof(IList).IsAssignableFrom(methodInfo.ReturnType))
                {
                    var list = (IList) methodInfo.Invoke(target, null);
                    outValue = new object[list.Count];
                    for (var i = 0; i < list.Count; i++)
                        outValue[i] = list[i];
                    return true;
                }

                outValue = null;
                return false;
            }

            outValue = null;
            return false;
        }

        public static bool SetGenericValue(Object target, SerializedProperty property, string valueName, SerializedPropertyType type)
        {
            switch (type)
            {
                case SerializedPropertyType.Generic:
                    var propertyType = property.GetPropertyType();
                    object objectValue;
                    if (!GetValueFromTypeInfo(target, valueName, out objectValue)) return false;

                    var propertyField = target.GetType().GetField(property.name);
                    var genericTarget = propertyField.GetValue(target);

                    var fields = propertyType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var field in fields)
                    {
                        if (field.IsPrivate && field.GetCustomAttributes(typeof(SerializeField), false).Length <= 0) continue;
                        var instanceField = objectValue.GetType().GetField(field.Name,
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (instanceField == null) continue;
                        var value = instanceField.GetValue(objectValue);

                        field.SetValue(genericTarget, value);
                    }

                    if (!propertyType.IsClass)
                        propertyField.SetValue(target, genericTarget);

                    return true;

                case SerializedPropertyType.Enum:
                    Enum enumValue;
                    if (!GetValueFromTypeInfo(target, valueName, out enumValue)) return false;

                    property.enumValueIndex = Convert.ToInt32(enumValue);
                    return true;
                case SerializedPropertyType.Integer:
                    int intValue;
                    if (!GetValueFromTypeInfo(target, valueName, out intValue)) return false;

                    property.intValue = intValue;
                    return true;

                case SerializedPropertyType.Boolean:
                    bool boolValue;
                    if (!GetValueFromTypeInfo(target, valueName, out boolValue)) return false;

                    property.boolValue = boolValue;
                    return true;

                case SerializedPropertyType.Float:
                    float floatValue;
                    if (!GetValueFromTypeInfo(target, valueName, out floatValue)) return false;

                    property.floatValue = floatValue;
                    return true;

                case SerializedPropertyType.String:
                    string stringValue;
                    if (!GetValueFromTypeInfo(target, valueName, out stringValue)) return false;

                    property.stringValue = stringValue;
                    return true;

                case SerializedPropertyType.Color:
                    Color colorValue;
                    if (!GetValueFromTypeInfo(target, valueName, out colorValue)) return false;

                    property.colorValue = colorValue;
                    return true;

                case SerializedPropertyType.ObjectReference:
                    Object unityObjectValue;
                    if (!GetValueFromTypeInfo(target, valueName, out unityObjectValue)) return false;

                    property.objectReferenceValue = unityObjectValue;
                    return true;

                case SerializedPropertyType.LayerMask:
                    LayerMask layerMaskValue;
                    if (!GetValueFromTypeInfo(target, valueName, out layerMaskValue)) return false;

                    property.intValue = layerMaskValue;
                    return true;

                case SerializedPropertyType.Vector2:
                    Vector2 vector2Value;
                    if (!GetValueFromTypeInfo(target, valueName, out vector2Value)) return false;

                    property.vector2Value = vector2Value;
                    return true;

                case SerializedPropertyType.Vector3:
                    Vector3 vector3Value;
                    if (!GetValueFromTypeInfo(target, valueName, out vector3Value)) return false;

                    property.vector3Value = vector3Value;
                    return true;

                case SerializedPropertyType.Vector4:
                    Vector4 vector4Value;
                    if (!GetValueFromTypeInfo(target, valueName, out vector4Value)) return false;

                    property.vector4Value = vector4Value;
                    return true;

                case SerializedPropertyType.Rect:
                    Rect rectValue;
                    if (!GetValueFromTypeInfo(target, valueName, out rectValue)) return false;

                    property.rectValue = rectValue;
                    return true;

                case SerializedPropertyType.AnimationCurve:
                    AnimationCurve animationCurveValue;
                    if (!GetValueFromTypeInfo(target, valueName, out animationCurveValue)) return false;

                    property.animationCurveValue = animationCurveValue;
                    return true;

                case SerializedPropertyType.Bounds:
                    Bounds boundsValue;
                    if (!GetValueFromTypeInfo(target, valueName, out boundsValue)) return false;

                    property.boundsValue = boundsValue;
                    return true;

                case SerializedPropertyType.Quaternion:
                    Quaternion quaternionValue;
                    if (!GetValueFromTypeInfo(target, valueName, out quaternionValue)) return false;

                    property.quaternionValue = quaternionValue;
                    return true;

                case SerializedPropertyType.Vector2Int:
                    Vector2Int vector2IntValue;
                    if (!GetValueFromTypeInfo(target, valueName, out vector2IntValue)) return false;

                    property.vector2IntValue = vector2IntValue;
                    return true;

                case SerializedPropertyType.Vector3Int:
                    Vector3Int vector3IntValue;
                    if (!GetValueFromTypeInfo(target, valueName, out vector3IntValue)) return false;

                    property.vector3IntValue = vector3IntValue;
                    return true;

                case SerializedPropertyType.RectInt:
                    RectInt rectIntValue;
                    if (!GetValueFromTypeInfo(target, valueName, out rectIntValue)) return false;

                    property.rectIntValue = rectIntValue;
                    return true;

                case SerializedPropertyType.BoundsInt:
                    BoundsInt boundsIntValue;
                    if (!GetValueFromTypeInfo(target, valueName, out boundsIntValue)) return false;

                    property.boundsIntValue = boundsIntValue;
                    return true;

                default:
                    return false;
            }
        }

        public static bool SetArrayElementGenericValue(Object target, SerializedProperty property, SerializedProperty element,
            string valueName,
            SerializedPropertyType type, int index)
        {
            switch (type)
            {
                case SerializedPropertyType.Enum:
                case SerializedPropertyType.Integer:
                    object[] intValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out intValue)) return false;

                    element.intValue = (int) intValue[index];
                    return true;

                case SerializedPropertyType.Boolean:
                    object[] boolValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out boolValue)) return false;

                    element.boolValue = (bool) boolValue[index];
                    return true;

                case SerializedPropertyType.Float:
                    object[] floatValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out floatValue)) return false;

                    element.floatValue = (float) floatValue[index];
                    return true;

                case SerializedPropertyType.String:
                    object[] stringValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out stringValue)) return false;

                    element.stringValue = (string) stringValue[index];
                    return true;

                case SerializedPropertyType.Color:
                    object[] colorValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out colorValue)) return false;

                    element.colorValue = (Color) colorValue[index];
                    return true;

                case SerializedPropertyType.ObjectReference:
                    object[] unityObjectValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out unityObjectValue)) return false;

                    element.objectReferenceValue = (Object) unityObjectValue[index];
                    return true;

                case SerializedPropertyType.LayerMask:
                    object[] layerMaskValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out layerMaskValue)) return false;

                    element.intValue = (LayerMask) layerMaskValue[index];
                    return true;

                case SerializedPropertyType.Vector2:
                    object[] vector2Value;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out vector2Value)) return false;

                    element.vector2Value = (Vector2) vector2Value[index];
                    return true;

                case SerializedPropertyType.Vector3:
                    object[] vector3Value;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out vector3Value)) return false;

                    element.vector3Value = (Vector3) vector3Value[index];
                    return true;

                case SerializedPropertyType.Vector4:
                    object[] vector4Value;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out vector4Value)) return false;

                    element.vector4Value = (Vector4) vector4Value[index];
                    return true;

                case SerializedPropertyType.Rect:
                    object[] rectValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out rectValue)) return false;

                    element.rectValue = (Rect) rectValue[index];
                    return true;

                case SerializedPropertyType.AnimationCurve:
                    object[] animationCurveValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out animationCurveValue)) return false;

                    element.animationCurveValue = (AnimationCurve) animationCurveValue[index];
                    return true;

                case SerializedPropertyType.Bounds:
                    object[] boundsValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out boundsValue)) return false;

                    element.boundsValue = (Bounds) boundsValue[index];
                    return true;

                case SerializedPropertyType.Quaternion:
                    object[] quaternionValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out quaternionValue)) return false;

                    element.quaternionValue = (Quaternion) quaternionValue[index];
                    return true;

                case SerializedPropertyType.Vector2Int:
                    object[] vector2IntValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out vector2IntValue)) return false;

                    element.vector2IntValue = (Vector2Int) vector2IntValue[index];
                    return true;

                case SerializedPropertyType.Vector3Int:
                    object[] vector3IntValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out vector3IntValue)) return false;

                    element.vector3IntValue = (Vector3Int) vector3IntValue[index];
                    return true;

                case SerializedPropertyType.RectInt:
                    object[] rectIntValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out rectIntValue)) return false;

                    element.rectIntValue = (RectInt) rectIntValue[index];
                    return true;

                case SerializedPropertyType.BoundsInt:
                    object[] boundsIntValue;
                    if (!GetObjectValueFromTypeInfo(target, valueName, out boundsIntValue)) return false;

                    element.boundsIntValue = (BoundsInt) boundsIntValue[index];
                    return true;

                default:
                    return false;
            }
        }

        public static object[] SetArrayGenericValue(SerializedProperty property, SerializedPropertyType type, Object target)
        {
            var value = new object[property.arraySize];
            for (var i = 0; i < property.arraySize; i++)
                switch (type)
                {
                    case SerializedPropertyType.Generic:
                        var propertyField = target.GetType().GetField(property.name);
                        value[i] = ((IList) propertyField.GetValue(target))[i];
                        break;

                    case SerializedPropertyType.Enum:
                    case SerializedPropertyType.Integer:
                    case SerializedPropertyType.LayerMask:
                        value[i] = property.GetArrayElementAtIndex(i).intValue;
                        break;

                    case SerializedPropertyType.Boolean:
                        value[i] = property.GetArrayElementAtIndex(i).boolValue;
                        break;

                    case SerializedPropertyType.Float:
                        value[i] = property.GetArrayElementAtIndex(i).floatValue;
                        break;

                    case SerializedPropertyType.String:
                        value[i] = property.GetArrayElementAtIndex(i).stringValue;
                        break;

                    case SerializedPropertyType.Color:
                        value[i] = property.GetArrayElementAtIndex(i).colorValue;
                        break;

                    case SerializedPropertyType.ObjectReference:
                        value[i] = property.GetArrayElementAtIndex(i).objectReferenceValue;
                        break;

                    case SerializedPropertyType.Vector2:
                        value[i] = property.GetArrayElementAtIndex(i).vector2Value;
                        break;

                    case SerializedPropertyType.Vector3:
                        value[i] = property.GetArrayElementAtIndex(i).vector3Value;
                        break;

                    case SerializedPropertyType.Vector4:
                        value[i] = property.GetArrayElementAtIndex(i).vector4Value;
                        break;

                    case SerializedPropertyType.Rect:
                        value[i] = property.GetArrayElementAtIndex(i).rectValue;
                        break;

                    case SerializedPropertyType.AnimationCurve:
                        value[i] = property.GetArrayElementAtIndex(i).animationCurveValue;
                        break;

                    case SerializedPropertyType.Bounds:
                        value[i] = property.GetArrayElementAtIndex(i).boundsValue;
                        break;

                    case SerializedPropertyType.Quaternion:
                        value[i] = property.GetArrayElementAtIndex(i).quaternionValue;
                        break;

                    case SerializedPropertyType.Vector2Int:
                        value[i] = property.GetArrayElementAtIndex(i).vector2IntValue;
                        break;

                    case SerializedPropertyType.Vector3Int:
                        value[i] = property.GetArrayElementAtIndex(i).vector3IntValue;
                        break;

                    case SerializedPropertyType.RectInt:
                        value[i] = property.GetArrayElementAtIndex(i).rectIntValue;
                        break;

                    case SerializedPropertyType.BoundsInt:
                        value[i] = property.GetArrayElementAtIndex(i).boundsIntValue;
                        break;

                    default:
                        return null;
                }

            return value;
        }
    }

    #endregion
}
#endif