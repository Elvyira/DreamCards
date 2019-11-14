#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(LayerNameAttribute))]
    public class LayerNameDrawer : BaseAutoValueDrawer
    {
        protected override InitState InitPropertyImpl(ref SerializedProperty property)
        {
            var layerField = PropertyUtility.GetAttribute<LayerNameAttribute>(property);

            if (property.propertyType != SerializedPropertyType.Integer)
                return new InitState(false, "\"" + property.displayName + "\" should be of type int");

            property.intValue = layerField.LayerId;
            return new InitState(true);
        }
    }
}
#endif