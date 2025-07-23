using UnityEngine;

public class EnemyStateBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<EnemyBase>()?.ActiveAttack();
        animator.GetComponent<EnemyBase>().isAttack = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<EnemyBase>()?.InactiveAttack();
        animator.GetComponent<EnemyBase>().isAttack = false;
    }
}
