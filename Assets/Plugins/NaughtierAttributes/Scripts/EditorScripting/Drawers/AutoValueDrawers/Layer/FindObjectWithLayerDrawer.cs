#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(FindObjectWithLayerAttribute))]
    public class FindObjectWithLayerDrawer : BaseSearchDrawer
    {
        protected override void Find(ref SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<FindObjectWithLayerAttribute>(property);
            property.objectReferenceValue = property.FindObjectWithLayer(attribute.Layer, attribute.IncludeInactive);
        }
    }
}
#endif