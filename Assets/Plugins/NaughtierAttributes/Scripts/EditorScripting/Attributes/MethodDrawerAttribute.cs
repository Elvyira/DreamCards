#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class MethodDrawerAttribute : BaseAttribute
    {
        public MethodDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif