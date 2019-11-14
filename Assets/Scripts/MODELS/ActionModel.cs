using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Action", fileName = "Action")]
public class ActionModel : CardModel
{
    [Header("Données graphiques")] 
    public AnimationClip animationClip;
}