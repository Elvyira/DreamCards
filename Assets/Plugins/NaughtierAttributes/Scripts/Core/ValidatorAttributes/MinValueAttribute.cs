using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinValueAttribute : BaseValidatorAttribute
    {
        public float MinValue { get; private set; }

        public MinValueAttribute(float minValue)
        {
            this.MinValue = minValue;
        }

        public MinValueAttribute(int minValue)
        {
            this.MinValue = minValue;
        }
    }
}
