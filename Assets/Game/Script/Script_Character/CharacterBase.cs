using UnityEngine;
using UnityEngine.AI;

public class CharacterBase : MonoBehaviour
{
    [Header("ステータス")]
    public int maxHp;     //最大体力
    public int currentHp; //現在の体力
    public int attack;    //攻撃力
    public int defence;   //防御力
    public int speed;     //移動速度
    public int luck;      //運

    [Header("上昇率")]
    public float addHp;      //体力
    public float addAttack;  //攻撃力
    public float addDefence; //防御力
    public float addSpeed;   //移動速度

    [Header("アニメーション")]
    public string animatorName; //BlendTree名

    [Header("コンポーネント参照")]
    protected Rigidbody rb;       //物理挙動
    protected NavMeshAgent agent; //経路探索
    protected Animator animator;  //アニメーション


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //コンポーネント取得
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //初期化
        currentHp = maxHp;   //体力
        agent.speed = speed; //移動速度
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
