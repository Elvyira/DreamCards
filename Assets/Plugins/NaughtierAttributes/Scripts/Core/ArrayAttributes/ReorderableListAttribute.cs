using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReorderableListAttribute : BaseArrayAttribute
    {
        public readonly bool drawButtons;

        public ReorderableListAttribute(bool drawButtons = true) => this.drawButtons = drawButtons;
    }
}
