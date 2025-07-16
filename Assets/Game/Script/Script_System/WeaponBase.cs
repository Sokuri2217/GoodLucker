using Unity.VisualScripting;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("攻撃力")]
    public int currentAttack;
    [Header("コライダー")]
    public new Collider collider;

    public CharacterBase character;

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
        character = other.GetComponent<CharacterBase>();
        if (other.gameObject.CompareTag("Enemy") && !character.invincible) 
        {
            Debug.Log("当たったよ");
            character.currentHp = character.TakeDamage(currentAttack);
        }
    }
}
