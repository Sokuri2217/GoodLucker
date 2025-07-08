using UnityEngine;
using UnityEngine.AI;

public class CharacterBase : MonoBehaviour
{
    [Header("体力")]
    public float maxHp;      //最大体力
    public float currentHp;  //現在の体力
    [Header("ステータス(攻撃・防御・速度・運)")]
    public float[] status; //ステータス

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
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
