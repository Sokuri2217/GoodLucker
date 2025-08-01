using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : CharacterBase
{
    [Header("移動")]
    public float dashSpeed;            //ダッシュ
    public float viewAngle;            //視野角
    public float searchRange;           //プレイヤーを検知する距離
    public float moveRadius;           //徘徊する範囲(移動先の範囲)
    public float moveLimit;            //移動先の変更時間
    public float moveTimer;            //計測用
    public LayerMask otherLayerMasks;  //レイヤー指定(その他)
    public float animaSetNum;          //アニメーション制御
    public bool isWalk;                //徘徊フラグ
    [Header("攻撃")]
    public bool isAttack;         //攻撃中
    public bool readyAttack;      //攻撃可能
    public float attackCoolLimit; //クールタイム
    public float attackCoolTimer; //計測用
    [Header("プレイヤー参照")]
    public Transform playerPos;
    public PlayerController playerController;
    [Header("スクリプト参照")]
    public WeaponBase weapon; //攻撃判定

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //ステータス設定
        for (int i = (int)StatusName.STR; i <= ((int)StatusName.LUK); i++)
        {
            //初期値を保存
            originStatus[i] = status[i];
            //NavMeshAgentの設定
            if (i == (int)StatusName.AGI)
                agent.speed = status[i];
        }

        //攻撃対象をPlayerに設定
        weapon.enemyTag = "Player";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!uiStage.isGame) return;

        if (uiStage.isGame)
        {
            if (playerController == null)
            {
                playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            }
            if (playerPos == null)
            {
                playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
        }

        //徘徊
        BasicEnemyMove();

        //移動状態
        isWalk = SearchPlayer();

        //攻撃のクールタイム
        if (!readyAttack) 
        {
            attackCoolTimer += Time.deltaTime;
            if (attackCoolTimer >= attackCoolLimit) 
            {
                readyAttack = true;
                attackCoolTimer = 0;
            }
        }

        //徘徊中
        if (isWalk)
        {
            //ダッシュ倍率を1にする
            dashSpeed = 1.0f;
            //アニメーションを歩行にする
            animaSetNum = 0.5f;
        }
        //追跡or逃亡
        else
        {
            //ダッシュ倍率を1.3にする
            dashSpeed = 1.3f;
            //アニメーションを走行にする
            animaSetNum = 1.0f;
        }
        //移動速度
        agent.speed = status[(int)StatusName.AGI] / 8 * dashSpeed;
        //アニメーション再生
        animator.SetFloat(animatorName, animaSetNum);
    }

    //徘徊処理
    public void BasicEnemyMove()
    {
        if(isWalk)
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
    }

    //視界にプレイヤーがいるかを確認(true = 徘徊・false = プレイヤー発見)
    public bool SearchPlayer()
    {
        Vector3 playerDirection = (playerPos.position - transform.position);
        float distance = playerDirection.magnitude;

        if (distance > searchRange)
            return true;

        playerDirection.Normalize();

        //視野角チェック
        float angle = Vector3.Angle(transform.forward, playerDirection);
        if (angle > viewAngle / 2.0f)
            return true;
        //Raycastで視界に入っているかどうか
        if (!Physics.Raycast(transform.position + Vector3.up * 1.5f, playerDirection, distance, otherLayerMasks))
        {
            //プレイヤーを視認している
            return false;
        }

        return true;
    }

    //攻撃判定の有効化
    public void ActiveAttack()
    {
        weapon.HitActive();
    }

    //攻撃判定の無効化
    public void InactiveAttack()
    {
        weapon.HitInactive();
    }
}
