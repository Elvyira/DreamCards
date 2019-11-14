#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(FindObjectWithTagAttribute))]
    public class FindObjectWithTagDrawer : BaseSearchDrawer
    {
        protected override void Find(ref SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<FindObjectWithTagAttribute>(property);
            property.objectReferenceValue = property.FindObjectWithTag(attribute.Tag, attribute.IncludeInactive);
        }
    }
}
#endif