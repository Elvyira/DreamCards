﻿using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowNonSerializedFieldAttribute : BaseDrawerAttribute
    {
    }
}
