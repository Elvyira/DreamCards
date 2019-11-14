using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DisableIfAttribute : BaseDecoratorAttribute
    {
        public string ConditionName { get; private set; }

        public DisableIfAttribute(string conditionName)
        {
            this.ConditionName = conditionName;
        }
    }
}
