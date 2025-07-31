using UnityEngine;
using UnityEngine.AI;

public class EnemySpown : MonoBehaviour
{
    [Header("敵生成")]
    public GameObject enemy;     //スポーンさせる敵
    public float createLimit;    //生成間隔
    public float createTimer;    //計測用
    public int createEnemyLimit; //敵の生成数上限
    public int createEnemyCount; //現在の生成数
    public float createRadius;   //生成範囲

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //敵を生成
        CreateEnemy();
    }

    public void CreateEnemy()
    {
        //雑魚敵
        if (createTimer >= createLimit)
        {
            //敵の生成上限に達するまでは生成する
            if (createEnemyCount < createEnemyLimit)
            {
                createTimer = 0;
                // 自身を中心としたランダムな位置を取得（XZ平面）
                Vector3 randomDirection = Random.insideUnitCircle * createRadius;
                Vector3 randomPosition = transform.position + new Vector3(randomDirection.x, 0, randomDirection.y);

                // NavMesh上の位置をサンプルする
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPosition, out hit, 5.0f, NavMesh.AllAreas))
                {
                    // 有効な位置が見つかったら、そこでオブジェクトを生成
                    Instantiate(enemy, hit.position, Quaternion.identity, null);
                }
            }
        }
        else
        {
            createTimer += Time.deltaTime;
        }
    }
}
