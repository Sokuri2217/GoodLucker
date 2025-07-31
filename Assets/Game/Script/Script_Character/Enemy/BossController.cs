using Unity.VisualScripting;
using UnityEngine;

public class BossController : EnemyBase
{
    [Header("アイテム取得")]
    public string changerLayerName;        //Layer名
    public float rayRange;                 //Rayの距離
    public LayerMask changerlayerMasks;    //レイヤー指定
    public bool searchChanger;             //変化オブジェクトを見つけたかどうか
    public bool isDie;                     //死亡フラグ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //変化オブジェクト使用
        //searchChanger = SearchChanger();

        //死亡処理
        if (currentHp <= 0 && !isDie)  
        {
            DeleteObject();
        }
    }

    ////ステータス変化
    //public bool  SearchChanger()
    //{
    //    Vector3 changerDirection = (playerPos.position - transform.position);
    //    float distance = changerDirection.magnitude;

    //    if (distance > searchRange)
    //        return false;

    //    changerDirection.Normalize();

    //    //視野角チェック
    //    float angle = Vector3.Angle(transform.forward, changerDirection);
    //    if (angle > viewAngle / 2.0f)
    //        return false;
    //    //Raycastで視界に入っているかどうか
    //    if (Physics.Raycast(transform.position + Vector3.up * 1.5f, changerDirection, distance, changerlayerMasks))
    //    {
    //        //Rayがオブジェクトに当たったとき
    //        //layer名を保存
    //        changerLayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
    //        //スクリプト情報を取得
    //        StatusChangerManager statusChangerManager = hit.collider.gameObject.GetComponent<StatusChangerManager>();
    //        //ステータスを変化させる処理
    //        if (changerLayerName != null && !statusChangerManager.isActive)
    //        {
    //            switch (changerLayerName)
    //            {
    //                case "Attack":
    //                    addStatus[(int)StatusName.STR] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.STR], (int)StatusName.STR);
    //                    break;
    //                case "Defence":
    //                    addStatus[(int)StatusName.DEF] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.DEF], (int)StatusName.DEF);
    //                    break;
    //                case "Speed":
    //                    addStatus[(int)StatusName.AGI] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.AGI], (int)StatusName.AGI);
    //                    break;
    //                case "Luck":
    //                    addStatus[(int)StatusName.LUK] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.LUK], (int)StatusName.LUK);
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }

    //        //オブジェクトを視認している
    //        return true;
    //    }

    //    return false;
    //}

    ////ステータス変化アイテム取得
    //public void GetStatusChanger()
    //{
    //    //目の前にあるオブジェクトのLayer名を取得
    //    Ray ray = new Ray(transform.position, transform.forward);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, rayRange, layerMasks))
    //    {
    //        //Rayがオブジェクトに当たったとき
    //        //layer名を保存
    //        changerLayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
    //        //スクリプト情報を取得
    //        StatusChangerManager statusChangerManager = hit.collider.gameObject.GetComponent<StatusChangerManager>();
    //        //ステータスを変化させる処理
    //        if (changerLayerName != null && !statusChangerManager.isActive)
    //        {
    //            switch (changerLayerName)
    //            {
    //                case "Attack":
    //                    addStatus[(int)StatusName.STR] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.STR], (int)StatusName.STR);
    //                    break;
    //                case "Defence":
    //                    addStatus[(int)StatusName.DEF] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.DEF], (int)StatusName.DEF);
    //                    break;
    //                case "Speed":
    //                    addStatus[(int)StatusName.AGI] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.AGI], (int)StatusName.AGI);
    //                    break;
    //                case "Luck":
    //                    addStatus[(int)StatusName.LUK] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.LUK], (int)StatusName.LUK);
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        changerLayerName = "";
    //    }
    //}

    //削除処理
    public void DeleteObject()
    {
        uiStage.killBossCount++;
        Destroy(gameObject);
    }
}

