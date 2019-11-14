#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(GetComponentsInChildrenAttribute))]
    public class GetComponentsInChildrenDrawer : BaseAutoValueArrayDrawer
    {
        protected override Object[] FoundArray(SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<GetComponentsInChildrenAttribute>(property);

            return (Object[]) property.GetComponentsInChildrenWithName(attribute.Name, attribute.IncludeInactive);
        }
    }
}
#endif