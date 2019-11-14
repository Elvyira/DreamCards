#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class ClassDrawerAttribute : BaseAttribute
    {
        public ClassDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif