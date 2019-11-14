#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class ArrayDrawerDatabase
    {
        private static Dictionary<Type, BaseArrayDrawer> drawersByAttributeType;

        static ArrayDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, BaseArrayDrawer>();
            drawersByAttributeType[typeof(ReorderableListAttribute)] = new ReorderableListArrayDrawer();

        }

        public static BaseArrayDrawer GetDrawerForAttribute(Type attributeType)
        {
            BaseArrayDrawer drawer;
            if (drawersByAttributeType.TryGetValue(attributeType, out drawer))
            {
                return drawer;
            }
            else
            {
                return null;
            }
        }
        
        public static void ClearCache()
        {
            foreach (var kvp in drawersByAttributeType)
            {
                kvp.Value.ClearCache();
            }
        }
    }
}
#endif
