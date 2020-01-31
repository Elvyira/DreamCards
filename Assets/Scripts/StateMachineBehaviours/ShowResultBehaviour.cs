using UnityEngine;

public class ShowResultBehaviour : StateMachineBehaviour
{
    [SerializeField] private bool _show;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => 
        InstanceManager.TurnManager.ShowResult(_show);
}
