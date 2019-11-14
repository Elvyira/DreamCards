#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    public abstract class BaseArrayDrawer
    {
        public abstract void DrawArray(SerializedProperty property, IArrayElementDrawer drawer);

        public virtual void ClearCache()
        {
        }
    }
}
#endif