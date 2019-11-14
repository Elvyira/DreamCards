#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class ClassDrawerDatabase
    {
        private static Dictionary<Type, BaseClassDrawer> drawersByAttributeType;

        static ClassDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, BaseClassDrawer>();
            
        }

        public static BaseClassDrawer GetDrawerForAttribute(Type attributeType)
        {
            BaseClassDrawer drawer;
            if (drawersByAttributeType.TryGetValue(attributeType, out drawer))
            {
                return drawer;
            }
            else
            {
                return null;
            }
        }
    }
}
#endif
