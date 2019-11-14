#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    public interface IArrayElementDrawer
    {
        void DrawElement(SerializedProperty property, SerializedProperty element, int index);
        
        void DrawElement(Rect rect, SerializedProperty property, SerializedProperty element, int index);

        float GetElementHeight(SerializedProperty property, SerializedProperty element);
    }
}
#endif