using UnityEngine;
using UnityEngine.AI;

public class ZakoController : EnemyBase
{
    [Header("追跡")]
    public bool isChase;               //追跡中かどうか
    [Header("攻撃")]
    public float attackDistance; //攻撃可能になる距離
    [Header("ボス")]
    public string bossName;
    [Header("スクリプト参照")]
    public BossController boss; //ステージボス

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //スクリプト取得
        boss = GameObject.Find(bossName).GetComponent<BossController>();
        //初期設定
        moveTimer = moveLimit; //徘徊先の設定
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
        //死亡処理
        if (currentHp <= 0)
        {
            //体力が0以下になったらオブジェクトを削除
            Destroy(gameObject);
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
        if ((isChase && !isWalk) || boss.invincible || invincible)   
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
            if (playerDistance <= attackDistance && !isAttack && readyAttack)
            {
                readyAttack = false;
                critical = ActiveCritical();
                weapon.currentAttack = status[(int)StatusName.STR];
                animator.SetTrigger("Attack");
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
}
