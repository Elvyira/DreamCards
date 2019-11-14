#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    public abstract class BasePropertyMeta
    {
        public abstract void ApplyPropertyMeta(SerializedProperty property, BaseMetaAttribute metaAttribute);
    }
}
#endif