#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class MethodDrawerDatabase
    {
        private static Dictionary<Type, BaseMethodDrawer> drawersByAttributeType;

        static MethodDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, BaseMethodDrawer>();
            drawersByAttributeType[typeof(ButtonAttribute)] = new ButtonMethodDrawer();

        }

        public static BaseMethodDrawer GetDrawerForAttribute(Type attributeType)
        {
            BaseMethodDrawer drawer;
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
