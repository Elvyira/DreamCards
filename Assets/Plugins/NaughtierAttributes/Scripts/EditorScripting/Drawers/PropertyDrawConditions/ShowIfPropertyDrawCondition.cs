#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [PropertyDrawCondition(typeof(ShowIfAttribute))]
    public class ShowIfPropertyDrawCondition : BasePropertyDrawCondition
    {
        public override bool CanDrawProperty(SerializedProperty property)
        {
            ShowIfAttribute showIfAttribute = PropertyUtility.GetAttribute<ShowIfAttribute>(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);
            bool outBool;
            if (SerializedPropertyUtility.GetValueFromTypeInfo(target, showIfAttribute.ConditionName, out outBool))
            {
                return outBool;
            }

            string warning = showIfAttribute.GetType().Name + " needs a valid boolean condition field or method name to work";
            EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true, context: target);

            return true;
        }
    }
}
#endif