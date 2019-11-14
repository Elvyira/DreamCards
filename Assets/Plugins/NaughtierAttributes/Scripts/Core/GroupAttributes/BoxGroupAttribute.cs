using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class BoxGroupAttribute : BaseGroupAttribute
    {
        public BoxGroupAttribute(string name = "", string backgroundColorName = null, string contentColorName = null) :
            base(name, backgroundColorName, contentColorName)
        {
        }

        public BoxGroupAttribute(string name, ColorValue backgroundColor, ColorValue contentColor = ColorValue.Default) :
            base(name, backgroundColor, contentColor)
        {
        }
        
        public BoxGroupAttribute(ColorValue backgroundColor, ColorValue contentColor = ColorValue.Default) :
            base("", backgroundColor, contentColor)
        {
        }
    }
}