using UnityEngine;

public class BackToHomeBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => 
        InstanceManager.GUIManager.FadeToHome();
}
