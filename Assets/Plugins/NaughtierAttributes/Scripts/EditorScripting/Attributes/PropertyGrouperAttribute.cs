#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class PropertyGrouperAttribute : BaseAttribute
    {
        public PropertyGrouperAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif