#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(FindAssetsAttribute))]
    public class FindAssetsDrawer : BaseAutoValueArrayDrawer
    {
        protected override Object[] FoundArray(SerializedProperty property)
        {
            return property.FindAssetsWithName(PropertyUtility.GetAttribute<FindAssetsAttribute>(property).Name);
        }
    }
}
#endif