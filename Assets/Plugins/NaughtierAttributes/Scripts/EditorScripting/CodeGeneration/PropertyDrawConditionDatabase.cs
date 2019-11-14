#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class PropertyDrawConditionDatabase
    {
        private static Dictionary<Type, BasePropertyDrawCondition> drawConditionsByAttributeType;

        static PropertyDrawConditionDatabase()
        {
            drawConditionsByAttributeType = new Dictionary<Type, BasePropertyDrawCondition>();
            drawConditionsByAttributeType[typeof(HideIfAttribute)] = new HideIfPropertyDrawCondition();
drawConditionsByAttributeType[typeof(ShowIfAttribute)] = new ShowIfPropertyDrawCondition();

        }

        public static BasePropertyDrawCondition GetDrawConditionForAttribute(Type attributeType)
        {
            BasePropertyDrawCondition drawCondition;
            if (drawConditionsByAttributeType.TryGetValue(attributeType, out drawCondition))
            {
                return drawCondition;
            }
            else
            {
                return null;
            }
        }
    }
}
#endif
