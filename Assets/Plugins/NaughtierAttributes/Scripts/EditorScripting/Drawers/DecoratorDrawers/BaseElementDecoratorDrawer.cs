#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NaughtierAttributes.Editor
{
    public abstract class BaseElementDecoratorDrawer
    {       
        public abstract void BeginDraw(SerializedProperty property, Action<SerializedProperty> propertyDrawCallback = null);
        
        public abstract void EndDraw(SerializedProperty property, Action<SerializedProperty> propertyDrawCallback = null);
        
        public abstract void BeginDrawElement(Rect rect, SerializedProperty property, BaseElementDecoratorAttribute baseElementDecoratorAttribute,
            Object target);

        public abstract void EndDrawElement(Rect rect, SerializedProperty property, BaseElementDecoratorAttribute baseElementDecoratorAttribute,
            Object target);

        public abstract void BeginDrawElement(SerializedProperty property, BaseElementDecoratorAttribute baseElementDecoratorAttribute, Object target, bool fromArray);

        public abstract void EndDrawElement(SerializedProperty property, BaseElementDecoratorAttribute baseElementDecoratorAttribute, Object target, bool fromArray);

        public abstract float GetElementHeight(SerializedProperty property, BaseElementDecoratorAttribute baseElementDecoratorAttribute, Object target);
    }
}
#endif