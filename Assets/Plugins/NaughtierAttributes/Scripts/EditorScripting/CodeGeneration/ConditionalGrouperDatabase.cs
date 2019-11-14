#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class ConditionalGrouperDatabase
    {
        private static Dictionary<Type, BaseConditionalGrouper> groupersByAttributeType;

        static ConditionalGrouperDatabase()
        {
            groupersByAttributeType = new Dictionary<Type, BaseConditionalGrouper>();
            groupersByAttributeType[typeof(FoldGroupAttribute)] = new FoldGroupConditionalGrouper();

        }

        public static BaseConditionalGrouper GetGrouperForAttribute(Type attributeType)
        {
            BaseConditionalGrouper grouper;
            if (groupersByAttributeType.TryGetValue(attributeType, out grouper))
            {
                return grouper;
            }
            else
            {
                return null;
            }
        }
    }
}
#endif
