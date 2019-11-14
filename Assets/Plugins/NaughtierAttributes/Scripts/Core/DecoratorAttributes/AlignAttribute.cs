using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AlignAttribute : BaseDecoratorAttribute
    {
        public Align Align { get; private set; }

        public AlignAttribute(Align align)
        {
            Align = align;
        }
    }
}