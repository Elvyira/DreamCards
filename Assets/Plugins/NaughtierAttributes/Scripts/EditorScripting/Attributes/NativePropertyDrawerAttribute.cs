#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class NativePropertyDrawerAttribute : BaseAttribute
    {
        public NativePropertyDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif