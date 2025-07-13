using UnityEngine;
using UnityEngine.AI;

public class CharacterBase : MonoBehaviour
{
    [Header("体力")]
    public float maxHp;      //最大体力
    public float currentHp;  //現在の体力
    [Header("ステータス(攻撃・防御・速度・運)")]
    public float[] originStatus = new float[4]; //初期ステータス
    public float[] status = new float[4];       //ステータス

    [Header("上昇率(攻撃・防御・速度・運)")]
    public float[] addStatus = new float[4];

    [Header("ステータス変化の持続時間(攻撃・防御・速度・運)")]
    public float[] addStatusLimit = new float[4]; //制限時間
    public float[] addStatusTimer = new float[4]; //計測用

    [Header("アニメーション")]
    public string animatorName; //BlendTree名

    [Header("コンポーネント参照")]
    protected Rigidbody rb;       //物理挙動
    protected NavMeshAgent agent; //経路探索
    protected Animator animator;  //アニメーション

    [Header("スクリプト参照")]
    protected GameManager gameManager; //ゲーム基盤

    protected enum StatusName
    {
        STR,
        DEF,
        AGI,
        LUK,
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //コンポーネント取得
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //ステータス設定
        //体力
        currentHp = maxHp;
        //その他
        gameManager = GameObject.Find("SelectManager").GetComponent<GameManager>();
        for (int i = (int)StatusName.STR; i <= ((int)StatusName.LUK); i++) 
        {
            //選んだキャラクターに応じて、ステータスを設定
            status[i] = gameManager.status[i];
            //初期値を保存
            originStatus[i] = status[i];
            //NavMeshAgentの設定
            if (i == (int)StatusName.AGI)
                agent.speed = status[i];
        }

        //タイマー設定
        for (int i = (int)StatusName.STR; i <= (int)StatusName.LUK; i++)
        {
            addStatusTimer[i] = addStatusLimit[i];
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //ステータス更新
        for (int i = (int)StatusName.STR; i <= (int)StatusName.LUK; i++) 
        {
            status[i] = originStatus[i] * addStatus[i];
        }

        //ステータス関係
        ChangeStatus();
    }

    //ステータス関係
    public void ChangeStatus()
    {
        //ステータスに変化が発生したときの処理
        for (int i = (int)StatusName.STR; i <= (int)StatusName.LUK; i++) 
        {
            //いずれかのステータスに倍率がかかったとき
            if (addStatus[i] > 1.0f || addStatus[i] < 1.0f) 
            {
                //時間計測
                addStatusTimer[i] -= Time.deltaTime;
                //一定時間経過で、元のステータスに戻る
                if(addStatusTimer[i] <= 0.0f)
                {
                    //倍率を元に戻す
                    addStatus[i] = 1.0f;
                    //タイマーを再設定
                    addStatusTimer[i] = addStatusLimit[i];
                }
            }
        }
    }
}
