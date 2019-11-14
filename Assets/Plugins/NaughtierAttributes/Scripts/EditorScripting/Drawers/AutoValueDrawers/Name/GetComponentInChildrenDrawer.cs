#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(GetComponentInChildrenAttribute))]
    public class GetComponentInChildrenDrawer : BaseSearchDrawer
    {
        protected override void Find(ref SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<GetComponentInChildrenAttribute>(property);
            property.objectReferenceValue = property.GetComponentInChildrenWithName(attribute.Name, attribute.IncludeInactive);
        }
    }
}
#endif