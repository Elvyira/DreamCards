using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public abstract class BaseClassAttribute : BaseNaughtierAttribute
    {
    }
}