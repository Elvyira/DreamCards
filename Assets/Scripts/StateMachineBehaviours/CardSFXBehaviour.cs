using UnityEngine;

public class CardSFXBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => 
        InstanceManager.TurnManager.PlayCardSFX();
}
