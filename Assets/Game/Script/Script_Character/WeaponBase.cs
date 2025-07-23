using Unity.VisualScripting;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("攻撃")]
    public int currentAttack;   //攻撃力
    public string enemyTag;     //攻撃対象のタグ
    [Header("コライダー")]
    public new Collider collider; //当たり判定
    [Header("スクリプト参照")]
    public CharacterBase character; //ゲーム内のキャラクター全般

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
        //当たり判定を無効化
        collider.enabled = false;
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
        if (other.gameObject.CompareTag("Player") && !character.invincible) 
        {
            Debug.Log("被弾した");
            character.currentHp = character.TakeDamage(currentAttack);
            
        }
    }
}
