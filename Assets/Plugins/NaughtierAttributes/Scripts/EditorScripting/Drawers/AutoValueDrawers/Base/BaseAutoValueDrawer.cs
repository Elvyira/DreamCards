#if UNITY_EDITOR
using UnityEditor;

namespace NaughtierAttributes.Editor
{
    public struct InitState
    {
        public readonly bool isOk;
        public readonly string message;

        public InitState(bool isOk, string message = null)
        {
            this.isOk = isOk;
            this.message = message;
        }
    }

    public abstract class BaseAutoValueDrawer
    {
        public void DrawProperty(SerializedProperty property, InitState state)
        {
            EditorDrawUtility.DrawPropertyField(property);
            if (!state.isOk)
                EditorDrawUtility.DrawHelpBox(state.message, MessageType.Warning, true, PropertyUtility.GetTargetObject(property));
        }

        public InitState InitProperty(ref SerializedProperty property)
        {
            return PropertyUtility.GetAttribute<BaseAutoValueAttribute>(property).PlayUpdate ? InitPropertyImpl(ref property) :
                !EditorApplication.isPlaying ? InitPropertyImpl(ref property) : new InitState(true);
        }

        protected abstract InitState InitPropertyImpl(ref SerializedProperty property);

        public virtual void ClearCache()
        {
        }
    }
}
#endif