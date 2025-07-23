using UnityEngine;

public class EnemyBase : CharacterBase
{
    [Header("攻撃")]
    public bool isAttack; //攻撃中
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

        if( playerController==null)
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
        if (playerPos == null)
        {
            playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
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
