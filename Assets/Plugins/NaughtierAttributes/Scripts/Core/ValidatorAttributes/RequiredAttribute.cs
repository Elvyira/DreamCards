using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RequiredAttribute : BaseValidatorAttribute
    {
        public string Message { get; private set; }

        public RequiredAttribute(string message = null)
        {
            this.Message = message;
        }
    }
}
