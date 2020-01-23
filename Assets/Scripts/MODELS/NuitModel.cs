using System;
using UnityEngine;
using UnityEngine.Video;

public enum TypeNuit : byte
{
    Reve,
    Songe,
    Cauchemar
}

[CreateAssetMenu(menuName = "Entity/Sommeil", fileName = "Sommeil")]
public class NuitModel : CardModel
{
    // @formatter:off
    #region Serialized
    
    [Header("Données nuit")] 
    public TypeNuit typeNuit;

    [Header("Données graphiques")] 
    public VideoClip startVideoClip;
    public VideoClip idleVideoClip;

    #endregion /Serialized
    // @formatter:on
    
#if UNITY_EDITOR
    protected override Type AssetType => typeof(NuitModel);
#endif
}