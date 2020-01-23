using System;

namespace MightyAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : BaseOnInspectorGUIMethodAttribute
    {
        public string Text { get; }

        public string EnabledCallback { get; }

        public ButtonAttribute(string text = null, string enabledCallback = null, bool executeInEditMode = true) : base(executeInEditMode)
        {
            Text = text;
            EnabledCallback = enabledCallback;
        }
    }
}