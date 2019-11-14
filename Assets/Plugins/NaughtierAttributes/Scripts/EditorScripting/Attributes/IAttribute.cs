#if UNITY_EDITOR
using System;

namespace NaughtierAttributes.Editor
{
    public interface IAttribute
    {
        Type TargetAttributeType { get; }
    }
}
#endif