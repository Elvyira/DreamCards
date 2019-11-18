using System.Collections.Generic;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using NaughtierAttributes;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class EntitiesDatabase : MonoBehaviour
{
    [SerializeField, FindAssets] private SommeilModel[] _sommeilEntities;
    [SerializeField, FindAssets] private ActionModel[] _actionEntities;
    [SerializeField, FindAssets] private ResultatModel[] _resultatEntities;
    [SerializeField, FindAssets] private NoteCarnetModel[] _notebookEntries;

    private static EntitiesDatabase m_instance;

    public static SommeilModel[] SommeilEntities => m_instance._sommeilEntities;
    public static ActionModel[] ActionEntities => m_instance._actionEntities;
    public static ResultatModel[] ResultatEntities => m_instance._resultatEntities;
    public static NoteCarnetModel[] NotebookEntries => m_instance._notebookEntries;

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
            if (sommeil.QRID == qrid)
                return sommeil;

        return null;
    }

    public static ActionModel GetAction(string qrid)
    {
        foreach (var action in m_instance._actionEntities)
            if (action.QRID == qrid)
                return action;

        return null;
    }

    public static ResultatModel GetResultat(SommeilModel sommeil, ActionModel action)
    {
        foreach (var resultat in m_instance._resultatEntities)
            if (resultat.CheckResultat(sommeil, action))
                return resultat;

        return null;
    }

    public static NoteCarnetModel[] GetNotesCarnetBySommeil(SommeilModel sommeil)
    {
        var entriesList = new List<NoteCarnetModel>();

        foreach (var notebookEntry in m_instance._notebookEntries)
            if (notebookEntry.sommeil.index == sommeil.index)
                entriesList.Add(notebookEntry);

        return entriesList.ToArray();
    }

    public static NoteCarnetModel[] GetUnlockedNotesCarnet()
    {
        var entriesList = new List<NoteCarnetModel>();

        foreach (var notebookEntry in m_instance._notebookEntries)
            if (SavedDataServices.IsNoteDiscovered(notebookEntry.sommeil.index, notebookEntry.typeNote))
                entriesList.Add(notebookEntry);

        return entriesList.ToArray();
    }

    public static NoteCarnetModel[] GetUnlockedNotesCarnet(SommeilModel sommeil)
    {
        var entriesList = new List<NoteCarnetModel>();

        foreach (var notebookEntry in m_instance._notebookEntries)
            if (notebookEntry.sommeil.index == sommeil.index &&
                SavedDataServices.IsNoteDiscovered(notebookEntry.sommeil.index, notebookEntry.typeNote))
                entriesList.Add(notebookEntry);

        return entriesList.ToArray();
    }

    public static void UnlockNoteCarnet(NoteCarnetModel noteCarnet)
    {
        if (SavedDataServices.DiscoverNote(noteCarnet.sommeil.index, noteCarnet.typeNote))
            SavedDataServices.SavePlayer();
    }

    #region Editor

#if UNITY_EDITOR
    private void Update()
    {
        if (EditorApplication.isPlaying) return;
        _sommeilEntities = _sommeilEntities.OrderBy(x => x.index).ToArray();
        _actionEntities = _actionEntities.OrderBy(x => x.index).ToArray();
        m_instance = this;
    }
#endif

    #endregion /Editor
}