using UnityEngine;
using UnityEngine.AI;

public class CharacterBase : MonoBehaviour
{
    [Header("体力")]
    public int maxHp;      //最大体力
    public int currentHp;  //現在の体力
    [Header("ステータス(攻撃・防御・速度・運)")]
    public int[] originStatus = new int[4]; //初期ステータス
    public int[] status = new int[4];       //ステータス
    public bool critical;                   //クリティカル判定
    [Header("上昇率(攻撃・防御・速度・運)")]
    public float[] addStatus = new float[4];
    [Header("ステータス変化の持続時間(攻撃・防御・速度・運)")]
    public float[] addStatusLimit = new float[4]; //制限時間
    public float[] addStatusTimer = new float[4]; //計測用
    public bool[]  isAdd = new bool[4];           //変化中
    [Header("無敵判定")]
    public bool invincible; //無敵中かどうか
    public float inviLimit; //無敵時間
    public float inviTimer; //計測用
    [Header("アニメーション")]
    public string animatorName; //BlendTree名
    [Header("コンポーネント参照")]
    protected Rigidbody rb;       //物理挙動
    protected NavMeshAgent agent; //経路探索
    protected Animator animator;  //アニメーション
    [Header("スクリプト参照")]
    protected GameManager gameManager; //ゲーム基盤
    protected UIStage uiStage;         //ステージUI

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
        //スクリプト取得
        gameManager = GameObject.Find("SelectManager").GetComponent<GameManager>();
        uiStage = GameObject.FindWithTag("UI").GetComponent<UIStage>();

        //ステータス設定
        //体力
        currentHp = maxHp;

        //タイマー設定
        for (int i = (int)StatusName.STR; i <= (int)StatusName.LUK; i++)
        {
            addStatusTimer[i] = addStatusLimit[i];
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //HP管理
        if (currentHp <= 0)
        {
            //HPが0未満にならないようにする
            currentHp = 0;
        }
        if(currentHp >= maxHp)
        {
            //現在のHPが最大Hpを超えないようにする
            currentHp = maxHp;
        }
        //ステータス更新
        for (int i = (int)StatusName.STR; i <= (int)StatusName.LUK; i++) 
        {
            if (isAdd[i]) 
            {
                status[i] = (int)(originStatus[i] * addStatus[i]);
            }
        }
        //無敵状態の解除
        if(invincible)
        {
            //一定時間経過で解除
            inviTimer += Time.deltaTime;
            if (inviTimer >= inviLimit) 
            {
                invincible = false;
                inviTimer = 0.0f;
            }
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
                //変化フラグをtrueにする
                isAdd[i] = true;
                //時間計測
                addStatusTimer[i] -= Time.deltaTime;
                //一定時間経過で、元のステータスに戻る
                if(addStatusTimer[i] <= 0.0f)
                {
                    //倍率を元に戻す
                    addStatus[i] = 1.0f;
                    //ステータスを元にも戻す
                    status[i] = originStatus[i];
                    //変化フラグをfalseにする
                    isAdd[i] = false;
                    //タイマーを再設定
                    addStatusTimer[i] = addStatusLimit[i];
                }
            }
        }
    }

    //クリティカル
    public bool ActiveCritical()
    {
        //乱数
        int criticalNum = Random.Range(0, 100);
        //発生確率
        int activeCritical = (status[(int)StatusName.LUK] / 2);
        //クリティカル倍率に用いるLUKが1未満にならないようにする
        if (activeCritical < 1)
        {
            activeCritical = 1;
        }
        //criticalNumが一定の値以下だとクリティカルになる
        if (criticalNum <= activeCritical)
        {
            return true;
        }
        return false;
    }

    //ダメージ処理
    public int TakeDamage(int damage)
    {
        //無敵ではないときにダメージを通す
        if(!invincible)
        {
            if(!critical)
            {
                //防御力を基にダメージ量を計算
                float defence = ((float)status[(int)StatusName.DEF] / 200.0f);
                int acitveDamage = (int)(damage * (1.0f - defence));
                currentHp -= acitveDamage;
            }
            else
            {
                //相手の防御力を無視し、さらに攻撃力を上げる
                //キャラクターのAGIの値に応じて倍率を変える
                float addCritical = (1.0f + ((float)status[(int)StatusName.AGI] / 200.0f));
                int activeDamage = (int)(damage * addCritical);
                currentHp -= activeDamage;
            }
            //無敵状態にする
            invincible = true;
            //クリティカル判定をリセット
            critical = false;
        }

        return currentHp;
    }
}
