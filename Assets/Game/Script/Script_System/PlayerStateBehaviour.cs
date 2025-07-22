using UnityEngine;

public class PlayerStateBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerController>()?.ActiveAttack();
        animator.GetComponent<PlayerController>().basicAttack = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerController>()?.InactiveAttack();
        animator.GetComponent<PlayerController>().basicAttack = false;
    }
}
