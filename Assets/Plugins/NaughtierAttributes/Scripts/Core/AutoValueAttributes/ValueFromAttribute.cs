using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ValueFromAttribute : BaseAutoValueAttribute
    {
        public readonly string ValueName;

        public ValueFromAttribute(string valueName, bool playUpdate = false) : base(playUpdate)
        {
            ValueName = valueName;
        }
    }
}