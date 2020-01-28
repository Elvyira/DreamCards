using System.Collections.Generic;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using MightyAttributes;
using UnityEngine;

public class EntitiesDatabase : MonoBehaviour
{
    #region Serialized

    [SerializeField, Entities] private SommeilModel[] _sommeilEntities;
    [SerializeField, Entities] private ObjetModel[] _objetEntities;
    [SerializeField, Entities] private ResultatModel[] _resultatEntities;

    #endregion /Serialized

    #region Instance

    private static EntitiesDatabase m_instance;

#if UNITY_EDITOR
    public static EntitiesDatabase Instance => m_instance ? m_instance : m_instance = EditModeUtility.FindFirstObject<EntitiesDatabase>();
#else
    public static EntitiesDatabase Instance => m_instance;
#endif

    #endregion /Instance

    public static SommeilModel[] SommeilEntities => Instance._sommeilEntities;
    public static ObjetModel[] ObjetEntities => Instance._objetEntities;
    public static ResultatModel[] ResultatEntities => Instance._resultatEntities;

    public void Init() => m_instance = this;

    public static CardModel GetCard(string qrLink)
    {
        var card = GetSommeil(qrLink);
        if (card == null) return GetObjet(qrLink);
        return card;
    }

    public static SommeilModel GetSommeil(string qrLink)
    {
        foreach (var sommeil in m_instance._sommeilEntities)
            if (sommeil.qrLink == qrLink)
                return sommeil;

        return null;
    }

    public static ObjetModel GetObjet(string qrLink)
    {
        foreach (var objet in m_instance._objetEntities)
            if (objet.qrLink == qrLink)
                return objet;

        return null;
    }

    public static ResultatModel GetResultat(SommeilModel sommeil, ObjetModel objet)
    {
        foreach (var resultat in m_instance._resultatEntities)
            if (resultat.CheckResultat(sommeil, objet))
                return resultat;

        return null;
    }

    public static NoteCarnet[] GetNotesCarnetBySommeil(SommeilModel sommeil)
    {
        var entriesList = new List<NoteCarnet>();

        foreach (var resultat in m_instance._resultatEntities)
            if (resultat.sommeil.index == sommeil.index)
                entriesList.Add(resultat.noteCarnet);

        return entriesList.ToArray();
    }

    public static NoteCarnet[] GetUnlockedNotesCarnet()
    {
        var entriesList = new List<NoteCarnet>();

        foreach (var resultat in m_instance._resultatEntities)
            if (SavedDataServices.IsNoteDiscovered(resultat.sommeil.index, resultat.noteCarnet.typeNote))
                entriesList.Add(resultat.noteCarnet);

        return entriesList.ToArray();
    }

    public static NoteCarnet[] GetUnlockedNotesCarnet(SommeilModel sommeil)
    {
        var entriesList = new List<NoteCarnet>();

        foreach (var resultat in m_instance._resultatEntities)
            if (resultat.sommeil.index == sommeil.index &&
                SavedDataServices.IsNoteDiscovered(resultat.sommeil.index, resultat.noteCarnet.typeNote))
                entriesList.Add(resultat.noteCarnet);

        return entriesList.ToArray();
    }

#if UNITY_EDITOR

    #region Editor

    [Button]
    private void RefreshDatabase()
    {
        _sommeilEntities = new SommeilModel[0];
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