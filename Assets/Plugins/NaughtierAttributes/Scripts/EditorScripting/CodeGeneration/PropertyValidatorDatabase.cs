#if UNITY_EDITOR
// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    public static class PropertyValidatorDatabase
    {
        private static Dictionary<Type, BasePropertyValidator> validatorsByAttributeType;

        static PropertyValidatorDatabase()
        {
            validatorsByAttributeType = new Dictionary<Type, BasePropertyValidator>();
            validatorsByAttributeType[typeof(MaxValueAttribute)] = new MaxValuePropertyValidator();
validatorsByAttributeType[typeof(MinValueAttribute)] = new MinValuePropertyValidator();
validatorsByAttributeType[typeof(RequiredAttribute)] = new RequiredPropertyValidator();
validatorsByAttributeType[typeof(ValidateInputAttribute)] = new ValidateInputPropertyValidator();

        }

        public static BasePropertyValidator GetValidatorForAttribute(Type attributeType)
        {
            BasePropertyValidator validator;
            if (validatorsByAttributeType.TryGetValue(attributeType, out validator))
            {
                return validator;
            }
            else
            {
                return null;
            }
        }
    }
}
#endif
