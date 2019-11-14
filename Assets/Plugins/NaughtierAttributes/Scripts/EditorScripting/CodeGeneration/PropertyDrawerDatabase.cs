#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class PropertyDrawerDatabase
    {
        private static Dictionary<Type, BasePropertyDrawer> drawersByAttributeType;

        static PropertyDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, BasePropertyDrawer>();
            drawersByAttributeType[typeof(HideAttribute)] = new HidePropertyDrawer();
drawersByAttributeType[typeof(DropdownAttribute)] = new DropdownPropertyDrawer();
drawersByAttributeType[typeof(EnumFlagsAttribute)] = new EnumFlagsPropertyDrawer();
drawersByAttributeType[typeof(LayerFieldAttribute)] = new LayerFieldPropertyDrawer();
drawersByAttributeType[typeof(MinMaxSliderAttribute)] = new MinMaxSliderPropertyDrawer();
drawersByAttributeType[typeof(ProgressBarAttribute)] = new ProgressBarPropertyDrawer();
drawersByAttributeType[typeof(ResizableTextAreaAttribute)] = new ResizableTextAreaPropertyDrawer();
drawersByAttributeType[typeof(SliderAttribute)] = new SliderPropertyDrawer();

        }

        public static BasePropertyDrawer GetDrawerForAttribute(Type attributeType)
        {
            BasePropertyDrawer drawer;
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
