using System.Collections.Generic;
#if UNITY_EDITOR
using System.Linq;
#endif
using NaughtierAttributes;
using UnityEngine;

public class EntitiesDatabase : MonoBehaviour
{
    [SerializeField, FindAssets] private SommeilModel[] _sommeilEntities;
    [SerializeField, FindAssets] private ActionModel[] _actionEntities;
    [SerializeField, FindAssets] private ResultatModel[] _resultatEntities;
    [SerializeField, FindAssets] private NoteCarnetModel[] _notebookEntries;

    private static EntitiesDatabase m_instance;

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

    public static NoteCarnetModel[] GetUnlockedNotebookEntries()
    {
        var entriesList = new List<NoteCarnetModel>();
        foreach (var notebookEntry in m_instance._notebookEntries)
        {
            if (SavedDataServices.IsNoteDiscovered(notebookEntry.sommeil.index, notebookEntry.typeNote))
                entriesList.Add(notebookEntry);
        }

        return entriesList.ToArray();
    }

    public static void UnlockNotebookEntry(NoteCarnetModel noteCarnet)
    {
        if (SavedDataServices.DiscoverNote(noteCarnet.sommeil.index, noteCarnet.typeNote))
            SavedDataServices.SavePlayer();
    }


    #region Editor

#if UNITY_EDITOR
    private void OnValidate()
    {
        _sommeilEntities = _sommeilEntities.OrderBy(x => x.index).ToArray();
        _actionEntities = _actionEntities.OrderBy(x => x.index).ToArray();
    }
#endif

    #endregion /Editor
}