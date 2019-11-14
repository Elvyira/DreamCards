#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(FindAssetsInFoldersAttribute))]
    public class FindAssetsInFoldersDrawer : BaseAutoValueArrayDrawer
    {
        protected override Object[] FoundArray(SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<FindAssetsInFoldersAttribute>(property);
            return property.FindAssetsInFolders(attribute.Name, attribute.Folders);
        }
    }
}
#endif