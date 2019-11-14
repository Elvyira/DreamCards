#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [AutoValueDrawer(typeof(AnimatorParameterAttribute))]
    public class AnimatorParameterDrawer : BaseAutoValueDrawer
    {
        protected override InitState InitPropertyImpl(ref SerializedProperty property)
        {
            var animatorParameter = PropertyUtility.GetAttribute<AnimatorParameterAttribute>(property);

            if (property.propertyType != SerializedPropertyType.Integer)
                return new InitState(false, "\"" + property.displayName + "\" should be of type int");

            property.intValue = animatorParameter.ParameterId;
            return new InitState(true);
        }
    }
}
#endif