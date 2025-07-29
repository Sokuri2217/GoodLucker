using Unity.VisualScripting;
using UnityEngine;

public class BossController : EnemyBase
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

        //変化オブジェクト使用
        SearchChanger();

        //死亡処理
        if (currentHp <= 0) 
        {
            //体力が0以下になったら死亡アニメーションを再生
            animator.SetTrigger("Die");
        }
    }

    //ステータス変化
    public bool  SearchChanger()
    {
        Vector3 changerDirection = (playerPos.position - transform.position);
        float distance = changerDirection.magnitude;

        if (distance > searchRange)
            return false;

        changerDirection.Normalize();

        //視野角チェック
        float angle = Vector3.Angle(transform.forward, changerDirection);
        if (angle > viewAngle / 2.0f)
            return false;
        //Raycastで視界に入っているかどうか
        if (!Physics.Raycast(transform.position + Vector3.up * 1.5f, changerDirection, distance, otherLayerMasks))
        {
            //プレイヤーを視認している
            return true;
        }

        return false;
    }

    //削除処理
    public void DeleteObject()
    {
        uiStage.killBossCount++;
        Destroy(gameObject);
    }
}

