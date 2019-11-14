#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class FieldDrawerDatabase
    {
        private static Dictionary<Type, BaseFieldDrawer> drawersByAttributeType;

        static FieldDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, BaseFieldDrawer>();
            drawersByAttributeType[typeof(ShowNonSerializedFieldAttribute)] = new ShowNonSerializedFieldFieldDrawer();

        }

        public static BaseFieldDrawer GetDrawerForAttribute(Type attributeType)
        {
            BaseFieldDrawer drawer;
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
