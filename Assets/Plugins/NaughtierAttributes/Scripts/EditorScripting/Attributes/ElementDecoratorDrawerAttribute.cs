#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class ElementDecoratorDrawerAttribute : BaseAttribute
    {
        public ElementDecoratorDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif