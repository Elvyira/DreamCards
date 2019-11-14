#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    public abstract class BasePropertyValidator
    {
        public abstract void ValidateProperty(SerializedProperty property);
    }
}
#endif