using System.Collections.Generic;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using MightyAttributes;
using UnityEngine;

public class EntitiesManager : MonoBehaviour
{
    #region Serialized

    [SerializeField, Entities] private SommeilModel[] _sommeilEntities;
    [SerializeField, Entities] private ObjetModel[] _objetEntities;
    [SerializeField, Entities] private ResultatModel[] _resultatEntities;

    #endregion /Serialized

    public SommeilModel[] SommeilEntities => _sommeilEntities;
    public ObjetModel[] ObjetEntities => _objetEntities;
    public ResultatModel[] ResultatEntities => _resultatEntities;

    public CardModel GetCard(string qrLink)
    {
        var card = GetSommeil(qrLink);
        if (card == null) return GetObjet(qrLink);
        return card;
    }

    public SommeilModel GetSommeil(string qrLink)
    {
        foreach (var sommeil in _sommeilEntities)
            if (sommeil.qrLink == qrLink)
                return sommeil;

        return null;
    }

    public ObjetModel GetObjet(string qrLink)
    {
        foreach (var objet in _objetEntities)
            if (objet.qrLink == qrLink)
                return objet;

        return null;
    }

    public ResultatModel GetResultat(SommeilModel sommeil, ObjetModel objet)
    {
        foreach (var resultat in _resultatEntities)
            if (resultat.CheckResultat(sommeil, objet))
                return resultat;

        return null;
    }

    public NoteCarnet[] GetNotesCarnetBySommeil(SommeilModel sommeil)
    {
        var entriesList = new List<NoteCarnet>();

        foreach (var resultat in _resultatEntities)
            if (resultat.sommeil.index == sommeil.index)
                entriesList.Add(resultat.noteCarnet);

        return entriesList.ToArray();
    }

    public NoteCarnet[] GetUnlockedNotesCarnet()
    {
        var entriesList = new List<NoteCarnet>();

        foreach (var resultat in _resultatEntities)
            if (SavedDataServices.IsNoteDiscovered(resultat.sommeil.index, resultat.noteCarnet.typeNote))
                entriesList.Add(resultat.noteCarnet);

        return entriesList.ToArray();
    }

    public NoteCarnet[] GetUnlockedNotesCarnet(SommeilModel sommeil)
    {
        var entriesList = new List<NoteCarnet>();

        foreach (var resultat in _resultatEntities)
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
    }

    #endregion /Editor

#endif
}