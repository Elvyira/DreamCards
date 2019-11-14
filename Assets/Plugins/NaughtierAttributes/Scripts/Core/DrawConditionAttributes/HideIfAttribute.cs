using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HideIfAttribute : BaseDrawConditionAttribute
    {
        public string ConditionName { get; private set; }

        public HideIfAttribute(string conditionName)
        {
            this.ConditionName = conditionName;
        }
    }
}
