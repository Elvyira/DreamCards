#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class PropertyValidatorAttribute : BaseAttribute
    {
        public PropertyValidatorAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif