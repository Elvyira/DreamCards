#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    public abstract class BaseDecoratorDrawer
    {
        public abstract void BeginDraw(SerializedProperty property);
        
        public abstract void EndDraw(SerializedProperty property);
    }
}
#endif