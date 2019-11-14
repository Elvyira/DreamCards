#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(FindAssetInFoldersAttribute))]
    public class FindAssetInFoldersDrawer : BaseSearchDrawer
    {
        protected override void Find(ref SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<FindAssetInFoldersAttribute>(property);
            property.objectReferenceValue = property.FindAssetInFolders(attribute.Name, attribute.Folders);
        }
    }
}
#endif