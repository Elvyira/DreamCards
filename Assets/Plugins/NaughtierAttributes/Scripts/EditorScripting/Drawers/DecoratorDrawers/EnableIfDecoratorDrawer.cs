#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [DecoratorDrawer(typeof(EnableIfAttribute))]
    public class EnableIfDecoratorDrawer : BaseDecoratorDrawer
    {
        public override void BeginDraw(SerializedProperty property)
        {
            bool drawEnabled = false;
            bool validCondition = false;

            EnableIfAttribute enableIfAttribute = PropertyUtility.GetAttribute<EnableIfAttribute>(property);
            Object target = PropertyUtility.GetTargetObject(property);

            FieldInfo conditionField = ReflectionUtility.GetField(target, enableIfAttribute.ConditionName);
            if (conditionField != null &&
                conditionField.FieldType == typeof(bool))
            {
                drawEnabled = (bool) conditionField.GetValue(target);
                validCondition = true;
            }

            MethodInfo conditionMethod = ReflectionUtility.GetMethod(target, enableIfAttribute.ConditionName);
            if (conditionMethod != null &&
                conditionMethod.ReturnType == typeof(bool) &&
                conditionMethod.GetParameters().Length == 0)
            {
                drawEnabled = (bool) conditionMethod.Invoke(target, null);
                validCondition = true;
            }

            if (validCondition)
            {
                GUI.enabled = drawEnabled;
            }
            else
            {
                string warning = enableIfAttribute.GetType().Name + " needs a valid boolean condition field or method name to work";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true, context: target);
            }
        }

        public override void EndDraw(SerializedProperty property)
        {
            GUI.enabled = true;
        }
    }
}
#endif