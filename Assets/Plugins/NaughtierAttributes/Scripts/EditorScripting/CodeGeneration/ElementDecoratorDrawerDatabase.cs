#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class ElementDecoratorDrawerDatabase
    {
        private static Dictionary<Type, BaseElementDecoratorDrawer> drawersByAttributeType;

        static ElementDecoratorDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, BaseElementDecoratorDrawer>();
            drawersByAttributeType[typeof(ShowAssetPreviewAttribute)] = new ShowAssetPreviewElementDecoratorDrawer();

        }

        public static BaseElementDecoratorDrawer GetDrawerForAttribute(Type attributeType)
        {
            BaseElementDecoratorDrawer drawer;
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
