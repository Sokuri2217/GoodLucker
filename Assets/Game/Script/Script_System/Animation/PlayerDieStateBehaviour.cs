using UnityEngine;

public class PlayerDieStateBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //アニメーション終了時にオブジェクトを削除
        animator.GetComponent<UIStage>()?.GameOver();
    }
}
