#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class DecoratorDrawerAttribute : BaseAttribute
    {
        public DecoratorDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif