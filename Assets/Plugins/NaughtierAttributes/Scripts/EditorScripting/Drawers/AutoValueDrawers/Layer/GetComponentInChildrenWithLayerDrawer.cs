#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(GetComponentInChildrenWithLayerAttribute))]
    public class GetComponentInChildrenWithLayerDrawer : BaseSearchDrawer
    {
        protected override void Find(ref SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<GetComponentInChildrenWithLayerAttribute>(property);
            property.objectReferenceValue = property.GetComponentInChildrenWithLayer(attribute.Layer, attribute.IncludeInactive);
        }
    }
}
#endif