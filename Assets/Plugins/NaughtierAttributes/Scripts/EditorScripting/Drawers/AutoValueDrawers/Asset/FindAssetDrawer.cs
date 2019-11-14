#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(FindAssetAttribute))]
    public class FindAssetDrawer : BaseSearchDrawer
    {
        protected override void Find(ref SerializedProperty property)
        {
            property.objectReferenceValue = property.FindAssetWithName(PropertyUtility.GetAttribute<FindAssetAttribute>(property).Name);
        }
    }
}
#endif