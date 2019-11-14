#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(GetComponentsInChildrenWithTagAttribute))]
    public class GetComponentsInChildrenWithTagDrawer : BaseAutoValueArrayDrawer
    {
        protected override Object[] FoundArray(SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<GetComponentsInChildrenWithTagAttribute>(property);
            return (Object[]) property.GetComponentsInChildrenWithTag(attribute.Tag, attribute.IncludeInactive);
        }
    }
}
#endif