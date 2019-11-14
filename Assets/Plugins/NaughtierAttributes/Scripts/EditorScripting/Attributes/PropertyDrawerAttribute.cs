#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class PropertyDrawerAttribute : BaseAttribute
    {
        public PropertyDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif