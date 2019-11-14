#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class NativePropertyDrawerDatabase
    {
        private static Dictionary<Type, BaseNativePropertyDrawer> drawersByAttributeType;

        static NativePropertyDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, BaseNativePropertyDrawer>();
            drawersByAttributeType[typeof(ShowNativePropertyAttribute)] = new ShowNativePropertyNativePropertyDrawer();

        }

        public static BaseNativePropertyDrawer GetDrawerForAttribute(Type attributeType)
        {
            BaseNativePropertyDrawer drawer;
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
