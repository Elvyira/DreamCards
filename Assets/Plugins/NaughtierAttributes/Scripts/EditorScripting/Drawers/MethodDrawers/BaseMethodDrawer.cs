#if UNITY_EDITOR
using System.Reflection;

namespace NaughtierAttributes.Editor
{
    public abstract class BaseMethodDrawer
    {
        public abstract void DrawMethod(UnityEngine.Object target, MethodInfo methodInfo);
    }
}
#endif