using UnityEngine;

public class NecromancerController : BossController
{
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
    }
    public void PowerUpEnemy()
    {
        for (int i = 0; i <= (int)StatusName.LUK; i++) 
        {
            //自身のステータスの3割を設定
            addEnemyStatus[i] = status[i] * 0.3f;
        }
    }
}
