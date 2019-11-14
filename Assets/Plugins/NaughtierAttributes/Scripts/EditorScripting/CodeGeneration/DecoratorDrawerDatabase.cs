#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class DecoratorDrawerDatabase
    {
        private static Dictionary<Type, BaseDecoratorDrawer> drawersByAttributeType;

        static DecoratorDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, BaseDecoratorDrawer>();
            drawersByAttributeType[typeof(AlignAttribute)] = new AlignDecoratorDrawer();
drawersByAttributeType[typeof(ColorAttribute)] = new ColorDecoratorDrawer();
drawersByAttributeType[typeof(DisableIfAttribute)] = new DisableIfDecoratorDrawer();
drawersByAttributeType[typeof(EnableIfAttribute)] = new EnableIfDecoratorDrawer();
drawersByAttributeType[typeof(ReadOnlyAttribute)] = new ReadOnlyDecoratorDrawer();

        }

        public static BaseDecoratorDrawer GetDrawerForAttribute(Type attributeType)
        {
            BaseDecoratorDrawer drawer;
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
