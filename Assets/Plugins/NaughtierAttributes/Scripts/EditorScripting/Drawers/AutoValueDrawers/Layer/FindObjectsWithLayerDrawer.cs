#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(FindObjectsWithLayerAttribute))]
    public class FindObjectsWithLayerDrawer : BaseAutoValueArrayDrawer
    {
        protected override Object[] FoundArray(SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<FindObjectsWithLayerAttribute>(property);

            return property.FindObjectsWithLayer(attribute.Layer, attribute.IncludeInactive);
        }
    }
}
#endif