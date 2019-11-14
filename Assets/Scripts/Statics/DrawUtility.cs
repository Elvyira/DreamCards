
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using System.Text.RegularExpressions;
using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[Flags]
public enum Indent
{
    All = -1,
    Nothing = 0,
    Label = 1,
    Content = 2
}

public static class IconName
{
    public const string EYE = "ViewToolOrbit";
    public const string CS_SCRIPT_ICON = "cs Script Icon";
    public const string REFRESH = "TreeEditor.Refresh";
    public const string SAVE = "SaveActive";
    public const string TRASH = "TreeEditor.Trash";
    public const string PLUS = "Toolbar Plus";
    public const string MINUS = "Toolbar Minus";
    public const string RECORD = "Animation.Record";
    public const string PLAY = "Animation.Play";
    public const string LOCK = "LockIcon-On";
    public const string UNLOCK = "LockIcon";
    public const string MULTIPLE = "TimelineSelector";
}

public class IconLabelWindow : EditorWindow
{
    private static IconLabelWindow m_instance;

    private string m_label;
    private int m_fontSize;
    private Color m_contentColor;
    private Action<int> m_onCloseEvent;
    private Action m_extraDrawAction;

    public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Vector2? position = null) =>
        Init(iconName, label, fontSize, contentColor, new Vector2(35 + label.Length * 5, 35), position);

    public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Action<int> onCloseEvent) =>
        Init(iconName, label, fontSize, contentColor, new Vector2(35 + label.Length * 5, 35), onCloseEvent);

    public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Vector2? position,
        Action<int> onCloseEvent) => Init(iconName, label, fontSize, contentColor, new Vector2(35 + label.Length * 5, 35), position,
        onCloseEvent);

    public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Vector2 size,
        Vector2? position = null) => BaseInit(iconName, label, fontSize, contentColor, size, position);

    public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Vector2 size,
        Action<int> onCloseEvent) => BaseInit(iconName, label, fontSize, contentColor, size, null, onCloseEvent);

    public static IconLabelWindow Init(string iconName, string label, int fontSize, Color contentColor, Vector2 size, Vector2? position,
        Action<int> onCloseEvent) => BaseInit(iconName, label, fontSize, contentColor, size, position, onCloseEvent);

    private static IconLabelWindow BaseInit(string iconName, string label, int fontSize, Color contentColor, Vector2 size,
        Vector2? position = null, Action<int> onCloseEvent = null)
    {
        m_instance = GetWindow<IconLabelWindow>();
        m_instance.m_label = label;
        m_instance.m_fontSize = fontSize;
        m_instance.m_contentColor = contentColor;
        m_instance.titleContent = DrawUtility.DrawIcon(iconName);
        m_instance.maxSize = m_instance.minSize = size;
        m_instance.maximized = false;
        if (position != null)
            m_instance.position = new Rect((Vector2) position, size);
        m_instance.m_onCloseEvent = onCloseEvent;
        m_instance.Show();
        return m_instance;
    }

    public static void AddExtraDraw(IconLabelWindow instance, Action extraDrawAction) =>
        AddExtraDraw(instance.GetInstanceID(), extraDrawAction);

    public static void AddExtraDraw(int instanceID, Action extraDrawAction)
    {
        if (m_instance != null && m_instance.GetInstanceID() == instanceID)
            m_instance.m_extraDrawAction = extraDrawAction;
    }

    public static void CloseWindow(IconLabelWindow instance) => CloseWindow(instance.GetInstanceID());

    public static void CloseWindow(int instanceID)
    {
        if (m_instance != null && m_instance.GetInstanceID() == instanceID)
            m_instance.Close();
    }

    private void OnDestroy()
    {
        m_extraDrawAction = null;
        m_onCloseEvent?.Invoke(m_instance.GetInstanceID());
    }

    private void OnGUI()
    {
        GUILayout.FlexibleSpace();
        DrawUtility.DrawHorizontal(() =>
        {
            GUILayout.FlexibleSpace();
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = m_fontSize,
                fontStyle = FontStyle.Bold,
            };
            style.normal.textColor = style.active.textColor = style.focused.textColor = style.hover.textColor = m_contentColor;

            GUILayout.Label(m_label, style);
            GUILayout.FlexibleSpace();
        });
        GUILayout.FlexibleSpace();
        m_extraDrawAction?.Invoke();
    }
}

#endif

public static class DrawUtility
{
#if UNITY_EDITOR

    #region Private

    public const float TAB_SIZE = 15;

    private static float IndentSpace => TAB_SIZE * EditorGUI.indentLevel;

    private static GUIStyle Skin => EditorGUIUtility.isProSkin ? GUI.skin.textField : GUI.skin.box;

    private static readonly GUIStyle HorizontalLine = new GUIStyle
    {
        normal = {background = EditorGUIUtility.whiteTexture},
        margin = new RectOffset(0, 0, 4, 4),
        fixedHeight = 2
    };

    private static readonly GUIContent[] MinMaxContent = {EditorGUIUtility.TrTextContent("Min"), EditorGUIUtility.TrTextContent("Max")};
    private static readonly float[] MinMaxWidths = {25, 27};

    private static readonly GUIContent[] MarginsContent =
    {
        EditorGUIUtility.TrTextContent("Left"),
        EditorGUIUtility.TrTextContent("Top"),
        EditorGUIUtility.TrTextContent("Right"),
        EditorGUIUtility.TrTextContent("Bottom")
    };

    private static readonly float[] MarginsWidths = {35, 27, 40, 45};

    private static readonly GUIContent[] AreaContent =
    {
        EditorGUIUtility.TrTextContent("Left"),
        EditorGUIUtility.TrTextContent("Right"),
        EditorGUIUtility.TrTextContent("Bottom"),
        EditorGUIUtility.TrTextContent("Top")
    };

    private static readonly float[] AreaWidths = {35, 40, 45, 27};

    private static Color InactiveColor => EditorGUIUtility.isProSkin ? new Color(.8f, .8f, .8f) : Color.grey;
    private static Color InactiveContentColor => EditorGUIUtility.isProSkin ? new Color(.9f, .9f, .9f) : Color.white;

    private static void DrawBox(string label, Indent indent, GUIStyle labelStyle = null)
    {
        var contentColor = GUI.contentColor;
        GUI.contentColor = contentColor;
        BeginGUILayoutIndent();
        GUILayout.BeginVertical(Skin);
        if (indent != Indent.Nothing) EditorGUI.indentLevel++;
        EditorGUILayout.Foldout(true, label, true, labelStyle ?? EditorStyles.largeLabel);
        if (indent == Indent.Label) EditorGUI.indentLevel--;
        GUI.contentColor = contentColor;
    }

    private static bool DrawBox(string label, ref bool unfold, Indent indent, GUIStyle labelStyle = null, bool otherUnfoldCondition = false)
    {
        var color = GUI.color;
        var contentColor = GUI.contentColor;
        GUI.color = unfold ? color : InactiveColor;
        GUI.contentColor = unfold ? contentColor : InactiveContentColor;
        BeginGUILayoutIndent();
        GUILayout.BeginVertical(Skin);
        if (indent != Indent.Nothing) EditorGUI.indentLevel++;
        unfold = EditorGUILayout.Foldout(unfold, label, true, labelStyle ?? EditorStyles.largeLabel) || otherUnfoldCondition;
        if (indent == Indent.Label) EditorGUI.indentLevel--;
        GUI.color = color;
        GUI.contentColor = contentColor;
        return unfold;
    }

    private static void EndDrawVertical(Indent indent, bool indented = true)
    {
        if (indented && indent.HasFlag(Indent.Content)) EditorGUI.indentLevel--;
        GUILayout.Space(1);
        GUILayout.EndVertical();
    }

    private static void DrawGUILayoutIndent() => GUILayout.Space(IndentSpace);

    private static bool LabelHasContent(GUIContent label) => label == null || label.text != string.Empty || label.image != null;

    private static Rect MultiFieldPrefixLabel(Rect totalPosition, int id, GUIContent label, int columns)
    {
        if (!LabelHasContent(label))
            return EditorGUI.IndentedRect(totalPosition);
        if (EditorGUIUtility.wideMode)
        {
            var labelPosition = new Rect(totalPosition.x + IndentSpace, totalPosition.y, EditorGUIUtility.labelWidth - IndentSpace, 16);
            var rect = totalPosition;
            rect.xMin += EditorGUIUtility.labelWidth;
            if (columns > 1)
            {
                --labelPosition.width;
                --rect.xMin;
            }

            if (columns == 2)
            {
                var num = (float) (((double) rect.width - 4.0) / 3.0);
                rect.xMax -= num + 2f;
            }

            EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, id);
            return rect;
        }

        var labelPosition1 = new Rect(totalPosition.x + IndentSpace, totalPosition.y, totalPosition.width - IndentSpace, 16f);
        var rect1 = totalPosition;
        rect1.xMin += IndentSpace + 15f;
        rect1.yMin += 16f;
        EditorGUI.HandlePrefixLabel(totalPosition, labelPosition1, label, id);
        return rect1;
    }

    #endregion /Private

    #region Public

    public static string DrawPrettyName(string name) => Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");

    public static void DrawVertical(Action drawAction, GUIStyle style, params GUILayoutOption[] options)
    {
        GUILayout.BeginVertical(style, options);
        drawAction();
        GUILayout.EndVertical();
    }

    public static void DrawVertical(Action drawAction, params GUILayoutOption[] options)
    {
        GUILayout.BeginVertical(options);
        drawAction();
        GUILayout.EndVertical();
    }

    public static void DrawHorizontal(Action drawAction, GUIStyle style, params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal(style, options);
        drawAction();
        GUILayout.EndHorizontal();
    }

    public static void DrawHorizontal(Action drawAction, params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal(options);
        drawAction();
        GUILayout.EndHorizontal();
    }

    public static Vector2 DrawScrollView(Vector2 scrollPosition, Action drawAction, params GUILayoutOption[] options)
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, options);
        drawAction();
        EditorGUILayout.EndScrollView();
        return scrollPosition;
    }

    public static void ReadOnlyDraw(Action drawAction, bool enable = false, bool condition = true)
    {
        var enabled = GUI.enabled;
        if (condition)
            GUI.enabled = enable;

        drawAction();

        if (condition)
            GUI.enabled = enabled;
    }

    public static void DrawGUILayoutIndented(Action drawAction)
    {
        BeginGUILayoutIndent();
        drawAction();
        EndGUILayoutIndent();
    }

    public static void BeginGUILayoutIndent()
    {
        GUILayout.BeginHorizontal();
        DrawGUILayoutIndent();
    }

    public static void EndGUILayoutIndent() => GUILayout.EndHorizontal();

    public static void DrawBoxList(SerializedProperty property, GUIContent[] elementsLabels = null, Indent indent = Indent.All,
        GUIStyle labelStyle = null)
    {
        if (!property.isArray)
        {
            EditorGUILayout.HelpBox($"{property.displayName} isn't array", MessageType.Error);
            return;
        }

        if (elementsLabels != null && elementsLabels.Length != property.arraySize)
        {
            EditorGUILayout.HelpBox("Elements labels length is invalid", MessageType.Error);
            return;
        }

        Undo.RecordObject(property.serializedObject.targetObject, "Box Array Property");

        var propertyIsExpanded = property.isExpanded;
        DrawBoxList(property.displayName, property.arraySize, ref propertyIsExpanded, () =>
        {
            for (var i = 0; i < property.arraySize; i++)
            {
                var element = property.GetArrayElementAtIndex(i);
                if (elementsLabels != null)
                    EditorGUILayout.PropertyField(element, elementsLabels[i]);
                else
                    EditorGUILayout.PropertyField(element);
            }
        }, indent, labelStyle);
        property.serializedObject.TryApplyModifiedProperties();
        property.isExpanded = propertyIsExpanded;
    }

    public static void DrawBoxList(string label, int length, ref bool unfold, Action drawAction, Indent indent = Indent.All,
        GUIStyle labelStyle = null)
    {
        if (BeginDrawBoxList(label, length, ref unfold, indent, labelStyle)) drawAction();
        EndDrawBoxList(indent);
    }

    public static void BeginDrawBoxList(string label, bool drawHorizontalLine, Indent indent = Indent.All, GUIStyle labelStyle = null)
    {
        DrawBox(label, indent, labelStyle);
        if (drawHorizontalLine) DrawHorizontalLine();
    }

    public static bool BeginDrawBoxList(string label, int length, ref bool unfold, Indent indent = Indent.All, GUIStyle labelStyle = null)
    {
        if (!DrawBox(label, ref unfold, indent, labelStyle, length == 0))
            return false;

        if (length > 0)
            DrawHorizontalLine();
        return true;
    }

    public static void EndDrawBoxList(Indent indent = Indent.All)
    {
        EndDrawVertical(indent);
        EndGUILayoutIndent();
    }

    public static void DrawFoldableBox(string label, ref bool unfold, Action drawAction, Indent indent = Indent.Label,
        GUIStyle labelStyle = null)
    {
        if (BeginDrawFoldableBox(label, ref unfold, indent, labelStyle)) drawAction();
        EndDrawFoldableBox(unfold, indent);
    }

    public static bool BeginDrawFoldableBox(string label, ref bool unfold, Indent indent = Indent.Label, GUIStyle labelStyle = null) =>
        DrawBox(label, ref unfold, indent, labelStyle);

    public static void EndDrawFoldableBox(bool unfold, Indent indent = Indent.Label)
    {
        EndDrawVertical(indent, unfold);
        EndGUILayoutIndent();
    }

    public static void DrawListFolder(string label, ref bool unfold, Action drawAction, Indent indent = Indent.Content)
    {
        if (BeginDrawList(label, ref unfold, indent)) drawAction();
        EndDrawList(unfold);
    }

    public static bool BeginDrawList(string label, ref bool unfold, Indent indent = Indent.Content)
    {
        GUILayout.BeginVertical();
        if (!(unfold = EditorGUILayout.Foldout(unfold, label, true))) return false;
        if (indent.HasFlag(Indent.Content))
            EditorGUI.indentLevel++;
        return true;
    }

    public static void EndDrawList(bool unfold, Indent indent = Indent.Content) => EndDrawVertical(indent, unfold);

    public static void DrawList(string label, SerializedProperty property)
    {
        if (!property.isArray) return;
        if (!(property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label, true))) return;
        EditorGUI.indentLevel++;
        DrawListSize(property);
        for (var i = 0; i < property.arraySize; i++)
            EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i));
        EditorGUI.indentLevel--;
        property.serializedObject.TryApplyModifiedProperties();
    }

    public static void DrawListSize(SerializedProperty property)
    {
        if (property.isArray && property.isExpanded)
            property.arraySize = Mathf.Max(0, EditorGUILayout.DelayedIntField("Size", property.arraySize));
    }

    public static void DrawReorderableList(ReorderableList list, SerializedProperty property, float? height = null,
        ReorderableList.HeaderCallbackDelegate drawHeaderCallback = null,
        ReorderableList.ElementCallbackDelegate drawElementCallback = null)
    {
        list.drawHeaderCallback = drawHeaderCallback ?? (rect =>
        {
            property.arraySize = Mathf.Max(0, EditorGUI.DelayedIntField(rect, "Size", property.arraySize));
        });

        list.drawElementCallback = drawElementCallback ?? ((rect, index, isActive, isFocused) =>
        {
            var element = property.GetArrayElementAtIndex(index);
            rect.y += 2f;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, height ?? EditorGUIUtility.singleLineHeight), element);
        });

        list.DoLayoutList();
        property.serializedObject.TryApplyModifiedProperties();
    }

    public static void DrawReorderableList(ReorderableList list, SerializedProperty property, string label, ref bool unfold,
        float? height = null,
        ReorderableList.HeaderCallbackDelegate drawHeaderCallback = null,
        ReorderableList.ElementCallbackDelegate drawElementCallback = null) =>
        DrawListFolder(label, ref unfold, () => DrawReorderableList(list, property, height, drawHeaderCallback, drawElementCallback));

    public static void DrawHorizontalLine()
    {
        var color = GUI.color;
        GUI.color = Color.grey;
        GUILayout.Box(GUIContent.none, HorizontalLine);
        GUI.color = color;
        GUILayout.Space(2);
    }

    public static Rect DrawHeader(Rect position, string label, bool jumpLine = true)
    {
        if (jumpLine)
            position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(position, label, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
        return position;
    }

    public static void DrawHeader(string label)
    {
        DrawVertical(() =>
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField(label, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
        });
    }

    public static bool DrawToggleButton(bool value, string offIconName, string onIconName, string label, Color offColor, Color onColor,
        params GUILayoutOption[] options) =>
        DrawToggleButton(value, DrawIconLabel(value ? onIconName : offIconName, label), offColor, onColor, options);

    public static bool DrawToggleButton(bool value, string iconName, string label, Color offColor, Color onColor,
        params GUILayoutOption[] options) => DrawToggleButton(value, DrawIconLabel(iconName, label), offColor, onColor, options);

    public static bool DrawToggleButton(bool value, GUIContent content, Color offColor, Color onColor, params GUILayoutOption[] options)
    {
        var color = GUI.color;
        GUI.color = value ? onColor : offColor;
        value = GUILayout.Toggle(value, content, GUI.skin.button, options.Length > 0 ? options : new[] {GUILayout.Height(30)});
        GUI.color = color;
        return value;
    }

    public static bool DrawToggleButton(bool value, string label, Color offColor, Color onColor, params GUILayoutOption[] options)
    {
        var color = GUI.color;
        GUI.color = value ? onColor : offColor;
        value = GUILayout.Toggle(value, label, GUI.skin.button, options.Length > 0 ? options : new[] {GUILayout.Height(30)});
        GUI.color = color;
        return value;
    }

    public static bool DrawToggleButton(bool value, string iconName, string label, params GUILayoutOption[] options) =>
        DrawToggleButton(value, DrawIconLabel(iconName, label), options);

    public static bool DrawToggleButton(bool value, GUIContent content, params GUILayoutOption[] options) =>
        GUILayout.Toggle(value, content, GUI.skin.button, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

    public static bool DrawToggleButton(bool value, string label, params GUILayoutOption[] options) =>
        GUILayout.Toggle(value, label, GUI.skin.button, options.Length > 0 ? options : new[] {GUILayout.Height(30)});

    public static void DrawButton(Action action, GUIStyle style, params GUILayoutOption[] options) =>
        DrawButton(action.Method.Name, action, style, options);

    public static void DrawButton(Action action, bool addSpace = true, params GUILayoutOption[] options) =>
        DrawButton(addSpace ? DrawPrettyName(action.Method.Name) : action.Method.Name, action, options);

    public static void DrawButton(string iconName, string label, Action action, float height) =>
        DrawButton(iconName, label, action, GUILayout.Height(height));

    public static void DrawButton(string iconName, string label, Action action, params GUILayoutOption[] options) =>
        DrawButton(DrawIconLabel(iconName, label), action, options);

    public static void DrawButton(GUIContent content, Action action, params GUILayoutOption[] options)
    {
        if (GUILayout.Button(content, options.Length > 0 ? options : new[] {GUILayout.Height(30)}))
            action();
    }

    public static void DrawButton(string iconName, string label, Action action, GUIStyle style, params GUILayoutOption[] options) =>
        DrawButton(DrawIconLabel(iconName, label), action, style, options);

    public static void DrawButton(GUIContent content, Action action, GUIStyle style, params GUILayoutOption[] options)
    {
        if (GUILayout.Button(content, style, options.Length > 0 ? options : new[] {GUILayout.Height(30)}))
            action();
    }

    public static void DrawButton(string label, Action action, float height) => DrawButton(label, action, GUILayout.Height(height));

    public static void DrawButton(string label, Action action, params GUILayoutOption[] options)
    {
        if (GUILayout.Button(label, options.Length > 0 ? options : new[] {GUILayout.Height(30)}))
            action();
    }

    public static void DrawButton(string label, Action action, GUIStyle style, params GUILayoutOption[] options)
    {
        if (GUILayout.Button(label, style, options.Length > 0 ? options : new[] {GUILayout.Height(30)}))
            action();
    }

    public static GUIContent DrawIconLabel(string iconName, string label) => new GUIContent(DrawIcon(iconName)) {text = $" {label}"};

    public static GUIContent DrawIcon(string iconName) => EditorGUIUtility.IconContent(iconName);

    public static void YesNoDialog(string title, string content, Action yesAction)
    {
        if (YesNoDialog(title, content)) yesAction();
    }

    public static bool YesNoDialog(string title, string content) => EditorUtility.DisplayDialog(title, content, "Yes", "No");

    public static Rect MultiFieldPrefixLabel(Rect position, string label, int columns)
    {
        var controlId = GUIUtility.GetControlID("Foldout".GetHashCode(), FocusType.Keyboard, position);
        position = MultiFieldPrefixLabel(position, controlId, EditorGUIUtility.TrTextContent(label), columns);
        position.height = 16f;
        return position;
    }

    public static Quaternion DrawRotationEuler(string label, Quaternion rotation) =>
        Quaternion.Euler(EditorGUILayout.Vector3Field(label, rotation.eulerAngles));

    public static void DrawComponentFromSelf<T>(MonoBehaviour source, ref T destination, params GUILayoutOption[] options)
        where T : Component => DrawComponentFromSelf($"{typeof(T).Name} From Self", source, ref destination, options);

    public static void DrawComponentFromSelf<T>(string label, MonoBehaviour source, ref T destination, params GUILayoutOption[] options)
        where T : Component
    {
        var dest = destination;
        DrawButton(label, () => dest = source.GetComponent<T>(), options.Length > 0 ? options : new[] {GUILayout.Height(20)});
        destination = dest;
    }

    public static void DrawFlagField(Rect position, SerializedProperty property) =>
        property.intValue = EditorGUI.MaskField(position, property.displayName, property.intValue, property.enumNames);

    public static void DrawFlagField(SerializedProperty property)
    {
        property.intValue = EditorGUILayout.MaskField(property.displayName, property.intValue, property.enumNames);
        property.serializedObject.TryApplyModifiedProperties();
    }

    public static bool PropertyField(this SerializedProperty property) => EditorGUILayout.PropertyField(property);

    public static void TryApplyModifiedProperties(this SerializedObject serializedObject)
    {
        if (serializedObject.hasModifiedProperties) serializedObject.ApplyModifiedProperties();
    }

    public static void DrawGizmosBox(Vector2 min, Vector2 max, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(new Vector2(min.x, min.y), new Vector2(min.x, max.y));
        Gizmos.DrawLine(new Vector2(min.x, max.y), new Vector2(max.x, max.y));
        Gizmos.DrawLine(new Vector2(max.x, max.y), new Vector2(max.x, min.y));
        Gizmos.DrawLine(new Vector2(max.x, min.y), new Vector2(min.x, min.y));
    }

    public static void DrawGizmosBox(Rect rect, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y));
        Gizmos.DrawLine(new Vector2(rect.x + rect.width, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height));
        Gizmos.DrawLine(new Vector2(rect.x + rect.width, rect.y + rect.height), new Vector2(rect.x, rect.y + rect.height));
        Gizmos.DrawLine(new Vector2(rect.x, rect.y + rect.height), new Vector2(rect.x, rect.y));
    }

    public static void DrawGizmosBoxFromCenter(Rect rect, Color color) =>
        DrawGizmosBox(new Rect(rect.x - rect.width / 2, rect.y - rect.height / 2, rect.width, rect.height), color);

    public static void DrawGizmosCross(Vector2 position, Color color, float width = 2f)
    {
        Gizmos.color = color;
        var width2 = width / 2;
        Gizmos.DrawLine(new Vector2(position.x - width2, position.y + width2), new Vector2(position.x + width2, position.y - width2));
        Gizmos.DrawLine(new Vector2(position.x - width2, position.y - width2), new Vector2(position.x + width2, position.y + width2));
    }

    public static void DrawGizmosWireCapsule(Vector3 position, Quaternion rotation, float radius, float height, Color color = default)
    {
        if (color != default)
            Handles.color = color;

        var angleMatrix = Matrix4x4.TRS(position, rotation, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            var pointOffset = (height - (radius * 2)) / 2;

            //draw sideways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
            Handles.DrawLine(new Vector3(0, pointOffset, -radius), new Vector3(0, -pointOffset, -radius));
            Handles.DrawLine(new Vector3(0, pointOffset, radius), new Vector3(0, -pointOffset, radius));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);
            //draw frontways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
            Handles.DrawLine(new Vector3(-radius, pointOffset, 0), new Vector3(-radius, -pointOffset, 0));
            Handles.DrawLine(new Vector3(radius, pointOffset, 0), new Vector3(radius, -pointOffset, 0));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);
            //draw center
            Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, radius);
            Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, radius);
        }
    }

    #endregion /Public

#endif
}