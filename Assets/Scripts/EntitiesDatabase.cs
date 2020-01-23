using System.Collections.Generic;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using MightyAttributes;
using UnityEngine;

public class EntitiesDatabase : MonoBehaviour
{
    [SerializeField, ReadOnly, FindAssets] private NuitModel[] _sommeilEntities;
    [SerializeField, ReadOnly, FindAssets] private ObjetModel[] _objetEntities;
    [SerializeField, ReadOnly, FindAssets] private ResultatModel[] _resultatEntities;

    private static EntitiesDatabase m_instance;

    private static EntitiesDatabase Instance => m_instance ? m_instance : m_instance = EditModeUtility.FindFirstObject<EntitiesDatabase>();

    public static NuitModel[] SommeilEntities => Instance._sommeilEntities;
    public static ObjetModel[] ObjetEntities => Instance._objetEntities;
    public static ResultatModel[] ResultatEntities => Instance._resultatEntities;

    private void Awake()
    {
        m_instance = this;
    }

    public static CardModel GetCard(string qrid)
    {
        var card = GetSommeil(qrid);
        if (card == null) return GetObjet(qrid);
        return card;
    }

    public static NuitModel GetSommeil(string qrid)
    {
        foreach (var sommeil in m_instance._sommeilEntities)
            if (sommeil.QRID == qrid)
                return sommeil;

        return null;
    }

    public static ObjetModel GetObjet(string qrid)
    {
        foreach (var action in m_instance._objetEntities)
            if (action.QRID == qrid)
                return action;

        return null;
    }

    public static ResultatModel GetResultat(NuitModel nuit, ObjetModel objet)
    {
        foreach (var resultat in m_instance._resultatEntities)
            if (resultat.CheckResultat(nuit, objet))
                return resultat;

        return null;
    }

    public static NoteCarnet[] GetNotesCarnetBySommeil(NuitModel nuit)
    {
        var entriesList = new List<NoteCarnet>();

        foreach (var resultat in m_instance._resultatEntities)
            if (resultat.nuit.index == nuit.index)
                entriesList.Add(resultat.noteCarnet);

        return entriesList.ToArray();
    }

    public static NoteCarnet[] GetUnlockedNotesCarnet()
    {
        var entriesList = new List<NoteCarnet>();

        foreach (var resultat in m_instance._resultatEntities)
            if (SavedDataServices.IsNoteDiscovered(resultat.nuit.index, resultat.noteCarnet.typeNote))
                entriesList.Add(resultat.noteCarnet);

        return entriesList.ToArray();
    }

    public static NoteCarnet[] GetUnlockedNotesCarnet(NuitModel nuit)
    {
        var entriesList = new List<NoteCarnet>();

        foreach (var resultat in m_instance._resultatEntities)
            if (resultat.nuit.index == nuit.index &&
                SavedDataServices.IsNoteDiscovered(resultat.nuit.index, resultat.noteCarnet.typeNote))
                entriesList.Add(resultat.noteCarnet);

        return entriesList.ToArray();
    }

#if UNITY_EDITOR
    
    #region Editor

    [Button]
    private void RefreshDatabase()
    {
        _sommeilEntities = new NuitModel[0];
        _objetEntities = new ObjetModel[0];
        _resultatEntities = new ResultatModel[0];
    }
    
    [OnInspectorGUI]
    private void OrderEntities()
    {
        if (EditorApplication.isPlaying) return;

        _sommeilEntities = _sommeilEntities.OrderBy(x => x.index).ToArray();
        _objetEntities = _objetEntities.OrderBy(x => x.index).ToArray();
        m_instance = this;
    }

    #endregion /Editor

#endif
}