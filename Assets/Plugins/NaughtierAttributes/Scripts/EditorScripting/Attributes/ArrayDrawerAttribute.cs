#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class ArrayDrawerAttribute : BaseAttribute
    {
        public ArrayDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif