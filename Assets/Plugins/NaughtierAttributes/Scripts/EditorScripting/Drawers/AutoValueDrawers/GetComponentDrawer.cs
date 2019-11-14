#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(GetComponentAttribute))]
    public class GetComponentDrawer : BaseAutoValueDrawer
    {
        protected override InitState InitPropertyImpl(ref SerializedProperty property)
        {
            if (!typeof(Object).IsAssignableFrom(property.GetPropertyType()))
                return new InitState(false, "\"" + property.displayName + "\" should inherit from UnityEngine.Object");

            property.objectReferenceValue = property.GetGameObject().GetComponent(property.GetPropertyType());
            return new InitState(true);
        }
    }
}
#endif