#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [PropertyMeta(typeof(OnValueChangedAttribute))]
    public class OnValueChangedPropertyMeta : BasePropertyMeta
    {
        public override void ApplyPropertyMeta(SerializedProperty property, BaseMetaAttribute metaAttribute)
        {
            OnValueChangedAttribute onValueChangedAttribute = (OnValueChangedAttribute)metaAttribute;
            Object target = PropertyUtility.GetTargetObject(property);

            MethodInfo callbackMethod = ReflectionUtility.GetMethod(target, onValueChangedAttribute.CallbackName);
            if (callbackMethod != null &&
                callbackMethod.ReturnType == typeof(void) &&
                callbackMethod.GetParameters().Length == 0)
            {
                property.serializedObject.ApplyModifiedProperties(); // We must apply modifications so that the callback can be invoked with up-to-date data

                callbackMethod.Invoke(target, null);
            }
            else
            {
                string warning = onValueChangedAttribute.GetType().Name + " can invoke only action methods - with void return type and no parameters";
                Debug.LogWarning(warning, target);
            }
        }
    }
}
#endif