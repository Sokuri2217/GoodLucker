using UnityEngine;

public class StatusChangerManager : MonoBehaviour
{
    [Header("使用許可フラグ")]
    public bool isActive;
    public float reActiveLimit; //再度使用できるまでの時間
    public float reActiveTimer; //計測用
    [Header("スクリプト参照")]
    public CharacterBase character;

    public void Start()
    {
        character = GameObject.FindWithTag("Player").GetComponent<CharacterBase>();
    }

    public void Update()
    {
        //クールタイム処理
        if(isActive)
        {
            //一定時間経過で再使用可能
            reActiveTimer += Time.deltaTime;
            if (reActiveTimer >= reActiveLimit)
            {
                isActive = false;
                reActiveTimer = 0;
            }
        }
    }

    public float RandomStatusChange(float add, int statusName)
    {
        //ステータス倍率の抽選(7割:上昇,3割:減少)
        int random = Random.Range(0,100);
        //上昇
        if (random < 70) 
        {
            add += character.status[statusName] / 100;
        }
        //減少
        else
        {
            add = character.status[statusName] / 100;
        }
        //クールタイムに入る
        isActive = true;

        return add;
    }
}
