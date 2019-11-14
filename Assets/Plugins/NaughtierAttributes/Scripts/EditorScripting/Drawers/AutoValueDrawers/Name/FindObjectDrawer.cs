#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(FindObjectAttribute))]
    public class FindObjectDrawer : BaseSearchDrawer
    {
        protected override void Find(ref SerializedProperty property)
        {
            var attribute = PropertyUtility.GetAttribute<FindObjectAttribute>(property);
            property.objectReferenceValue = property.FindObjectWithName(attribute.Name, attribute.IncludeInactive);
        }
    }
}
#endif