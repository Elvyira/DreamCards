using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : BaseDrawerAttribute
    {
        public string Text { get; private set; }

        public ButtonAttribute(string text = null)
        {
            this.Text = text;
        }
    }
}
