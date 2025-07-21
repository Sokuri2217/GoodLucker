using UnityEngine;

public class EnemyBase : CharacterBase
{
    [Header("プレイヤー参照")]
    public Transform playerPos;
    public PlayerController playerController;

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
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
