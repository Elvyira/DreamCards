#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [PropertyDrawCondition(typeof(HideIfAttribute))]
    public class HideIfPropertyDrawCondition : BasePropertyDrawCondition
    {
        public override bool CanDrawProperty(SerializedProperty property)
        {
            HideIfAttribute hideIfAttribute = PropertyUtility.GetAttribute<HideIfAttribute>(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);
            bool outBool;
            if (SerializedPropertyUtility.GetValueFromTypeInfo(target, hideIfAttribute.ConditionName, out outBool))
            {
                return !outBool;
            }

            string warning = hideIfAttribute.GetType().Name + " needs a valid boolean condition field or method name to work";
            EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true, context: target);

            return true;
        }
    }
}
#endif