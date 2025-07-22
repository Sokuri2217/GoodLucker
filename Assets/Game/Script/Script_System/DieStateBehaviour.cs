using UnityEngine;

public class DieStateBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //アニメーション終了時にオブジェクトを削除
        animator.GetComponent<BossController>()?.DeleteObject();
    }
}
