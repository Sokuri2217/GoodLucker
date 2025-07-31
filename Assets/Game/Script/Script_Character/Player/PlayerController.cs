using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterBase
{
    [Header("ステータス")]
    public float dashSpeed;                      //ダッシュ倍率
    public float avoidDistance;                  //回避処理
    public float avoidSecond;                    //回避時に何秒かけて移動するか

    [Header("攻撃")]
    public float[] IntervalLimit = new float[2];   //各攻撃のクールダウン(通常・特殊)
    public float[] IntervalTimer = new float[2];   //計測用(通常・特殊)
    public int selectSkill;                        //使用中のスキル
    public float reSelectSkillLimit;               //使用後、次にスキルを選択できるまでの時間
    public float reSelectSkillTimer;               //計測用
    public float[] reUseSkillLimit = new float[4]; //スキルのクールタイム
    public float[] reUseSkillTimer = new float[4]; //計測用
    public bool[] isUseSkill = new bool[4];        //使用中のスキル


    [Header("アイテム取得")]
    public string changerLayerName; //Layer名
    public float rayRange;          //Rayの距離
    public LayerMask layerMasks;    //レイヤー指定

    [Header("フラグ")]
    public bool isAvoid;                     //回避中
    public bool basicAttack;                 //通常攻撃
    public bool spSkill;                     //特殊攻撃
    public bool[] attackInput = new bool[2]; //各攻撃入力中
    public bool useChanger;                  //使用可能

    [Header("カメラ参照")]
    public string cameraName;         //参照先の名前
    public Transform cameraTransform; //カメラのTransform

    [Header("回転")]
    public float rotationSpeed; //速度
    [Header("音")]
    public AudioClip useSkillSE; //スキル使用SE
    [Header("スクリプト参照")]
    public WeaponBase weapon;                    //武器
    protected CameraController cameraController; //カメラ
    protected SEManager seManager;               //SE

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //カメラの情報を取得
        GameObject cameraObj = GameObject.Find(cameraName);
        cameraTransform = cameraObj.GetComponent<Transform>();
        cameraController = cameraObj.GetComponent<CameraController>();

        //スクリプト取得
        uiStage = GameObject.FindWithTag("UI").GetComponent<UIStage>();
        seManager = GameObject.Find("SEManager").GetComponent<SEManager>();

        //ステータス設定
        for (int i = (int)StatusName.STR; i <= ((int)StatusName.LUK); i++)
        {
            //選んだキャラクターに応じて、ステータスを設定
            status[i] = gameManager.status[i];
            //初期値を保存
            originStatus[i] = status[i];
            //NavMeshAgentの設定
            if (i == (int)StatusName.AGI)
                agent.speed = status[i];
        }
        //初回はスキルを即座に使えるようにする
        reSelectSkillTimer = reSelectSkillLimit;

        //攻撃対象をEnemyに設定
        weapon.enemyTag = "Enemy";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //移動(攻撃中は移動不可)
        {
            if (!basicAttack && !spSkill) 
            {
                if(!isAvoid)
                {
                    //水平方向
                    Move3D();
                }
                //回避
                Avoid3D();
            }
            //プレイヤーが浮くのを防止
            Vector3 playerPos = transform.position;
            playerPos.y = 0.0f;
            transform.position = playerPos;

        }
        //ステータス
        {
            GetStatusChanger();
        }
        //攻撃
        {
            if(uiStage.isGame)
            {
                UseBasicAttack();  //通常
                UseSpSkill();      //特殊
            }
        }
    }

    //水平方向の移動
    public void Move3D()
    {
        //入力(WASD)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //ダッシュ入力(左Shift)
        dashSpeed = Input.GetKey(KeyCode.LeftShift) ? 1.2f : 1.0f;

        Vector3 vec = new Vector3(horizontal, 0.0f, vertical).normalized;

        //移動アニメーション設定
        //基本的に停止アニメーションを設定
        float animaSetNum = 0;
        if (vec.magnitude > 0.1f) 
        {
            //カメラの向きから移動方向を設定
            Vector3 cameraForward = cameraController.transform.forward;
            Vector3 cameraRight = cameraController.transform.right;
            cameraForward.y = 0.0f;
            cameraRight.y = 0.0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            //カメラに合わせてベクトルを設定
            Vector3 setVec = cameraForward * vec.z + cameraRight * vec.x;

            //プレイヤーの移動
            transform.position += setVec * status[(int)StatusName.AGI] / 5 * dashSpeed * Time.deltaTime;

            //プレイヤーを進行方向に合わせて回転させる
            Quaternion playerRotation = Quaternion.LookRotation(setVec);
            Quaternion setRotation = Quaternion.Slerp(transform.rotation, playerRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = setRotation;

            //歩行か走行のときはアニメーションを変更
            animaSetNum = Input.GetKey(KeyCode.LeftShift) ? 1.0f : 0.5f;
        }

        //アニメーション再生
        animator.SetFloat(animatorName, animaSetNum);
    }

    //ジャンプ
    public void Avoid3D()
    {
        //Spaceを押したときにジャンプする
        if (Input.GetKeyDown(KeyCode.Space) && !isAvoid) 
        {
            //アニメーション再生
            animator.SetTrigger("Avoid");
            //移動させる
            StartCoroutine(AvoidMove());
        }
    }

    //通常攻撃
    protected virtual void UseBasicAttack()
    {
        if (!basicAttack && !spSkill) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                critical = ActiveCritical();
                //武器に自身の攻撃力を渡す
                weapon.currentAttack = status[(int)StatusName.STR];
                //アニメーションを再生
                animator.SetTrigger("NormalAttack");
            }
        }
    }

    //特殊攻撃使用
    protected virtual void UseSpSkill()
    {
        //スキル選択画面を表示
        //一時停止中は入力を受け付けない
        //使用後、再度選択できるようになるまで少し時間を空ける
        if (reSelectSkillTimer >= reSelectSkillLimit)
        {
            if (Input.GetKey(KeyCode.R) && uiStage.isGame && !basicAttack)
            {
                spSkill = true;
            }
            else if (!Input.GetKey(KeyCode.R))
            {
                spSkill = false;
            }
        }
        else
        {
            //時間計測
            reSelectSkillTimer += Time.deltaTime;
        }
        //選択スキルの番号を保存
        selectSkill = uiStage.currentSelectSkill;
        //スキル使用
        //左クリックで使用
        if (Input.GetMouseButtonDown(0) && spSkill && !isUseSkill[selectSkill])
        {
            //スキルを使用中にする
            isUseSkill[selectSkill] = true;
            //使用音を鳴らす
            seManager.seSource.PlayOneShot(useSkillSE);
            //選択画面を閉じる
            spSkill = false;
            //タイマーをリセット
            reSelectSkillTimer = 0;
        }
    }

    //ステータス変化アイテム取得
    public void GetStatusChanger()
    {
        //目の前にあるオブジェクトのLayer名を取得
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayRange, layerMasks))
        {
            //Rayがオブジェクトに当たったとき
            //layer名を保存
            changerLayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            //スクリプト情報を取得
            StatusChangerManager statusChangerManager = hit.collider.gameObject.GetComponent<StatusChangerManager>();
            //ステータスを変化させる処理
            if (changerLayerName != null && !statusChangerManager.isActive) 
            {
                useChanger = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    switch (changerLayerName)
                    {
                        case "Attack":
                            addStatus[(int)StatusName.STR] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.STR], (int)StatusName.STR);
                            break;
                        case "Defence":
                            addStatus[(int)StatusName.DEF] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.DEF], (int)StatusName.DEF);
                            break;
                        case "Speed":
                            addStatus[(int)StatusName.AGI] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.AGI], (int)StatusName.AGI);
                            break;
                        case "Luck":
                            addStatus[(int)StatusName.LUK] = statusChangerManager.RandomStatusChange(addStatus[(int)StatusName.LUK], (int)StatusName.LUK);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        else
        {
            changerLayerName = "";
            useChanger = false;
        }
    }

    //回避処理
    private IEnumerator AvoidMove()
    {
        //回避中フラグを立てる
        isAvoid = true;
        //NavMeshAgentを無効化
        agent.enabled = false;
        //Kinematicを無効化
        rb.isKinematic = false;

        //初期座標と回避後の最終座標を設定
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + transform.forward * avoidDistance;

        //計測用
        float avoidTimer = 0.0f;

        //移動処理
        while (avoidTimer < avoidSecond) 
        {
            //avoidSecond分の時間をかけて移動
            transform.position = Vector3.Lerp(startPos, targetPos, avoidTimer / avoidSecond);
            avoidTimer += Time.deltaTime;
            yield return null;
        }

        //最終地点をtargetPosにする
        transform.position = targetPos;
        //再入力可能
        isAvoid = false;
        //NavMeshAgentを有効化
        agent.enabled = true;
        //Kinematicを有効化
        rb.isKinematic = true;
    }

    //攻撃判定の有効化
    public void ActiveAttack()
    {
        weapon.HitActive();
    }

    //攻撃判定の無効化
    public void InactiveAttack()
    {
        weapon.HitInactive();
    }
}
