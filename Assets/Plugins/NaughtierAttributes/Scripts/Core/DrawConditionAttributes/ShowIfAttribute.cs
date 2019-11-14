using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : BaseDrawConditionAttribute
    {
        public string ConditionName { get; private set; }
        
        public ShowIfAttribute(string conditionName)
        {
            this.ConditionName = conditionName;
        }
    }
}
