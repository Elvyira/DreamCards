#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    public abstract class BasePropertyDrawer
    {
        public abstract void DrawProperty(SerializedProperty property);
        
        public virtual void ClearCache()
        {

        }
    }
}
#endif