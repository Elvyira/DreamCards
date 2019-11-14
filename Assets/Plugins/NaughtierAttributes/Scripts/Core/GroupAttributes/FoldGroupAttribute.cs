using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class FoldGroupAttribute : BaseConditionalGroupAttribute
    {
        public FoldGroupAttribute(string name = "", string backgroundColor = null, string contentColor = null) :
            base(name, backgroundColor, contentColor)
        {
        }

        public FoldGroupAttribute(string name, ColorValue backgroundColor, ColorValue contentColor = ColorValue.Default) :
            base(name, backgroundColor, contentColor)
        {
        }

        public FoldGroupAttribute(ColorValue backgroundColor, ColorValue contentColor = ColorValue.Default) :
            base("", backgroundColor, contentColor)
        {
        }
    }
}