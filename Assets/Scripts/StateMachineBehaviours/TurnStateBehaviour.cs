using UnityEngine;

public class TurnStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private TurnState _state;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => 
        InstanceManager.TurnManager.SelectState(_state);
}
