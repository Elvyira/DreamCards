using UnityEngine;

public abstract class CardModel : ScriptableObject
{
    [Header("Données carte")] public byte index;
    public string nom;
    public string QRID;
}
