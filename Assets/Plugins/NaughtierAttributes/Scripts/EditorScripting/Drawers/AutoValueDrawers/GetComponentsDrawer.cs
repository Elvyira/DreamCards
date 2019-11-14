#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(GetComponentsAttribute))]
    public class GetComponentsDrawer : BaseAutoValueArrayDrawer
    {
        protected override Object[] FoundArray(SerializedProperty property)
        {
            return (Object[]) property.GetGameObject().GetComponents(property.GetPropertyType());
        }
    }
}
#endif