#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    public abstract class BasePropertyDrawCondition
    {
        public abstract bool CanDrawProperty(SerializedProperty property);
    }
}
#endif