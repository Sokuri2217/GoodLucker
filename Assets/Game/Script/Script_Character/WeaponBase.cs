using Unity.VisualScripting;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("攻撃")]
    public int currentAttack;   //攻撃力
    public string enemyTag;     //攻撃対象のタグ
    [Header("コライダー")]
    public new Collider collider; //当たり判定
    [Header("SE")]
    public AudioSource seSource;     //コンポーネント
    public AudioClip basicDamage;    //通常被弾
    public AudioClip criticalDamage; //クリティカル被弾
    [Header("スクリプト参照")]
    public CharacterBase character; //ゲーム内のキャラクター全般
    public SEManager se;            //効果音

    //ステータス識別用
    protected enum StatusName
    {
        STR,
        DEF,
        AGI,
        LUK,
    }

    protected virtual void Start()
    {
        //コンポーネント取得
        collider = GetComponent<Collider>();
        seSource = GetComponent<AudioSource>();
        //スクリプト取得
        se = GameObject.Find("SEManager").GetComponent<SEManager>();
        //当たり判定を無効化
        collider.enabled = false;
        //SEManagerから音量を取得
        seSource.volume = se.seSource.volume;
    }

    //当たり判定を有効化
    public void HitActive()
    {
        collider.enabled = true;
    }

    //当たり判定を無効化
    public void HitInactive()
    {
        collider.enabled = false;
    }

    //ダメージ処理
    public void OnTriggerEnter(Collider other)
    {
        character = other.GetComponentInParent<CharacterBase>();
        if (other.gameObject.CompareTag(enemyTag) && !character.invincible && !character.skillAvoid)  
        {
            if(character.critical)
            {
                //クリティカル音
                seSource.PlayOneShot(criticalDamage);
            }
            //攻撃音
            seSource.PlayOneShot(basicDamage);
            character.currentHp = character.TakeDamage(currentAttack);
        }
    }
}
