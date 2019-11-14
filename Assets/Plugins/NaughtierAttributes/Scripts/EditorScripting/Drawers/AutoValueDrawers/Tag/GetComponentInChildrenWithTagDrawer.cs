#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(GetComponentInChildrenWithTagAttribute))]
    public class GetComponentInChildrenWithTagDrawer : BaseSearchDrawer
    {
        protected override void Find(ref SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<GetComponentInChildrenWithTagAttribute>(property);
            property.objectReferenceValue = property.GetComponentInChildrenWithTag(attribute.Tag, attribute.IncludeInactive);
        }
    }
}
#endif