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

    [Header("コンポーネント参照")]
    public Rigidbody rb;       //物理挙動
    public NavMeshAgent agent; //経路探索


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //コンポーネント取得
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        //初期化
        currentHp = maxHp;   //体力
        agent.speed = speed; //移動速度
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
