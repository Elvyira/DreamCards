#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [MethodDrawer(typeof(ButtonAttribute))]
    public class ButtonMethodDrawer : BaseMethodDrawer
    {
        public override void DrawMethod(Object target, MethodInfo methodInfo)
        {
            if (methodInfo.GetParameters().Length == 0)
            {
                ButtonAttribute buttonAttribute = (ButtonAttribute) methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
                string buttonText = string.IsNullOrEmpty(buttonAttribute.Text)
                    ? EditorDrawUtility.DrawPrettyName(methodInfo.Name)
                    : buttonAttribute.Text;

                if (GUILayout.Button(buttonText))
                {
                    methodInfo.Invoke(target, null);
                }
            }
            else
            {
                string warning = typeof(ButtonAttribute).Name + " works only on methods with no parameters";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true, context: target);
            }
        }
    }
}
#endif