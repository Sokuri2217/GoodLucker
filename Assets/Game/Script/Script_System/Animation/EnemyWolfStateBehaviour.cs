using UnityEngine;

public class EnemyWolfStateBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<WolfController>()?.ActiveJumpAttackCollider();
        animator.GetComponent<WolfController>().isJumpAttack = true;
        animator.GetComponent<WolfController>().isWalk = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<WolfController>()?.InactiveJumpAttackCollider();
        animator.GetComponent<WolfController>().isJumpAttack = false;
        animator.GetComponent<WolfController>().isWalk = true;
    }
}
