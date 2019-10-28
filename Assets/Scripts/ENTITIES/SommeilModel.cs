using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public enum TypeSommeil : byte 
{
    Reve,
    Songe,
    Cauchemar
}

[CreateAssetMenu (menuName = "Entity/Sommeil", fileName = "Sommeil")]
public class SommeilModel : ScriptableObject 
{
    public string QRID;
    public TypeSommeil typeSommeil;
    public VideoClip startVideoClip, idleVideoClip;

    public ActionModel[] actionReussiteCritique, actionReussite, actionEchecCritique;

    public TypeResultat CheckAction (ActionModel action) 
    {
        if (CheckActionID(action.QRID, actionReussiteCritique))
        {
            // do reussite critique stuff

            return TypeResultat.ReussiteCritique;
        }

        if (CheckActionID(action.QRID, actionReussite))
        {
            // do reussite stuff

            return TypeResultat.Reussite;
        }

        if (CheckActionID(action.QRID, actionEchecCritique))
        {
            // do echec critique stuff

            return TypeResultat.EchecCritique;
        }
        return TypeResultat.Echec;
    }

    private bool CheckActionID(string qrid, ActionModel[] actionList)
    {
        foreach (var action in actionList)
        {
            if (action.QRID == qrid)
                return true;
        }
        return false;
    }
}