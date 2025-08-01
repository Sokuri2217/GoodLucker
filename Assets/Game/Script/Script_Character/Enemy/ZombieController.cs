using UnityEngine;

public class ZombieController : ZakoController
{
    [Header("消滅までの時間")]
    public float deleteLimit; //制限時間
    public float deleteTimer; //計測用
    [Header("強化間隔")]
    public float powerUpLimit; //強化されるまでの時間
    public float powerUpTimer; //計測用
    [Header("敵種判別用")]
    public int enemyNum;       //敵数の増減処理に使用
    [Header("スクリプト参照")]
    private NecromancerController necromancer; //ボス

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //スクリプト取得
        necromancer = GameObject.Find("Boss_Necromancer").GetComponent<NecromancerController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (necromancer != null)
        {
            //強化処理
            PowerUpEnemy();
            //殺害処理
            PlayerKill();
            //自然消滅の処理
            HealingBoss();
        }
    }

    //強化処理
    public void PowerUpEnemy()
    {
        //一定時間経過ごとに強化
        powerUpTimer += Time.deltaTime;
        if (powerUpTimer >= powerUpLimit)
        {
            for (int i = 0; i <= (int)StatusName.LUK; i++) 
            {
                status[i] += (int)necromancer.addEnemyStatus[i];
            }
            //タイマーをリセット
            powerUpTimer = 0;
        }
    }

    //殺害処理
    public void PlayerKill()
    {
        //体力が0になったら
        if (currentHp <= 0)
        {
            //自身の最大HPの半分をボスにダメージとして与える
            float reflectionDamage = (maxHp / 2);
            necromancer.currentHp -= (int)reflectionDamage;
            necromancer.createEnemyCount[enemyNum]--;
        }
    }

    //自然消滅の処理
    public void HealingBoss()
    {
        //時間経過
        deleteTimer += Time.deltaTime;
        if (deleteTimer >= deleteLimit)
        {
            deleteTimer = 0.0f;
            //自身の体力の半分をボスに与え回復させる
            necromancer.currentHp += (currentHp / 2);
            //自身は消去
            Destroy(gameObject);
        }
    }
}
