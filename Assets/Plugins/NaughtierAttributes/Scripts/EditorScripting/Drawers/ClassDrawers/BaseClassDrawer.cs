#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    public abstract class BaseClassDrawer
    {
        public abstract void BeginDraw(SerializedProperty script);
        public abstract void EndDraw(SerializedProperty script);
    }
}
#endif