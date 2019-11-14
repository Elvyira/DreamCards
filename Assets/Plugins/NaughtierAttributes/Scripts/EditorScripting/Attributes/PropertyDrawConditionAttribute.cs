#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class PropertyDrawConditionAttribute : BaseAttribute
    {
        public PropertyDrawConditionAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif