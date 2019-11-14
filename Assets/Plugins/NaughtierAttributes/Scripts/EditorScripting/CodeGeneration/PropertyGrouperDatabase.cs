#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class PropertyGrouperDatabase
    {
        private static Dictionary<Type, BasePropertyGrouper> groupersByAttributeType;

        static PropertyGrouperDatabase()
        {
            groupersByAttributeType = new Dictionary<Type, BasePropertyGrouper>();
            groupersByAttributeType[typeof(BoxGroupAttribute)] = new BoxGroupPropertyGrouper();

        }

        public static BasePropertyGrouper GetGrouperForAttribute(Type attributeType)
        {
            BasePropertyGrouper grouper;
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
