using NaughtierAttributes;
using UnityEditor;
#if UNITY_EDITOR
using System.Linq;
using NaughtierAttributes.Editor;
#endif
using UnityEngine;
using UnityEngine.Video;

public enum TypeResultat : byte
{
    ReussiteCritique,
    Reussite,
    Echec,
    EchecCritique,
}

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Entity/Resultat", fileName = "Resultat")]
#endif
public class ResultatModel : ScriptableObject
{
    [Header("Données résultat")] public TypeResultat typeResultat;

    [Header("Entités liées")] public SommeilModel sommeil;
    public ActionModel action;
    [ShowIf("IsNotEchec")] public NoteCarnetModel noteCarnet;

    [Header("Données graphiques")] public VideoClip videoClip;
    public AnimationClip animationClip;

    public bool CheckResultat(SommeilModel sommeil, ActionModel action)
    {
        return (sommeil.index == this.sommeil.index && action.index == this.action.index);
    }

    #region Editor

#if UNITY_EDITOR
    private bool IsNotEchec() => typeResultat != TypeResultat.Echec;

    private void OnValidate()
    {
        if (noteCarnet == null || sommeil == null) return;
        noteCarnet.sommeil = sommeil;
        switch (typeResultat)
        {
            case TypeResultat.ReussiteCritique:
                noteCarnet.typeNote = TypeNote.ReussiteCritique;
                break;
            case TypeResultat.Reussite:
                noteCarnet.typeNote = IsThereNotesWithSameSommeilAndType(TypeNote.Reussite1) ? TypeNote.Reussite2 : TypeNote.Reussite1;
                break;
            case TypeResultat.EchecCritique:
                noteCarnet.typeNote =
                    IsThereNotesWithSameSommeilAndType(TypeNote.EchecCritique1) ? TypeNote.EchecCritique2 : TypeNote.EchecCritique1;
                break;
        }
    }

    private bool IsThereNotesWithSameSommeilAndType(TypeNote typeNote) =>
        sommeil != null && EditModeUtility.FindAssetsOfType<NoteCarnetModel>()
            .Where(x => x.sommeil == sommeil && x.GetInstanceID() != noteCarnet.GetInstanceID())
            .Any(x => x.typeNote == typeNote);

    [CustomEditor(typeof(ResultatModel))]
    private class ResultatModelDrawer : NaughtierEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var resultat = (ResultatModel) target;
            resultat.OnValidate();
        }
    }
    
#endif

    #endregion /Editor
}