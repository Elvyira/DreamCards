using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaxValueAttribute : BaseValidatorAttribute
    {
        public float MaxValue { get; private set; }

        public MaxValueAttribute(float maxValue)
        {
            this.MaxValue = maxValue;
        }

        public MaxValueAttribute(int maxValue)
        {
            this.MaxValue = maxValue;
        }
    }
}
