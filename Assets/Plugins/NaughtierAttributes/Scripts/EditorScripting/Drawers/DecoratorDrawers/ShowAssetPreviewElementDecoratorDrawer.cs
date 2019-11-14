#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace NaughtierAttributes.Editor
{
    [ElementDecoratorDrawer(typeof(ShowAssetPreviewAttribute))]
    public class ShowAssetPreviewElementDecoratorDrawer : BaseElementDecoratorDrawer
    {
        private Color _previousBackgroundColor, _previousContentColor;

        public override void BeginDraw(SerializedProperty property, Action<SerializedProperty> propertyDrawCallback = null)
        {
            var target = PropertyUtility.GetTargetObject(property);
            var attribute = PropertyUtility.GetAttribute<ShowAssetPreviewAttribute>(property);

            if (property.isArray)
            {
                if (!EditorDrawUtility.DrawArrayHeader(property)) return;

                BeginDrawElement(property, attribute, target, true);

                EditorGUI.indentLevel++;
                EditorDrawUtility.DrawArraySizeField(property);
                return;
            }

            BeginDrawElement(property, attribute, target, false);
        }

        public override void EndDraw(SerializedProperty property, Action<SerializedProperty> propertyDrawCallback = null)
        {
            var target = PropertyUtility.GetTargetObject(property);
            var attribute = PropertyUtility.GetAttribute<ShowAssetPreviewAttribute>(property);
            if (property.isArray)
            {
                if (!property.isExpanded) return;
                
                EditorDrawUtility.DrawArrayBody(property, index =>
                {
                    var element = property.GetArrayElementAtIndex(index);
                    if (propertyDrawCallback != null)
                        propertyDrawCallback.Invoke(element);
                    DrawElement(element, attribute);
                });

                EditorGUI.indentLevel--;
                EndDrawElement(property, attribute, target, true);

                return;
            }

            if (propertyDrawCallback != null)
                propertyDrawCallback.Invoke(property);

            EndDrawElement(property, attribute, target, false);
        }

        public override void BeginDrawElement(Rect rect, SerializedProperty property, BaseElementDecoratorAttribute baseElementDecoratorAttribute,
            Object target)
        {
            if (property.isArray) return;

            var attribute = baseElementDecoratorAttribute as ShowAssetPreviewAttribute;

            _previousBackgroundColor = GUI.backgroundColor;
            _previousContentColor = GUI.contentColor;

            GUI.backgroundColor = EditorDrawUtility.GetColor(target, attribute.BackgroundColor, GUI.backgroundColor);
            GUI.contentColor = EditorDrawUtility.GetColor(target, attribute.ContentColor, GUI.contentColor);
        }

        public override void EndDrawElement(Rect rect, SerializedProperty property, BaseElementDecoratorAttribute baseElementDecoratorAttribute,
            Object target)
        {
            if (property.isArray) return;
            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null)
            {
                Texture2D previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
                if (previewTexture != null)
                {
                    var attribute = baseElementDecoratorAttribute as ShowAssetPreviewAttribute;

                    float textureRatio = (float) previewTexture.width / previewTexture.height;

                    int width = Mathf.Clamp(attribute.Size, 0, previewTexture.width);
                    int height = Mathf.Clamp((int) (attribute.Size / textureRatio), 0, previewTexture.height);

                    Rect newRect = Rect.zero;
                    switch (attribute.Align)
                    {
                        case Align.Left:
                            newRect = new Rect(rect.x + 5, rect.y + 5, width, height);
                            break;
                        case Align.Center:
                            newRect = new Rect(Screen.width / 2f - width / 2f, rect.y + 5, width, height);
                            break;
                        case Align.Right:
                            newRect = new Rect(Screen.width - width - 29, rect.y + 5, width, height);
                            break;
                    }

                    GUI.Box(new Rect(rect.x, rect.y, rect.width, height + 9), "");
                    GUI.Box(new Rect(rect.x, rect.y, rect.width, height + 9), "");
                    GUI.Label(new Rect(newRect.x, newRect.y, newRect.width, newRect.height), previewTexture);
                }
                else
                {
                    string warning = property.name + " doesn't have an asset preview";
                    EditorDrawUtility.DrawHelpBox(new Rect(rect.x, rect.y, rect.width, 40), warning, MessageType.Warning,
                        logToConsole: true,
                        context: PropertyUtility.GetTargetObject(property));
                }
            }
            else
            {
                string warning = property.name + " doesn't have an asset preview";
                EditorDrawUtility.DrawHelpBox(new Rect(rect.x, rect.y, rect.width, 40), warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));
            }

            GUI.backgroundColor = _previousBackgroundColor;
            GUI.contentColor = _previousContentColor;
        }

        public override void BeginDrawElement(SerializedProperty property, BaseElementDecoratorAttribute baseElementDecoratorAttribute,
            Object target,
            bool fromArray)
        {
            var attribute = PropertyUtility.GetAttribute<ShowAssetPreviewAttribute>(property);

            _previousBackgroundColor = GUI.backgroundColor;
            _previousContentColor = GUI.contentColor;

            GUI.backgroundColor = EditorDrawUtility.GetColor(target, attribute.BackgroundColor, GUI.backgroundColor);
            GUI.contentColor = EditorDrawUtility.GetColor(target, attribute.ContentColor, GUI.contentColor);

            EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 3, 3)
            });
            EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0)
            });
            EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0)
            });
        }

        public override void EndDrawElement(SerializedProperty property, BaseElementDecoratorAttribute baseElementDecoratorAttribute, Object target,
            bool fromArray)
        {
            if (!fromArray)
                DrawElement(property, baseElementDecoratorAttribute as ShowAssetPreviewAttribute);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = _previousBackgroundColor;
            GUI.contentColor = _previousContentColor;
        }

        public override float GetElementHeight(SerializedProperty property, BaseElementDecoratorAttribute baseElementDecoratorAttribute,
            Object target)
        {
            if (property.isArray) return 0;

            if (property.propertyType != SerializedPropertyType.ObjectReference || property.objectReferenceValue == null) return 50;

            Texture2D previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);

            if (previewTexture == null) return 50;

            var attribute = baseElementDecoratorAttribute as ShowAssetPreviewAttribute;
            float textureRatio = (float) previewTexture.width / previewTexture.height;
            return Mathf.Clamp(attribute.Size / textureRatio, 0, previewTexture.height) + 15;
        }

        private void DrawElement(SerializedProperty property, ShowAssetPreviewAttribute attribute)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null)
            {
                Texture2D previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
                if (previewTexture != null)
                {
                    int width = Mathf.Clamp(attribute.Size, 0, previewTexture.width);
                    int height = Mathf.Clamp(attribute.Size, 0, previewTexture.height);
                    GUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
                    {
                        margin = new RectOffset(10, 10, 5, 5),
                        padding = new RectOffset(0, 0, 0, 0)
                    });
                    GUILayout.BeginVertical(new GUIStyle(GUI.skin.box)
                    {
                        margin = new RectOffset(0, 0, 0, 0),
                        padding = new RectOffset(5, 5, 5, 0)
                    });
                    EditorDrawUtility.DrawWithAlign(attribute.Align,
                        () =>
                        {
                            if (previewTexture != null)
                                GUILayout.Label(previewTexture, GUILayout.MaxWidth(width), GUILayout.MaxHeight(height));
                        });
                    GUILayout.EndVertical();
                    GUILayout.EndVertical();
                }
                else
                {
                    string warning = property.name + " doesn't have an asset preview";
                    EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true,
                        context: PropertyUtility.GetTargetObject(property));
                }
            }
            else
            {
                string warning = property.name + " doesn't have an asset preview";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true,
                    context: PropertyUtility.GetTargetObject(property));
            }
        }
    }
}
#endif