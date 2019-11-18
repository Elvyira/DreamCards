﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public static class EditModeUtility
{
    public static GameObject[] GetRootGameObjects()
    {
        var activeScene = SceneManager.GetActiveScene();
        return activeScene.isLoaded ? activeScene.GetRootGameObjects() : new GameObject[0];
    }
    
    public static T[] FindAssetsOfType<T>() =>
        AssetDatabase.FindAssets($"t:{typeof(T)}".Replace("UnityEngine.", "")).Select(AssetDatabase.GUIDToAssetPath)
            .Select((s, i) => AssetDatabase.LoadAssetAtPath(s, typeof(T))).Where(asset => asset != null).Cast<T>().ToArray();

    public static T FindFirstObject<T>(bool includeInactive = true) where T : Component
    {
        T t = default;
        return GetRootGameObjects().Any(o => (t = o.GetComponentsInChildren<T>(includeInactive).FirstOrDefault()) != null)
            ? t
            : null;
    }

    public static T[] FindAllObjects<T>(bool includeInactive = true) where T : Component
    {
        var list = new List<T>();
        foreach (var o in GetRootGameObjects()) list.AddRange(o.GetComponentsInChildren<T>(includeInactive));
        return list.ToArray();
    }
}
#endif