#if UNITY_EDITOR
using System;
using MightyAttributes.Editor;

public class CustomWrapperDrawer : BaseWrapperDrawer
{
    public T[] GetAttributes<T>(Type type) where T : Attribute => GetAttributes(type, typeof(T)) as T[];

    public object[] GetAttributes(Type type, Type attributeType) => type.GetCustomAttributes(attributeType, true);
}
#endif