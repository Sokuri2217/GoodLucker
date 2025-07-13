using JetBrains.Annotations;
using UnityEngine;

public class CreateStatusChanger : MonoBehaviour
{
    [Header("生成オブジェクト")]
    public GameObject[] statusChanger = new GameObject[4];

    [Header("スクリプト参照")]
    private UIStage uiStage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //コンポーネント取得
        uiStage = GameObject.Find("StageUI").GetComponent<UIStage>();
        //オブジェクト生成
        while(true)
        {
            //抽選を行い、現在の個数が上限未満ならば生成する
            int createObj = Random.Range(0, 4);
            if (uiStage.createCount[createObj] < uiStage.createLimit[createObj]) 
            {
                Instantiate(statusChanger[createObj], this.transform);
                uiStage.createCount[createObj]++;
                break;
            }
        }
    }
}
