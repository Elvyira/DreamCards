using System;
using UnityEngine;
using UnityEngine.Video;

public enum TypeSommeil : byte
{
    Reve,
    Songe,
    Cauchemar
}

[CreateAssetMenu(menuName = "Entity/Sommeil", fileName = "Sommeil")]
public class SommeilModel : CardModel
{
    // @formatter:off
    #region Serialized
    
    [Header("Données sommeil")] 
    public TypeSommeil typeSommeil;

    [Header("Données graphiques")] 
    public VideoClip startVideoClip;
    public VideoClip idleVideoClip;

    #endregion /Serialized
    // @formatter:on
    
#if UNITY_EDITOR
    protected override Type AssetType => typeof(SommeilModel);
#endif
}