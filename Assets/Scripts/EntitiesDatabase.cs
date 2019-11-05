using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class EntitiesDatabase : MonoBehaviour
{
    [SerializeField] private SommeilModel[] _sommeilEntities;
    [SerializeField] private ActionModel[] _actionEntities;
    [SerializeField] private ResultatModel[] _resultatEntities;

    private static EntitiesDatabase m_instance;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _sommeilEntities = FindAssetsOfType<SommeilModel>();
        _actionEntities = FindAssetsOfType<ActionModel>();
        _resultatEntities = FindAssetsOfType<ResultatModel>();
    }

    private static T[] FindAssetsOfType<T>() =>
        AssetDatabase.FindAssets($"t:{typeof(T)}".Replace("UnityEngine.", "")).Select(AssetDatabase.GUIDToAssetPath)
            .Select((s, i) => AssetDatabase.LoadAssetAtPath(s, typeof(T))).Where(asset => asset != null).Cast<T>().ToArray();
#endif

    private void Awake()
    {
        m_instance = this;
    }

    public static CardModel GetCard(string qrid)
    {
        var card = GetSommeil(qrid);
        if (card == null) return GetAction(qrid);
        return card;
    }

    public static SommeilModel GetSommeil(string qrid)
    {
        foreach (var sommeil in m_instance._sommeilEntities)
        {
            if (sommeil.QRID == qrid) return sommeil;
        }
        return null;
    }

    public static ActionModel GetAction(string qrid)
    {
        foreach (var action in m_instance._actionEntities)
        {
            if (action.QRID == qrid) return action;
        }
        return null;
    }

    public static ResultatModel GetResultat(SommeilModel sommeil, ActionModel action)
    {
        foreach (var resultat in m_instance._resultatEntities)
        {
            if (resultat.CheckResultat(sommeil, action))
                return resultat;
        }
        return null;
    }
}