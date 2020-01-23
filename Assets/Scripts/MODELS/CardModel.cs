using System;
using MightyAttributes;
using UnityEngine;

public abstract class CardModel : ScriptableObject
{
    [Header("Données carte"), ValidateInput("IndexValid", "index is invalid")]
    public byte index;

    public string nom;
    public string QRID;

#if UNITY_EDITOR
    protected abstract Type AssetType { get; }
    
    private bool IndexValid(byte index)
    {
        foreach (var asset in EditModeUtility.FindAssetsOfType(AssetType))
        {
            if (!(asset is CardModel model)) return false;
            
            if (model.QRID == QRID) continue;
            if (model.index == index) return false;
        }
        return true;
    }
#endif
}