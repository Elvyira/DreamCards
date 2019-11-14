#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public class FieldDrawerAttribute : BaseAttribute
    {
        public FieldDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
#endif