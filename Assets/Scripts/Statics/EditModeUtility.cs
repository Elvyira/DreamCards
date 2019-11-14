#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

public static class EditModeUtility
{
    public static T[] FindAssetsOfType<T>() =>
        AssetDatabase.FindAssets($"t:{typeof(T)}".Replace("UnityEngine.", "")).Select(AssetDatabase.GUIDToAssetPath)
            .Select((s, i) => AssetDatabase.LoadAssetAtPath(s, typeof(T))).Where(asset => asset != null).Cast<T>().ToArray();
}
#endif