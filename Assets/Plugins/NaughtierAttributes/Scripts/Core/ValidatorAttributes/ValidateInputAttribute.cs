using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ValidateInputAttribute : BaseValidatorAttribute
    {
        public string CallbackName { get; private set; }
        public string Message { get; private set; }

        public ValidateInputAttribute(string callbackName, string message = null)
        {
            this.CallbackName = callbackName;
            this.Message = message;
        }
    }
}
