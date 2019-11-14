#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(GetComponentInChildrenWithLayerAttribute))]
    public class GetComponentsInChildrenWithLayerDrawer : BaseAutoValueArrayDrawer
    {
        protected override Object[] FoundArray(SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<GetComponentInChildrenWithLayerAttribute>(property);

            return (Object[]) property.GetComponentsInChildrenWithLayer(attribute.Layer, attribute.IncludeInactive);
        }
    }
}
#endif