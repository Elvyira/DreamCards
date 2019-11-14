#if UNITY_EDITOR
using System.Reflection;

namespace NaughtierAttributes.Editor
{
    public abstract class BaseFieldDrawer
    {
        public abstract void DrawField(UnityEngine.Object target, FieldInfo field);
    }
}
#endif
