using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class BaseAutoValueAttribute : BaseNaughtierAttribute
    {
        public readonly bool PlayUpdate;

        protected BaseAutoValueAttribute(bool playUpdate)
        {
            PlayUpdate = playUpdate;
        }
    }
}