#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtierAttributes.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class NaughtierEditor : UnityEditor.Editor
    {
        private SerializedProperty script;

        private IEnumerable<FieldInfo> fields;
        private HashSet<FieldInfo> groupedFields;
        private HashSet<FieldInfo> conditionalGroupedFields;
        private Dictionary<string, List<FieldInfo>> groupedFieldsByGroupName;
        private IEnumerable<FieldInfo> nonSerializedFields;
        private IEnumerable<PropertyInfo> nativeProperties;
        private IEnumerable<MethodInfo> methods;

        private Dictionary<string, SerializedProperty> serializedPropertiesByFieldName;

        private bool useDefaultInspector;

        private void OnEnable()
        {
            script = serializedObject.FindProperty("m_Script");

            // Cache serialized fields
            fields = ReflectionUtility.GetAllFields(target, f => serializedObject.FindProperty(f.Name) != null);
            
            if (fields == null) return; 

            // If there are no NaughtyAttributes use default inspector
            if (fields.All(f => f.GetCustomAttributes(typeof(BaseNaughtierAttribute), true).Length == 0))
            {
                useDefaultInspector = true;
            }
            else
            {
                useDefaultInspector = false;

                // Cache grouped fields
                groupedFields =
                    new HashSet<FieldInfo>(fields.Where(f => f.GetCustomAttributes(typeof(BaseGroupAttribute), true).Length > 0));
                conditionalGroupedFields =
                    new HashSet<FieldInfo>(
                        fields.Where(f => f.GetCustomAttributes(typeof(BaseConditionalGroupAttribute), true).Length > 0));

                // Cache grouped fields by group name
                groupedFieldsByGroupName = new Dictionary<string, List<FieldInfo>>();
                foreach (var groupedField in groupedFields)
                {
                    string[] groupNames = groupedField.GetCustomAttributes(typeof(BaseGroupAttribute), true)
                        .Select(x => (x as BaseGroupAttribute).Name).ToArray();

                    foreach (var groupName in groupNames)
                    {
                        if (groupedFieldsByGroupName.ContainsKey(groupName))
                        {
                            groupedFieldsByGroupName[groupName].Add(groupedField);
                        }
                        else
                        {
                            groupedFieldsByGroupName[groupName] = new List<FieldInfo>()
                            {
                                groupedField
                            };
                        }
                    }
                }

                foreach (var conditionalGroupedField in conditionalGroupedFields)
                {
                    string[] groupNames = conditionalGroupedField.GetCustomAttributes(typeof(BaseConditionalGroupAttribute), true)
                        .Select(x => (x as BaseConditionalGroupAttribute).Name).ToArray();

                    foreach (var groupName in groupNames)
                    {
                        if (groupedFieldsByGroupName.ContainsKey(groupName))
                        {
                            groupedFieldsByGroupName[groupName].Add(conditionalGroupedField);
                        }
                        else
                        {
                            groupedFieldsByGroupName[groupName] = new List<FieldInfo>()
                            {
                                conditionalGroupedField
                            };
                        }
                    }
                }

                // Cache serialized properties by field name
                serializedPropertiesByFieldName = new Dictionary<string, SerializedProperty>();
                foreach (var field in fields)
                {
                    serializedPropertiesByFieldName[field.Name] = serializedObject.FindProperty(field.Name);
                }
            }

            // Cache non-serialized fields
            nonSerializedFields = ReflectionUtility.GetAllFields(
                target,
                f => f.GetCustomAttributes(typeof(BaseDrawerAttribute), true).Length > 0 && serializedObject.FindProperty(f.Name) == null);

            // Cache the native properties
            nativeProperties = ReflectionUtility.GetAllProperties(
                target, p => p.GetCustomAttributes(typeof(BaseDrawerAttribute), true).Length > 0);

            // Cache methods with DrawerAttribute
            methods = ReflectionUtility.GetAllMethods(
                target, m => m.GetCustomAttributes(typeof(BaseDrawerAttribute), true).Length > 0);
        }

        private void OnDisable()
        {
            PropertyDrawerDatabase.ClearCache();
            ArrayDrawerDatabase.ClearCache();
        }

        public override void OnInspectorGUI()
        {
            if (fields == null) OnEnable(); 
            if (useDefaultInspector)
            {
                DrawDefaultInspector();
            }
            else
            {
                serializedObject.Update();

                if (script != null)
                {
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(script);
                    GUI.enabled = true;
                }

                // Draw fields
                HashSet<string> drawnGroups = new HashSet<string>();
                foreach (var field in fields)
                {
                    if (groupedFields.Contains(field) || conditionalGroupedFields.Contains(field))
                    {
                        if (groupedFields.Contains(field))
                        {
                            // Draw grouped fields
                            string[] groupNames = field.GetCustomAttributes(typeof(BaseGroupAttribute), true)
                                .Select(x => (x as BaseGroupAttribute).Name).ToArray();

                            for (var i = 0; i < groupNames.Length; i++)
                            {
                                var groupName = groupNames[i];
                                if (!drawnGroups.Contains(groupName))
                                {
                                    drawnGroups.Add(groupName);

                                    BasePropertyGrouper grouper = GetPropertyGrouperForField(field, i);
                                    if (grouper != null)
                                    {
                                        grouper.BeginGroup(serializedPropertiesByFieldName[field.Name], groupName);

                                        ValidateAndDrawFields(groupedFieldsByGroupName[groupName]);

                                        grouper.EndGroup();
                                    }
                                    else
                                    {
                                        ValidateAndDrawFields(groupedFieldsByGroupName[groupName]);
                                    }
                                }
                            }
                        }

                        if (conditionalGroupedFields.Contains(field))
                        {
                            // Draw conditional grouped fields

                            string[] groupNames = field.GetCustomAttributes(typeof(BaseConditionalGroupAttribute), true)
                                .Select(x => (x as BaseConditionalGroupAttribute).Name).ToArray();
                            for (var i = 0; i < groupNames.Length; i++)
                            {
                                var groupName = groupNames[i];
                                if (!drawnGroups.Contains(groupName))
                                {
                                    drawnGroups.Add(groupName);

                                    BaseConditionalGrouper grouper = GetConditionnalGrouperForField(field, i);
                                    if (grouper != null)
                                    {
                                        var canDraw = grouper.CanDraw(serializedPropertiesByFieldName[field.Name], groupName);
                                        if (canDraw)
                                        {
                                            grouper.BeginGroup();
                                            ValidateAndDrawFields(groupedFieldsByGroupName[groupName]);
                                        }

                                        grouper.EndGroup(canDraw);
                                    }
                                    else
                                    {
                                        ValidateAndDrawFields(groupedFieldsByGroupName[groupName]);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Draw non-grouped field
                        ValidateAndDrawField(field);
                    }
                }

                serializedObject.ApplyModifiedProperties();
            }

            // Draw non-serialized fields
            foreach (var field in nonSerializedFields)
            {
                BaseDrawerAttribute baseDrawerAttribute = (BaseDrawerAttribute) field.GetCustomAttributes(typeof(BaseDrawerAttribute), true)[0];
                BaseFieldDrawer drawer = FieldDrawerDatabase.GetDrawerForAttribute(baseDrawerAttribute.GetType());
                if (drawer != null)
                {
                    drawer.DrawField(target, field);
                }
            }

            // Draw native properties
            foreach (var property in nativeProperties)
            {
                BaseDrawerAttribute baseDrawerAttribute = (BaseDrawerAttribute) property.GetCustomAttributes(typeof(BaseDrawerAttribute), true)[0];
                BaseNativePropertyDrawer drawer = NativePropertyDrawerDatabase.GetDrawerForAttribute(baseDrawerAttribute.GetType());
                if (drawer != null)
                {
                    drawer.DrawNativeProperty(target, property);
                }
            }

            // Draw methods
            foreach (var method in methods)
            {
                BaseDrawerAttribute baseDrawerAttribute = (BaseDrawerAttribute) method.GetCustomAttributes(typeof(BaseDrawerAttribute), true)[0];
                BaseMethodDrawer methodDrawer = MethodDrawerDatabase.GetDrawerForAttribute(baseDrawerAttribute.GetType());
                if (methodDrawer != null)
                {
                    methodDrawer.DrawMethod(target, method);
                }
            }
        }

        private void ValidateAndDrawFields(IEnumerable<FieldInfo> fields)
        {
            foreach (var field in fields)
            {
                ValidateAndDrawField(field);
            }
        }

        private void ValidateAndDrawField(FieldInfo field)
        {
            ValidateField(field);
            ApplyFieldMeta(field);
            DrawField(field);
        }

        private void ValidateField(FieldInfo field)
        {
            BaseValidatorAttribute[] validatorAttributes = (BaseValidatorAttribute[]) field.GetCustomAttributes(typeof(BaseValidatorAttribute), true);

            foreach (var attribute in validatorAttributes)
            {
                BasePropertyValidator validator = PropertyValidatorDatabase.GetValidatorForAttribute(attribute.GetType());
                if (validator != null)
                {
                    validator.ValidateProperty(serializedPropertiesByFieldName[field.Name]);
                }
            }
        }

        private void DrawField(FieldInfo field)
        {
            SerializedProperty property = serializedPropertiesByFieldName[field.Name];

            // Check if the field has draw conditions
            BasePropertyDrawCondition drawCondition = GetPropertyDrawConditionForField(field);
            if (drawCondition != null)
            {
                bool canDrawProperty = drawCondition.CanDrawProperty(property);
                if (!canDrawProperty)
                {
                    return;
                }
            }

            // Check if the field has HideInInspectorAttribute
            HideInInspector[] hideInInspectorAttributes = (HideInInspector[]) field.GetCustomAttributes(typeof(HideInInspector), true);
            if (hideInInspectorAttributes.Length > 0)
            {
                return;
            }

            // Draw the field
            EditorGUI.BeginChangeCheck();

            BaseDecoratorDrawer[] decoratorDrawers = GetDecoratorDrawersForField(field);
            for (var i = 0; i < decoratorDrawers.Length; i++)
                decoratorDrawers[i].BeginDraw(property);

            BaseElementDecoratorDrawer[] elementDecoratorDrawers = GetElementDecoratorDrawersForField(field);
            var length = elementDecoratorDrawers.Length;
            BaseArrayDrawer arrayDrawer = GetArrayDrawerForField(field);
            bool hasArrayDrawer = arrayDrawer != null;
            if (!hasArrayDrawer)
                for (var i = 0; i < length - 1; i++)
                    elementDecoratorDrawers[i].BeginDraw(property);

            BasePropertyDrawer drawer = GetPropertyDrawerForField(field);
            BaseAutoValueDrawer autoValueDrawer = GetAutoValueDrawerForField(field);
            if (!hasArrayDrawer)
            {
                if (autoValueDrawer != null)
                {
                    var state = autoValueDrawer.InitProperty(ref property);
                    if (drawer == null)
                        DrawPropertyField(length > 0, property, p => autoValueDrawer.DrawProperty(p, state),
                            elementDecoratorDrawers);
                }

                if (drawer != null)
                    DrawPropertyField(length > 0, property, drawer.DrawProperty, elementDecoratorDrawers);
                else if (autoValueDrawer == null)
                    DrawPropertyField(length > 0, property, p => EditorDrawUtility.DrawPropertyField(p),
                        elementDecoratorDrawers);
            }

            if (hasArrayDrawer)
            {
                if (autoValueDrawer != null)
                    autoValueDrawer.InitProperty(ref property);
                arrayDrawer.DrawArray(property, drawer as IArrayElementDrawer);
            }

            if (!hasArrayDrawer)
                for (var i = length - 2; i >= 0; i--)
                    elementDecoratorDrawers[i].EndDraw(property);

            for (var i = decoratorDrawers.Length - 1; i >= 0; i--)
                decoratorDrawers[i].EndDraw(property);

            if (EditorGUI.EndChangeCheck())
            {
                OnValueChangedAttribute[] onValueChangedAttributes =
                    (OnValueChangedAttribute[]) field.GetCustomAttributes(typeof(OnValueChangedAttribute), true);
                foreach (var onValueChangedAttribute in onValueChangedAttributes)
                {
                    BasePropertyMeta meta = PropertyMetaDatabase.GetMetaForAttribute(onValueChangedAttribute.GetType());
                    if (meta != null)
                    {
                        meta.ApplyPropertyMeta(property, onValueChangedAttribute);
                    }
                }
            }
        }

        private void DrawPropertyField(bool condition, SerializedProperty property, Action<SerializedProperty> drawCallback,
            BaseElementDecoratorDrawer[] elementDecoratorDrawers)
        {
            if (condition)
            {
                elementDecoratorDrawers.Last().BeginDraw(property, drawCallback);
                elementDecoratorDrawers.Last().EndDraw(property, drawCallback);
            }
            else
            {
                drawCallback(property);
            }
        }

        private void ApplyFieldMeta(FieldInfo field)
        {
            // Apply custom meta attributes
            BaseMetaAttribute[] metaAttributes = field
                .GetCustomAttributes(typeof(BaseMetaAttribute), true)
                .Where(attr => attr.GetType() != typeof(OnValueChangedAttribute))
                .Select(obj => obj as BaseMetaAttribute)
                .ToArray();

            Array.Sort(metaAttributes, (x, y) => { return x.Order - y.Order; });

            foreach (var metaAttribute in metaAttributes)
            {
                BasePropertyMeta meta = PropertyMetaDatabase.GetMetaForAttribute(metaAttribute.GetType());
                if (meta != null)
                {
                    meta.ApplyPropertyMeta(serializedPropertiesByFieldName[field.Name], metaAttribute);
                }
            }
        }

        private BaseElementDecoratorDrawer[] GetElementDecoratorDrawersForField(FieldInfo field)
        {
            BaseElementDecoratorAttribute[] drawerAttributes =
                (BaseElementDecoratorAttribute[]) field.GetCustomAttributes(typeof(BaseElementDecoratorAttribute), true);
            BaseElementDecoratorDrawer[] drawers = new BaseElementDecoratorDrawer[drawerAttributes.Length];
            for (var i = 0; i < drawerAttributes.Length; i++)
                drawers[i] = ElementDecoratorDrawerDatabase.GetDrawerForAttribute(drawerAttributes[i].GetType());
            return drawers;
        }

        private BaseDecoratorDrawer[] GetDecoratorDrawersForField(FieldInfo field)
        {
            BaseDecoratorAttribute[] drawerAttributes = (BaseDecoratorAttribute[]) field.GetCustomAttributes(typeof(BaseDecoratorAttribute), true);
            BaseDecoratorDrawer[] drawers = new BaseDecoratorDrawer[drawerAttributes.Length];
            for (var i = 0; i < drawerAttributes.Length; i++)
                drawers[i] = DecoratorDrawerDatabase.GetDrawerForAttribute(drawerAttributes[i].GetType());
            return drawers;
        }

        private BasePropertyDrawer GetPropertyDrawerForField(FieldInfo field)
        {
            BaseDrawerAttribute[] drawerAttributes = (BaseDrawerAttribute[]) field.GetCustomAttributes(typeof(BaseDrawerAttribute), true);
            if (drawerAttributes.Length > 0)
            {
                BasePropertyDrawer drawer = PropertyDrawerDatabase.GetDrawerForAttribute(drawerAttributes[0].GetType());
                return drawer;
            }
            else
            {
                return null;
            }
        }

        private BaseArrayDrawer GetArrayDrawerForField(FieldInfo field)
        {
            BaseArrayAttribute[] drawerAttributes = (BaseArrayAttribute[]) field.GetCustomAttributes(typeof(BaseArrayAttribute), true);
            if (drawerAttributes.Length > 0)
            {
                BaseArrayDrawer drawer = ArrayDrawerDatabase.GetDrawerForAttribute(drawerAttributes[0].GetType());
                return drawer;
            }
            else
            {
                return null;
            }
        }

        private BaseAutoValueDrawer GetAutoValueDrawerForField(FieldInfo field)
        {
            BaseAutoValueAttribute[] drawerAttributes = (BaseAutoValueAttribute[]) field.GetCustomAttributes(typeof(BaseAutoValueAttribute), true);
            if (drawerAttributes.Length > 0)
            {
                BaseAutoValueDrawer drawer = AutoValueDrawerDatabase.GetDrawerForAttribute(drawerAttributes[0].GetType());
                return drawer;
            }
            else
            {
                return null;
            }
        }

        private BasePropertyGrouper GetPropertyGrouperForField(FieldInfo field, int index)
        {
            BaseGroupAttribute[] groupAttributes = (BaseGroupAttribute[]) field.GetCustomAttributes(typeof(BaseGroupAttribute), true);
            if (groupAttributes.Length > 0)
            {
                BasePropertyGrouper drawer = PropertyGrouperDatabase.GetGrouperForAttribute(groupAttributes[index].GetType());
                return drawer;
            }
            else
            {
                return null;
            }
        }

        private BaseConditionalGrouper GetConditionnalGrouperForField(FieldInfo field, int index)
        {
            BaseConditionalGroupAttribute[] groupAttributes =
                (BaseConditionalGroupAttribute[]) field.GetCustomAttributes(typeof(BaseConditionalGroupAttribute), true);
            if (groupAttributes.Length > 0)
            {
                BaseConditionalGrouper drawer = ConditionalGrouperDatabase.GetGrouperForAttribute(groupAttributes[index].GetType());
                return drawer;
            }
            else
            {
                return null;
            }
        }

        private BasePropertyDrawCondition GetPropertyDrawConditionForField(FieldInfo field)
        {
            BaseDrawConditionAttribute[] drawConditionAttributes =
                (BaseDrawConditionAttribute[]) field.GetCustomAttributes(typeof(BaseDrawConditionAttribute), true);
            if (drawConditionAttributes.Length > 0)
            {
                BasePropertyDrawCondition drawCondition =
                    PropertyDrawConditionDatabase.GetDrawConditionForAttribute(drawConditionAttributes[0].GetType());
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