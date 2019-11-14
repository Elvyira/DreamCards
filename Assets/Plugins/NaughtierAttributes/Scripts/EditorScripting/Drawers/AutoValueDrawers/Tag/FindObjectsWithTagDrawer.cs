#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(FindObjectsWithTagAttribute))]
    public class FindObjectsWithTagDrawer : BaseAutoValueArrayDrawer
    {
        protected override Object[] FoundArray(SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<FindObjectsWithTagAttribute>(property);
            return property.FindObjectsWithTag(attribute.Tag, attribute.IncludeInactive);
        }
    }
}
#endif