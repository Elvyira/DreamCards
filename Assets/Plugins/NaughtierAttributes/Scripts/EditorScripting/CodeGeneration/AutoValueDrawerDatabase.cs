#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class AutoValueDrawerDatabase
    {
        private static Dictionary<Type, BaseAutoValueDrawer> drawersByAttributeType;

        static AutoValueDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, BaseAutoValueDrawer>();
            drawersByAttributeType[typeof(AnimatorParameterAttribute)] = new AnimatorParameterDrawer();
drawersByAttributeType[typeof(FindAssetAttribute)] = new FindAssetDrawer();
drawersByAttributeType[typeof(FindAssetInFoldersAttribute)] = new FindAssetInFoldersDrawer();
drawersByAttributeType[typeof(FindAssetsAttribute)] = new FindAssetsDrawer();
drawersByAttributeType[typeof(FindAssetsInFoldersAttribute)] = new FindAssetsInFoldersDrawer();
drawersByAttributeType[typeof(GetComponentAttribute)] = new GetComponentDrawer();
drawersByAttributeType[typeof(GetComponentsAttribute)] = new GetComponentsDrawer();
drawersByAttributeType[typeof(FindObjectsWithLayerAttribute)] = new FindObjectsWithLayerDrawer();
drawersByAttributeType[typeof(FindObjectWithLayerAttribute)] = new FindObjectWithLayerDrawer();
drawersByAttributeType[typeof(GetComponentInChildrenWithLayerAttribute)] = new GetComponentInChildrenWithLayerDrawer();
drawersByAttributeType[typeof(GetComponentInChildrenWithLayerAttribute)] = new GetComponentsInChildrenWithLayerDrawer();
drawersByAttributeType[typeof(LayerNameAttribute)] = new LayerNameDrawer();
drawersByAttributeType[typeof(FindObjectAttribute)] = new FindObjectDrawer();
drawersByAttributeType[typeof(FindObjectsAttribute)] = new FindObjectsDrawer();
drawersByAttributeType[typeof(GetComponentInChildrenAttribute)] = new GetComponentInChildrenDrawer();
drawersByAttributeType[typeof(GetComponentsInChildrenAttribute)] = new GetComponentsInChildrenDrawer();
drawersByAttributeType[typeof(FindObjectsWithTagAttribute)] = new FindObjectsWithTagDrawer();
drawersByAttributeType[typeof(FindObjectWithTagAttribute)] = new FindObjectWithTagDrawer();
drawersByAttributeType[typeof(GetComponentInChildrenWithTagAttribute)] = new GetComponentInChildrenWithTagDrawer();
drawersByAttributeType[typeof(GetComponentsInChildrenWithTagAttribute)] = new GetComponentsInChildrenWithTagDrawer();
drawersByAttributeType[typeof(ValueFromAttribute)] = new ValueFromDrawer();

        }

        public static BaseAutoValueDrawer GetDrawerForAttribute(Type attributeType)
        {
            BaseAutoValueDrawer drawer;
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
