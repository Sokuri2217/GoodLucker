using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterBase
{
    [Header("ステータス")]
    public float dashSpeed;                      //ダッシュ倍率

    [Header("戦闘")]
    public float[] IntervalLimit = new float[2];   //各攻撃のクールダウン(通常・特殊)
    public float[] IntervalTimer = new float[2];   //計測用(通常・特殊)
    public int selectSkill;                        //使用中のスキル
    public float reSelectSkillLimit;               //使用後、次にスキルを選択できるまでの時間
    public float reSelectSkillTimer;               //計測用
    public float[] reUseSkillLimit = new float[4]; //スキルのクールタイム
    public float[] reUseSkillTimer = new float[4]; //計測用
    public bool[] isUseSkill = new bool[4];        //使用中のスキル
    public bool[] activeSkill = new bool[4];       //適応中のスキル
    public float[] activeSkillLimit = new float[4];//スキルの効果時間
    public float[] activeSkillTimer = new float[4];//計測用
    public int beforeHp;                           //Hpの保存

    [Header("アイテム取得")]
    public string changerLayerName; //Layer名
    public float rayRange;          //Rayの距離
    public LayerMask layerMasks;    //レイヤー指定

    [Header("フラグ")]
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
    public AudioClip useSkillSE;         //スキル使用SE
    public AudioClip useStatusChangerSE; //ステータスチェンジャー使用SE
    [Header("スクリプト参照")]
    public WeaponBase weapon;                    //武器
    protected CameraController cameraController; //カメラ
    protected SEManager seManager;               //SE
    protected StatusChangerManager statusChanger;//昇降台

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //UIに自身の情報を渡す
        uiStage.LoadPlayer(this.gameObject);

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

            //スキル関連の初期設定
            //効果時間
            activeSkillTimer[i] = activeSkillLimit[i];
        }
        //初回はスキルを即座に使えるようにする
        reSelectSkillTimer = reSelectSkillLimit;
        //昇降台でステータスが低下するようにする
        badStatus = true;

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
                //水平方向
                Move3D();
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
                ActiveSkill();     //特殊行動の効果適応
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

    //特殊行動使用
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

    //特殊行動の効果
    public void ActiveSkill()
    {
        for (int i = (int)StatusName.STR; i <= (int)StatusName.LUK; i++) 
        {
            //スキル使用フラグがtrueのとき
            if (isUseSkill[i] && activeSkillTimer[i] >= activeSkillLimit[i]) 
            {
                //効果が適応されていない場合
                if (!activeSkill[i])
                {
                    //効果適応処理
                    switch (i)
                    {
                        case (int)StatusName.STR:
                            originStatus[i] *= 3;       //攻撃力を上昇
                            originStatus[(i + 1)] /= 2; //防御力を減少
                            break;
                        case (int)StatusName.DEF:
                            criticalGuard = true; //クリティカルを受けないようにする
                            beforeHp = currentHp; //スキル使用前の体力を保存
                            originStatus[i] *= 2; //防御力を上昇
                            break;
                        case (int)StatusName.AGI:
                            addBasicCritical += (float)(status[i] / 100); //クリティカル倍率を上昇
                            randomAvoid += 30;                            //無効化確率が30%上昇 
                            break;
                        case (int)StatusName.LUK:
                            badStatus = false; //デバフにかからないようにする
                            //自身のLUKの基礎値の半分を他のステータスの基礎値に加算
                            for(int j=(int)StatusName.STR;j<= (int)StatusName.AGI;j++)
                            {
                                originStatus[j] += (originStatus[i] / 2);
                            }
                            break;
                    }
                    //変更後のステータスを適応
                    status[i] = originStatus[i];
                    //効果適応済みにする
                    activeSkill[i] = true;
                }
            }

            //有効中のスキル処理
            if (activeSkill[i])
            {
                //効果時間の計測
                activeSkillTimer[i] -= Time.deltaTime;
                if (activeSkillTimer[i] <= 0.0f)
                {
                    //スキルを解除する
                    activeSkill[i] = false;
                    //スキル解除時に効果のあるスキルの処理
                    //変更されたステータス等を元に戻す
                    switch (i)
                    {
                        case (int)StatusName.STR:
                            originStatus[i] /= 3;       //攻撃力を元に戻す
                            originStatus[(i + 1)] *= 2; //防御力を元に戻す
                            break;
                        case (int)StatusName.DEF:
                            criticalGuard = false;                                         //クリティカルを受けるようにする
                            currentHp += (((beforeHp - currentHp) + originStatus[i] * 3)); //スキル使用後から減った体力+防御力依存の値を回復
                            originStatus[i] /= 2;                                          //防御力を元に戻す
                            break;
                        case (int)StatusName.AGI:
                            addBasicCritical -= (float)(status[i] / 100); //クリティカル倍率を元に戻す
                            randomAvoid -= 30;                            //無効化確率を元に戻す
                            break;
                        case (int)StatusName.LUK:
                            badStatus = true; //デバフにかかるようにする
                            //ステータスを元に戻す
                            for (int j = (int)StatusName.STR; j <= (int)StatusName.AGI; j++)
                            {
                                originStatus[j] -= (originStatus[i] / 2);
                            }
                            break;
                    }
                    //変更後のステータスを適応
                    status[i] = originStatus[i];
                }
            }
            //効果が切れた後のクールタイム処理
            else if (!activeSkill[i] && isUseSkill[i])
            {
                reUseSkillTimer[i] += Time.deltaTime;
                if (reUseSkillTimer[i] >= reUseSkillLimit[i])
                {
                    //タイマーをリセット
                    activeSkillTimer[i] = activeSkillLimit[i];
                    reUseSkillTimer[i] = 0.0f;
                    //クールタイムを終了し、再度使用可能にする
                    isUseSkill[i] = false;
                }
            }
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
            statusChanger = hit.collider.gameObject.GetComponent<StatusChangerManager>();
            //ステータスを変化させる処理
            if (changerLayerName != null && !statusChanger.isActive) 
            {
                useChanger = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    switch (changerLayerName)
                    {
                        case "Attack":
                            addStatus[(int)StatusName.STR] = statusChanger.RandomStatusChange(addStatus[(int)StatusName.STR], (int)StatusName.STR, this.gameObject);
                            break;
                        case "Defence":
                            addStatus[(int)StatusName.DEF] = statusChanger.RandomStatusChange(addStatus[(int)StatusName.DEF], (int)StatusName.DEF, this.gameObject);
                            break;
                        case "Speed":
                            addStatus[(int)StatusName.AGI] = statusChanger.RandomStatusChange(addStatus[(int)StatusName.AGI], (int)StatusName.AGI, this.gameObject);
                            break;
                        case "Luck":
                            addStatus[(int)StatusName.LUK] = statusChanger.RandomStatusChange(addStatus[(int)StatusName.LUK], (int)StatusName.LUK, this.gameObject);
                            break;
                        default:
                            break;
                    }
                    //使用音を鳴らす
                    seManager.seSource.PlayOneShot(useStatusChangerSE);
                }
            }
        }
        else
        {
            changerLayerName = "";
            statusChanger = null;
            useChanger = false;
        }
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
