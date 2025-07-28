using UnityEngine;
using UnityEngine.AI;

public class NecromancerController : BossController
{
    [Header("移動関連")]
    public bool isAway;                //逃亡中かどうか
    public float awayDistance;         //プレイヤーから離れる距離
    public float awayRadius;           //移動範囲
    [Header("敵生成")]
    public GameObject[] zombie = new GameObject[2]; //敵(雑魚・中ボス)
    public float[] createLimit=new float[2];        //生成間隔
    public float[] createTimer=new float[2];        //計測用
    public int[] createEnemyLimit = new int[2];     //敵の生成数上限
    public int[] createEnemyCount = new int[2];     //現在の生成数
    public float createRadius;                      //生成範囲
    public bool createStrong;                       //中ボス生成可能フラグ
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
            CreateEnemy();
        }
        //移動
        {
            UniqueEnemyMove();

            //徘徊中でないときは逃亡
            if (isWalk)
            {
                isAway = false;
            }
            else
            {
                isAway = true;
            }
        }
    }

    //能力
    //強化
    public void PowerUpEnemy()
    {
        for (int i = 0; i <= (int)StatusName.LUK; i++) 
        {
            //自身のステータスの3割を設定
            addEnemyStatus[i] = status[i] * 0.3f;
        }
    }
    //生成
    public void CreateEnemy()
    {
        //雑魚敵
        if (createEnemyCount[0] < createEnemyLimit[0]) 
        {
            //一定間隔で敵を生成
            createTimer[0] += Time.deltaTime;
            if (createTimer[0] >= createLimit[0])
            {
                createTimer[0] = 0;
                // 自身を中心としたランダムな位置を取得（XZ平面）
                Vector3 randomDirection = Random.insideUnitCircle * (createRadius * 10);
                Vector3 randomPosition = transform.position + new Vector3(randomDirection.x, 0, randomDirection.y);

                // NavMesh上の位置をサンプルする
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPosition, out hit, 5.0f, NavMesh.AllAreas)) 
                {
                    // 有効な位置が見つかったら、そこでオブジェクトを生成
                    createEnemyCount[0]++;
                    Instantiate(zombie[0], hit.position, Quaternion.identity);
                    return;
                }
            }
        }
        //中ボス
        if(createEnemyCount[1] < createEnemyLimit[1])
        {
            //逃亡中に生成できる
            if (createStrong)
            {
                if (isAway)
                {
                    createTimer[1] = 0;
                    createStrong = false;

                    // 自身を中心としたランダムな位置を取得（XZ平面）
                    Vector3 randomDirection = Random.insideUnitCircle * createRadius;
                    Vector3 randomPosition = transform.position + new Vector3(randomDirection.x, 0, randomDirection.y);

                    // NavMesh上の位置をサンプルする
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPosition, out hit, 5.0f, NavMesh.AllAreas))
                    {
                        // 有効な位置が見つかったら、そこでオブジェクトを生成
                        createEnemyCount[1]++;
                        Instantiate(zombie[1], hit.position, Quaternion.identity);
                        return;
                    }
                }
            }
            else
            {
                //時間経過で再度生成可能
                createTimer[1] += Time.deltaTime;
                if (createTimer[1] >= createLimit[1])
                {
                    createTimer[1] = 0;
                    createStrong = true;
                }
            }
        }
    }

    //逃亡
    public void UniqueEnemyMove()
    {
        if ((isAway && !isWalk) || invincible) 
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
}
