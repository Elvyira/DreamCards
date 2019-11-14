#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [NativePropertyDrawer(typeof(ShowNativePropertyAttribute))]
    public class ShowNativePropertyNativePropertyDrawer : BaseNativePropertyDrawer
    {
        public override void DrawNativeProperty(UnityEngine.Object target, PropertyInfo property)
        {
            object value = property.GetValue(target, null);

            if (value == null)
            {
                string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNativePropertyNativePropertyDrawer).Name, "Reference");
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true, context: target);
            }
            else if (!EditorDrawUtility.DrawLayoutField(value, property.Name))
            {
                string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNativePropertyNativePropertyDrawer).Name, property.PropertyType.Name);
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true, context: target);
            }
        }
    }
}
#endif