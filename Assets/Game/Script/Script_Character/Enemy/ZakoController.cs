using UnityEngine;
using UnityEngine.AI;

public class ZakoController : EnemyBase
{
    [Header("移動")]
    public float dashSpeed;            //ダッシュ
    public float viewAngle;            //視野角
    public float chaseRange;           //プレイヤーを検知する距離
    public float moveRadius;           //徘徊する範囲(移動先の範囲)
    public float moveLimit;            //移動先の変更時間
    public float moveTimer;            //計測用
    public bool isChase;               //追跡中かどうか
    public LayerMask otherLayerMasks;  //レイヤー指定(その他)
    public float animaSetNum;          //アニメーション制御
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
            Move3D();
        }
        //死亡処理
        if (currentHp <= 0)
        {
            //体力が0以下になったらオブジェクトを削除
            Destroy(gameObject);
        }
    }

    //移動処理
    public void Move3D()
    {
        //追跡
        if (isChase || boss.invincible || invincible)  
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
            if (playerDistance <= attackDistance && !isAttack)
            {
                //乱数
                int criticalNum = Random.Range(0, 100);
                //発生確率
                int activeCritical = (status[(int)StatusName.LUK] / 2);
                //クリティカル倍率に用いるLUKが1未満にならないようにする
                if (activeCritical < 1)
                {
                    activeCritical = 1;
                }
                //criticalNumが一定の値以下だとクリティカルになる
                if (criticalNum <= activeCritical)
                {
                    critical = true;
                }
                weapon.currentAttack = status[(int)StatusName.STR];
                animator.SetTrigger("Attack");
            }
        }
        //徘徊
        else
        {
            Vector3 movePos;
            if (moveTimer >= moveLimit)
            {
                //自身を中心とした一定範囲内の全てのNavMeshを検索対象にする
                int moveLayerMask = -1;
                //ランダムな方向を取得
                Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
                randomDirection.y = 0;
                //目的地までのオフセットを取得
                randomDirection += transform.position;
                NavMeshHit navhit;
                //NavMesh上のみを移動するように調整
                NavMesh.SamplePosition(randomDirection, out navhit, moveRadius, moveLayerMask);
                //目的地を設定
                movePos = navhit.position;
                //目的地の方に向く(水平回転のみ)
                transform.forward = randomDirection;
                //目的地へ移動
                agent.SetDestination(movePos);
                //タイマーをリセット
                moveTimer = 0.0f;
            }
            else
            {
                //徘徊先の変更までの時間を計測
                moveTimer += Time.deltaTime;
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

        //移動状態の切り替え
        isChase = CheckPlayer();
        //追跡中
        if (isChase)
        {
            //ダッシュ倍率を1.2にする
            dashSpeed = 1.2f;
            //アニメーションを走行にする
            animaSetNum = 1.0f;
        }
        //徘徊中
        else
        {
            //ダッシュ倍率を1にする
            dashSpeed = 1.0f;
            //アニメーションを歩行にする
            animaSetNum = 0.5f;
        }
        //移動速度
        agent.speed = status[(int)StatusName.AGI] / 8 * dashSpeed;
        //アニメーション再生
        animator.SetFloat(animatorName, animaSetNum);
    }

    public bool CheckPlayer()
    {
        Vector3 playerDirection = (playerPos.position - transform.position);
        float distance = playerDirection.magnitude;

        if (distance > chaseRange)
            return false;

        playerDirection.Normalize();

        //視野角チェック
        float angle = Vector3.Angle(transform.forward, playerDirection);
        if (angle > viewAngle / 2.0f)
            return false;
        //Raycastで視界に入っているかどうか
        if(!Physics.Raycast(transform.position+Vector3.up*1.5f,playerDirection,distance,otherLayerMasks))
        {
            //プレイヤーを視認している
            return true;
        }

        return false;
    }
}
