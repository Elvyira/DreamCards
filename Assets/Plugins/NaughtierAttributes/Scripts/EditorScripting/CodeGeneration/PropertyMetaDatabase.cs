#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class PropertyMetaDatabase
    {
        private static Dictionary<Type, BasePropertyMeta> metasByAttributeType;

        static PropertyMetaDatabase()
        {
            metasByAttributeType = new Dictionary<Type, BasePropertyMeta>();
            metasByAttributeType[typeof(InfoBoxAttribute)] = new InfoBoxPropertyMeta();
metasByAttributeType[typeof(OnValueChangedAttribute)] = new OnValueChangedPropertyMeta();

        }

        public static BasePropertyMeta GetMetaForAttribute(Type attributeType)
        {
            BasePropertyMeta meta;
            if (metasByAttributeType.TryGetValue(attributeType, out meta))
            {
                return meta;
            }
            else
            {
                return null;
            }
        }
    }
}
#endif
