#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    [DecoratorDrawer(typeof(AlignAttribute))]
    public class AlignDecoratorDrawer : BaseDecoratorDrawer
    {
        public override void BeginDraw(SerializedProperty property)
        {
            EditorDrawUtility.BeginDrawAlign(PropertyUtility.GetAttribute<AlignAttribute>(property).Align);
        }

        public override void EndDraw(SerializedProperty property)
        {
            EditorDrawUtility.EndDrawAlign(PropertyUtility.GetAttribute<AlignAttribute>(property).Align);
        }
    }
}
#endif