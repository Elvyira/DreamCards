#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace NaughtierAttributes.Editor
{
    [ArrayDrawer(typeof(ReorderableListAttribute))]
    public class ReorderableListArrayDrawer : BaseArrayDrawer
    {
        private readonly Dictionary<string, ReorderableList> _reorderableListsByPropertyScriptAndName =
            new Dictionary<string, ReorderableList>();

        public override void DrawArray(SerializedProperty property, IArrayElementDrawer drawer)
        {
            if (property.isArray)
            {
                var target = PropertyUtility.GetTargetObject(property);
                var attribute = PropertyUtility.GetAttribute<ReorderableListAttribute>(property);

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(-15);

                EditorGUILayout.BeginVertical(new GUIStyle
                {
                    padding = new RectOffset(15, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0)
                });

                if (!EditorDrawUtility.DrawArrayHeader(property))
                {
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    return;
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();

                if (!_reorderableListsByPropertyScriptAndName.ContainsKey(property.serializedObject.targetObject.GetType() + property.name))
                {
                    ReorderableList reorderableList = new ReorderableList(property.serializedObject, property, true, true,
                        attribute.drawButtons, attribute.drawButtons)
                    {
                        drawHeaderCallback = (Rect rect) =>
                        {
                            property.arraySize = Mathf.Max(0, EditorGUI.DelayedIntField(rect, "Size", property.arraySize));
                        },

                        drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                        {
                            var element = property.GetArrayElementAtIndex(index);
                            rect.y += 2f;

                            foreach (var elementDecoratorAttribute in PropertyUtility.GetAttributes<BaseElementDecoratorAttribute>(property))
                            {
                                ElementDecoratorDrawerDatabase.GetDrawerForAttribute(elementDecoratorAttribute.GetType())
                                    .BeginDrawElement(new Rect(rect.x, rect.y + 21, rect.width, 0), element, elementDecoratorAttribute,
                                        target);
                            }

                            if (drawer != null)
                            {
                                var height = drawer.GetElementHeight(property, element);
                                drawer.DrawElement(new Rect(rect.x, rect.y, rect.width, height), property, element, index);
                            }
                            else
                                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element);

                            foreach (var elementDecoratorAttribute in PropertyUtility.GetAttributes<BaseElementDecoratorAttribute>(property))
                                ElementDecoratorDrawerDatabase.GetDrawerForAttribute(elementDecoratorAttribute.GetType())
                                    .EndDrawElement(new Rect(rect.x, rect.y + 21, rect.width, 0), element, elementDecoratorAttribute,
                                        target);
                        },

                        elementHeightCallback = (index) =>
                        {
                            var element = property.GetArrayElementAtIndex(index);
                            var height = drawer != null ? drawer.GetElementHeight(property, element) : 20;

                            foreach (var elementDecoratorAttribute in PropertyUtility.GetAttributes<BaseElementDecoratorAttribute>(property))
                                height += ElementDecoratorDrawerDatabase.GetDrawerForAttribute(
                                    elementDecoratorAttribute.GetType()).GetElementHeight(element, elementDecoratorAttribute, target);

                            return height;
                        }
                    };

                    _reorderableListsByPropertyScriptAndName[property.serializedObject.targetObject.GetType() + property.name] =
                        reorderableList;
                }

                _reorderableListsByPropertyScriptAndName[property.serializedObject.targetObject.GetType() + property.name].DoLayoutList();
                EditorGUILayout.EndVertical();
                GUILayout.Space(-1);
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(-14);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(17);
                EditorGUILayout.EndVertical();
            }
            else
            {
                string warning = typeof(ReorderableListAttribute).Name + " can be used only on arrays or lists";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));

                EditorDrawUtility.DrawPropertyField(property);
            }
        }

        public override void ClearCache()
        {
            _reorderableListsByPropertyScriptAndName.Clear();
        }
    }
}
#endif