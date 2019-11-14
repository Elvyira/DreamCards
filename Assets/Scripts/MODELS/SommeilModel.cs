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
    [Header("Données sommeil")]
    public TypeSommeil typeSommeil;

    [Header("Données graphiques")] public VideoClip startVideoClip;
    public VideoClip idleVideoClip;
}