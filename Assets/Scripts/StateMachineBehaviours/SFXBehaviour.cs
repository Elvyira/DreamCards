using UnityEngine;

public class SFXBehaviour : StateMachineBehaviour
{
    public SFXSource source;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => 
        InstanceManager.AudioManager.PlaySource(source);
}
