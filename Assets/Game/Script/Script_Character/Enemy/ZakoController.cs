using UnityEngine;

public class ZakoController : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //死亡処理
        if (currentHp <= 0)
        {
            //体力が0以下になったらオブジェクトを削除
            Destroy(gameObject);
        }
    }
}
