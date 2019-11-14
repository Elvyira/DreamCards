#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class ConditionalGrouperAttribute : BaseAttribute
    {
        public ConditionalGrouperAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif