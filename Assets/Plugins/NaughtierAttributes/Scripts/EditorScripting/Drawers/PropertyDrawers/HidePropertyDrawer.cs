#if UNITY_EDITOR
using NaughtierAttributes;
using NaughtierAttributes.Editor;
using UnityEditor;

[PropertyDrawer(typeof(HideAttribute))]
public class HidePropertyDrawer : BasePropertyDrawer 
{
	public override void DrawProperty(SerializedProperty property)
	{
	}
}
#endif