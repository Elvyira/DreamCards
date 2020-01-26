﻿#if UNITY_EDITOR
using System.Linq;
#endif
using System;
using MightyAttributes;
using UnityEngine;
using UnityEngine.Video;

public enum TypeResultat : byte
{
    ReussiteCritique,
    Reussite,
    EchecCritique,
}

[Flags]
public enum TypeNote : byte
{
    ReussiteCritique = 1,
    Reussite1 = 1 << 1,
    Reussite2 = 1 << 2,
    EchecCritique1 = 1 << 3,
    EchecCritique2 = 1 << 4
}

[Serializable]
public class NoteCarnet
{
    [ReadOnly] public string sommeil;
    [ReadOnly] public string objet;

    [ResizableTextArea] public string note;
    [ReadOnly] public TypeNote typeNote;
}

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Entity/Resultat", fileName = "Resultat")]
#endif
public class ResultatModel : ScriptableObject
{
    // @formatter:off
    #region Serialized
    
    [Header("Données résultat")]
    public TypeResultat typeResultat;

    [Header("Entités liées")]
    [AssetOnly] public SommeilModel sommeil;
    [AssetOnly] public ObjetModel objet;

    [Header("Données graphiques")]
    [AssetOnly] public VideoClip videoClip;
    [AssetOnly] public AnimationClip animationClip;

    [Header("Note Carnet")] 
    [DrawSerializable(SerializableOption.ContentOnly), Line(DecoratorPosition.Wrap)] public NoteCarnet noteCarnet;

    #endregion /Serialized
    // @formatter:on

    public bool CheckResultat(SommeilModel sommeil, ObjetModel objet)
    {
        return (sommeil.index == this.sommeil.index && objet.index == this.objet.index);
    }

    public void UnlockNoteCarnet()
    {
        if (SavedDataServices.DiscoverNote(sommeil.index, noteCarnet.typeNote))
            SavedDataServices.SavePlayer();
    }

#if UNITY_EDITOR

    #region Editor

    [OnInspectorGUI]
    private void OnValidate()
    {
        if (objet != null) noteCarnet.objet = objet.nom;

        if (sommeil == null) return;
        noteCarnet.sommeil = sommeil.nom;

        switch (typeResultat)
        {
            case TypeResultat.ReussiteCritique:
                noteCarnet.typeNote = TypeNote.ReussiteCritique;
                break;
            case TypeResultat.Reussite:
                noteCarnet.typeNote = IsResultatTypeDuplicate(TypeNote.Reussite1)
                    ? TypeNote.Reussite2
                    : TypeNote.Reussite1;
                break;
            case TypeResultat.EchecCritique:
                noteCarnet.typeNote = IsResultatTypeDuplicate(TypeNote.EchecCritique1)
                    ? TypeNote.EchecCritique2
                    : TypeNote.EchecCritique1;
                break;
        }
    }

    private bool IsResultatTypeDuplicate(TypeNote typeNote) =>
        sommeil != null && EditModeUtility.FindAssetsOfType<ResultatModel>()
            .Where(x => x.sommeil == sommeil && x.GetInstanceID() != GetInstanceID())
            .Any(x => x.noteCarnet.typeNote == typeNote);

    #endregion /Editor

#endif
}