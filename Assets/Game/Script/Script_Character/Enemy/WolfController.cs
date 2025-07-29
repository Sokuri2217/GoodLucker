using UnityEngine;

public class WolfController : BossController
{
    [Header("移動")]
    public bool isChase; //追跡中
    [Header("攻撃")]
    public float normalAttackDistance;                  //通常攻撃出来る距離

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!uiStage.gameClear && !uiStage.gameOver)
        {
            //移動
            UniqueEnemyMove();
        }
        //徘徊中でないときは追跡
        if (isWalk)
        {
            isChase = false;
        }
        else
        {
            isChase = true;
        }
    }

    //移動処理
    public void UniqueEnemyMove()
    {
        //追跡
        if ((isChase && !isWalk) || invincible)
        {
            if (!isAttack)
            {
                //プレイヤーの方に向く(水平回転のみ)
                Vector3 chaseForward = (playerPos.position - transform.position).normalized;
                chaseForward.y = 0;
                transform.forward = chaseForward;
            }
            //プレイヤーを追いかける
            agent.SetDestination(playerPos.position);
            //プレイヤーとの距離
            float playerDistance = Vector3.Distance(transform.position, playerPos.position);
            //一定距離近づくと攻撃する
            if (playerDistance <= normalAttackDistance && !isAttack && readyAttack) 
            {
                readyAttack = false;
                ActiveNormalAttack();
            }
        }
        //攻撃中は移動しない
        if (isAttack)
        {
            agent.isStopped = true;
            agent.speed = 0;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    //通常攻撃
    public void ActiveNormalAttack()
    {
        critical = ActiveCritical();
        weapon.currentAttack = status[(int)StatusName.STR];
        animator.SetTrigger("NormalAttack");
    }
}