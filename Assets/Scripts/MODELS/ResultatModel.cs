using UnityEngine;
using UnityEngine.Video;

public enum TypeResultat : byte
{
    ReussiteCritique,
    Reussite,
    Echec,
    EchecCritique,
}

[CreateAssetMenu(menuName = "Entity/Resultat", fileName = "Resultat")]
public class ResultatModel : ScriptableObject
{
    public TypeResultat typeResultat;

    public SommeilModel sommeilModel;
    public ActionModel actionModel;

    public VideoClip videoClip;
    public AnimationClip animationClip;

    public bool CheckResultat(SommeilModel sommeil, ActionModel action)
    {
        return (sommeil.QRID == sommeilModel.QRID && action.QRID == actionModel.QRID);
    }
}