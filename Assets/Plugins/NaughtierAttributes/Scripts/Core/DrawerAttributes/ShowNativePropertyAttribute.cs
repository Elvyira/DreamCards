using System;
using UnityEngine;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ShowNativePropertyAttribute : BaseDrawerAttribute
    {
    }
}
