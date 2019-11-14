#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [FieldDrawer(typeof(ShowNonSerializedFieldAttribute))]
    public class ShowNonSerializedFieldFieldDrawer : BaseFieldDrawer
    {
        public override void DrawField(UnityEngine.Object target, FieldInfo field)
        {
            object value = field.GetValue(target);

            if (value == null)
            {
                string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNonSerializedFieldFieldDrawer).Name, "Reference");
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true, context: target);
            }
            else if (!EditorDrawUtility.DrawLayoutField(value, field.Name))
            {
                string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNonSerializedFieldFieldDrawer).Name, field.FieldType.Name);
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true, context: target);
            }
        }
    }
}
#endif