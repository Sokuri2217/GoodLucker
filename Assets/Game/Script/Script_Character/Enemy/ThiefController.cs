using UnityEngine;

public class ThiefController : ZakoController
{
    //敵の生成
    private EnemySpown enemySpown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //スクリプト取得
        enemySpown = GameObject.Find("EnemySpown").GetComponent<EnemySpown>();

        //初回設定
        enemySpown.createEnemyCount++;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //死亡処理
        if (currentHp <= 0) 
        {
            enemySpown.createEnemyCount--;
        }
    }
}
