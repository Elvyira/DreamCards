#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [DecoratorDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDecoratorDrawer : BaseDecoratorDrawer
    {
        public override void BeginDraw(SerializedProperty property)
        {
            GUI.enabled = false;
        }

        public override void EndDraw(SerializedProperty property)
        {
            GUI.enabled = true;
        }
    }
}
#endif