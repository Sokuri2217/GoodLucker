using UnityEngine;
using UnityEngine.AI;

public class NecromancerController : BossController
{
    [Header("移動関連")]
    public float dashSpeed;            //ダッシュ
    public float viewAngle;            //視野角
    public float searchRange;           //プレイヤーを検知する距離
    public float moveRadius;           //徘徊する範囲(移動先の範囲)
    public float moveLimit;            //移動先の変更時間
    public float moveTimer;            //計測用
    public bool isAway;                //逃亡中かどうか
    public LayerMask otherLayerMasks;  //レイヤー指定(その他)
    public float animaSetNum;          //アニメーション制御
    public float awayDistance;         //プレイヤーから離れる距離
    public float awayRadius;           //移動範囲
    [Header("雑魚の強化倍率")]
    public float[] addEnemyStatus = new float[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //能力
        {
            PowerUpEnemy();
        }
        //移動
        {
            Move3D();
        }
    }

    //能力
    public void PowerUpEnemy()
    {
        for (int i = 0; i <= (int)StatusName.LUK; i++) 
        {
            //自身のステータスの3割を設定
            addEnemyStatus[i] = status[i] * 0.3f;
        }
    }

    //移動
    public void Move3D()
    {
        //徘徊中
        if(!isAway)
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
        //逃亡中
        else
        {
            // プレイヤーの方向に反対向きの単位ベクトルを計算
            Vector3 fleeDirection = (transform.position - playerPos.position).normalized;

            // 逃げる目的地をプレイヤーから離れる方向に計算
            Vector3 desiredFleePos = transform.position + fleeDirection * awayDistance;

            // desiredFleePosがNavMesh上に存在するか確認
            NavMeshHit hit;
            if (NavMesh.SamplePosition(desiredFleePos, out hit, awayRadius, NavMesh.AllAreas))
            {
                // 計算した位置がプレイヤーからさらに遠いか確認
                if ((hit.position - playerPos.position).sqrMagnitude > (transform.position - playerPos.position).sqrMagnitude)
                {
                    // プレイヤーからさらに遠い位置が見つかれば、その位置に向かう
                    agent.SetDestination(hit.position);
                }
                else
                {
                    // そうでなければ、最適な逃げ場所を探す
                    SearchAwayPoint();
                }
            }
            else
            {
                // NavMesh上に無理に行けない位置だった場合、最適な逃げ場所を探す
                SearchAwayPoint();
            }

            transform.forward = fleeDirection;
        }

        //移動状態
        isAway = SearchPlayer();
        //逃亡中
        if (isAway)
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

    //逃亡先を探す
    public void SearchAwayPoint()
    {
        Vector3 bestPos = transform.position;  // 初期位置を最適な位置として設定
        float bestDist = 0f;  // 最適位置との距離

        // 周囲をランダムに10回探索して、最もプレイヤーから遠い位置を見つける
        for (int i = 0; i < 10; i++)
        {
            // ランダムな方向に探索範囲を広げる
            Vector3 randomDir = Random.insideUnitSphere * awayRadius;
            randomDir += transform.position;

            // ランダムな位置がNavMesh上に存在するか確認
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, 2f, NavMesh.AllAreas))
            {
                // プレイヤーからの距離を計算
                float dist = (hit.position - playerPos.position).sqrMagnitude;

                // プレイヤーから最も遠い位置を更新
                if (dist > bestDist)
                {
                    bestDist = dist;
                    bestPos = hit.position;
                }
            }
        }

        // 最もプレイヤーから遠い位置を目的地に設定
        agent.SetDestination(bestPos);

    }

    //視界にプレイヤーがいるかを確認
    public bool SearchPlayer()
    {
        Vector3 playerDirection = (playerPos.position - transform.position);
        float distance = playerDirection.magnitude;

        if (distance > searchRange)
            return false;

        playerDirection.Normalize();

        //視野角チェック
        float angle = Vector3.Angle(transform.forward, playerDirection);
        if (angle > viewAngle / 2.0f)
            return false;
        //Raycastで視界に入っているかどうか
        if (!Physics.Raycast(transform.position + Vector3.up * 1.5f, playerDirection, distance, otherLayerMasks))
        {
            //プレイヤーを視認している
            return true;
        }

        return false;
    }
}
