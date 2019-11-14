#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class AutoValueDrawerAttribute : BaseAttribute
    {
        public AutoValueDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif