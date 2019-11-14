#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(FindObjectsAttribute))]
    public class FindObjectsDrawer : BaseAutoValueArrayDrawer
    {
        protected override Object[] FoundArray(SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<FindObjectsAttribute>(property);
            return property.FindObjectsWithName(attribute.Name, attribute.IncludeInactive);
        }
    }
}
#endif