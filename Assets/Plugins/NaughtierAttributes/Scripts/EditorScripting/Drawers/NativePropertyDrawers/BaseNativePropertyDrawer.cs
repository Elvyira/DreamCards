#if UNITY_EDITOR
using System.Reflection;

namespace NaughtierAttributes.Editor
{
    public abstract class BaseNativePropertyDrawer
    {
        public abstract void DrawNativeProperty(UnityEngine.Object target, PropertyInfo property);
    }
}
#endif