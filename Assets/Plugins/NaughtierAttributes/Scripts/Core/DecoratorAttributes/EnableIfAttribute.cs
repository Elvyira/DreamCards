using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnableIfAttribute : BaseDecoratorAttribute
    {
        public string ConditionName { get; private set; }

        public EnableIfAttribute(string conditionName)
        {
            this.ConditionName = conditionName;
        }
    }
}
