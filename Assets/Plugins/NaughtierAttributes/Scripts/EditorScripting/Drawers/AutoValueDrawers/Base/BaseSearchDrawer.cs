#if UNITY_EDITOR
using NaughtierAttributes.Editor;
using UnityEditor;
using UnityEngine;

public abstract class BaseSearchDrawer : BaseAutoValueDrawer {

	protected override InitState InitPropertyImpl(ref SerializedProperty property)
	{
		if (property.isArray)
			return new InitState(false, "\"" + property.displayName + "\" should not be an array");
		
		if (!typeof(Object).IsAssignableFrom(property.GetPropertyType()))
			return new InitState(false, "\"" + property.displayName + "\" should inherit from UnityEngine.Object");
		
		Find(ref property);
		return new InitState(true);
	}

	protected abstract void Find(ref SerializedProperty property);
}
#endif