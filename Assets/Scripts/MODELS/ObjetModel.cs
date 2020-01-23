using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Objet", fileName = "Objet")]
public class ObjetModel : CardModel
{
    // @formatter:off
    #region Serialized
    
    [Header("Données graphiques")] 
    public AnimationClip animationClip;
    
    #endregion /Serialized
    // @formatter:on

#if UNITY_EDITOR
    protected override Type AssetType => typeof(ObjetModel);
#endif
}