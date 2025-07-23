using UnityEngine;

public class StatusChangerManager : MonoBehaviour
{
    [Header("使用許可フラグ")]
    public bool isActive;
    public float reActiveLimit; //再度使用できるまでの時間
    public float reActiveTimer; //計測用
    [Header("マテリアル")]
    public Material defaultTexture;  //使用可能
    public Material coolDownTexture; //使用不可
    [Header("コンポーネント参照")]
    public MeshRenderer meshRenderer; //描画
    [Header("スクリプト参照")]
    public CharacterBase character; //キャラクター

    public void Start()
    {
        //コンポーネント取得
        meshRenderer = GetComponent<MeshRenderer>();
        //スクリプト取得
        character = GameObject.FindWithTag("Player").GetComponent<CharacterBase>();
    }

    public void Update()
    {
        //クールタイム処理
        if(isActive)
        {
            //テクスチャ変更
            meshRenderer.material = coolDownTexture;
            //一定時間経過で再使用可能
            reActiveTimer += Time.deltaTime;
            if (reActiveTimer >= reActiveLimit)
            {
                isActive = false;
                //テクスチャを元に戻す
                meshRenderer.material = defaultTexture;
                reActiveTimer = 0;
            }
        }
    }

    public float RandomStatusChange(float add, int statusName)
    {
        //倍率をリセット(実装は未確定)
        add = 1.0f;
        //ステータス倍率の抽選(7割:上昇,3割:減少)
        int random = Random.Range(0,100);
        //上昇
        if (random < 70) 
        {
            add += (float)character.status[statusName] / 100;
        }
        //減少
        else
        {
            add = (float)character.status[statusName] / 100;
        }
        //クールタイムに入る
        isActive = true;

        return add;
    }
}
