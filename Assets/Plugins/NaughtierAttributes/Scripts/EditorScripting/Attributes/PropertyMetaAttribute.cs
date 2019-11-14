#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class PropertyMetaAttribute : BaseAttribute
    {
        public PropertyMetaAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif