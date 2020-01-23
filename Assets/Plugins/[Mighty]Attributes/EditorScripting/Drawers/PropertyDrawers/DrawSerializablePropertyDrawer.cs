#if UNITY_EDITOR
using UnityEngine;
using System;
using MightyAttributes;
using MightyAttributes.Editor;
using UnityEditor;

[DrawerTarget(typeof(DrawSerializableAttribute))]
public class DrawSerializablePropertyDrawer : BasePropertyDrawer, IArrayElementDrawer, IRefreshDrawer
{
    private readonly MightyCache<(object[], MightyDrawer[], MightyInfo<SerializableOption>)> m_serializableCache =
        new MightyCache<(object[], MightyDrawer[], MightyInfo<SerializableOption>)>();

    public MightyDrawer EnableProperty(SerializedProperty property, object classReference)
    {
        if (classReference.GetType().GetCustomAttributes(typeof(SerializableAttribute), true).Length == 0)
        {
            EditorDrawUtility.DrawPropertyField(property);
            EditorDrawUtility.DrawHelpBox($"\"{property.displayName}\" type should be a Serializable class");
            return null;
        }
        
        var drawer = new MightyDrawer();
        drawer.OnEnableSerializableClass(property.GetTargetObject(), classReference, property);
        return drawer;
    }
    
    public void DrawProperty(SerializedProperty property, DrawSerializableAttribute attribute, MightyDrawer drawer)
    {
        if (property.isArray)
        {
            EditorDrawUtility.DrawPropertyField(property);
            EditorDrawUtility.DrawHelpBox($"\"{property.displayName}\" can't be an array");
            return;
        }

        DrawSerializable(property, attribute, drawer);
    }

    private void DrawSerializable(SerializedProperty property, DrawSerializableAttribute attribute, MightyDrawer drawer)
    {
        var context = property.GetTargetObject();
        
        var option = attribute.SerializableOption;
        attribute.Option = (FieldOption) option;

        if (option == SerializableOption.ContentOnly)
            drawer.OnGUI(context, property.serializedObject);
        else
        {
            if (!option.Contains(SerializableOption.DontFold))
            {
                if (!EditorDrawUtility.DrawFoldout(property,
                    option.Contains(SerializableOption.BoldLabel) ? GUIStyleUtility.BoldFoldout : EditorStyles.foldout))
                    return;
            }
            else
                DrawLabel(property, attribute, EditorGUIUtility.TrTextContent(property.displayName));

            if (!option.Contains(SerializableOption.DontIndent)) EditorGUI.indentLevel++;
            drawer.OnGUI(context, property.serializedObject);
            if (!option.Contains(SerializableOption.DontIndent)) EditorGUI.indentLevel--;
        }
    }

    public override void DrawProperty(BaseMightyMember mightyMember, SerializedProperty property, BaseDrawerAttribute baseAttribute)
    {
        if (property.isArray)
        {
            EditorDrawUtility.DrawArray(property, index => DrawElement(mightyMember, index, baseAttribute));
            return;
        }

        DrawSerializable(mightyMember, property, 0, (DrawSerializableAttribute) baseAttribute);
    }

    public void DrawElement(BaseMightyMember mightyMember, int index, BaseDrawerAttribute baseAttribute) =>
        DrawSerializable(mightyMember, mightyMember.GetElement(index), index, (DrawSerializableAttribute) baseAttribute);

    public void DrawElement(GUIContent label, BaseMightyMember mightyMember, int index, BaseDrawerAttribute baseAttribute) =>
        DrawSerializable(mightyMember, mightyMember.GetElement(index), index, (DrawSerializableAttribute) baseAttribute,
            label);

    public void DrawElement(Rect rect, BaseMightyMember mightyMember, int index, BaseDrawerAttribute baseAttribute) =>
        DrawSerializable(mightyMember, mightyMember.GetElement(index), index, (DrawSerializableAttribute) baseAttribute);

    public float GetElementHeight(BaseMightyMember mightyMember, int index, BaseDrawerAttribute baseAttribute) => 0;

    private void DrawSerializable(BaseMightyMember mightyMember, SerializedProperty property, int index,
        DrawSerializableAttribute attribute, GUIContent label = null)
    {
        var context = mightyMember.Context;
        if (mightyMember.PropertyType.GetCustomAttributes(typeof(SerializableAttribute), true).Length == 0)
        {
            EditorDrawUtility.DrawPropertyField(property, label);
            EditorDrawUtility.DrawHelpBox($"\"{property.displayName}\" type should be a Serializable class");
            return;
        }

        if (!m_serializableCache.Contains(mightyMember)) InitDrawer(mightyMember, attribute);
        var (_, drawers, optionInfo) = m_serializableCache[mightyMember];
        var option = optionInfo.Value;

        if (index >= drawers.Length) InitDrawer(mightyMember, attribute);
        if (index >= drawers.Length) return;

        attribute.Option = (FieldOption) option;

        if (option == SerializableOption.ContentOnly)
            drawers[index].OnGUI(context, property.serializedObject);
        else
        {
            if (!option.Contains(SerializableOption.DontFold))
            {
                if (!EditorDrawUtility.DrawFoldout(property, label ?? EditorGUIUtility.TrTextContent(property.displayName),
                    option.Contains(SerializableOption.BoldLabel) ? GUIStyleUtility.BoldFoldout : EditorStyles.foldout))
                    return;
            }
            else
                DrawLabel(property, attribute, label);

            if (!option.Contains(SerializableOption.DontIndent)) EditorGUI.indentLevel++;
            drawers[index].OnGUI(context, property.serializedObject);
            if (!option.Contains(SerializableOption.DontIndent)) EditorGUI.indentLevel--;
        }
    }

    public void ApplyAutoValues(BaseMightyMember mightyMember, DrawSerializableAttribute attribute, bool refreshDrawers)
    {
        if (!m_serializableCache.Contains(mightyMember)) InitDrawer(mightyMember, attribute);
        foreach (var drawer in m_serializableCache[mightyMember].Item2) drawer.ApplyAutoValues(refreshDrawers);
    }

    public override void InitDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
    {
        var property = mightyMember.Property;
        var target = mightyMember.InitAttributeTarget<DrawSerializableAttribute>();
        var context = mightyMember.Context;

        var isArray = property.isArray;
        var size = isArray ? property.arraySize : 1;
        var classReferences = new object[size];
        var drawers = new MightyDrawer[size];
        if (isArray)
            for (var i = 0; i < size; i++)
            {
                classReferences[i] = property.GetArrayElementAtIndex(i).GetSerializableClassReference();
                (drawers[i] = new MightyDrawer()).OnEnableSerializableClass(context, classReferences[i],
                    property.GetArrayElementAtIndex(i));
            }
        else
        {
            classReferences[0] = property.GetSerializableClassReference();
            (drawers[0] = new MightyDrawer()).OnEnableSerializableClass(context, classReferences[0], property);
        }

        var attribute = (DrawSerializableAttribute) mightyAttribute;

        if (!property.GetInfoFromMember<SerializableOption>(target, attribute.OptionCallback, out var optionInfo))
            optionInfo = new MightyInfo<SerializableOption>(attribute.SerializableOption);

        m_serializableCache[mightyMember] = (classReferences, drawers, optionInfo);
    }

    public override void ClearCache()
    {
        foreach (var (_, drawers, _) in m_serializableCache.Values)
        foreach (var drawer in drawers)
            drawer.OnDisable();

        m_serializableCache.ClearCache();
    }

    public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
    {
        if (!m_serializableCache.Contains(mightyMember))
        {
            InitDrawer(mightyMember, mightyAttribute);
            return;
        }

        m_serializableCache[mightyMember].Item3.RefreshValue();
    }
}
#endif