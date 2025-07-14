using UnityEngine;

public class PlayerController : CharacterBase
{
    [Header("ステータス")]
    public float dashSpeed;         //ダッシュ倍率
    public float upperForce;        //ジャンプ力
    public float[] IntervalLimit = new float[2]; //各攻撃のクールダウン(通常・特殊)
    public float[] IntervalTimer = new float[2]; //計測用(通常・特殊)

    [Header("アイテム取得")]
    public string changerLayerName;                    //Layer名
    public float rayRange;                             //Rayの距離
    public LayerMask layerMasks;  //レイヤー指定

    [Header("フラグ")]
    public bool isJump;                      //ジャンプ中
    public bool onGround;                    //接地
    public bool basicAttack;                 //通常攻撃
    public bool spSkill;                     //特殊攻撃
    public bool[] attackInput = new bool[2]; //各攻撃入力中

    [Header("カメラ参照")]
    public string cameraName;         //参照先の名前
    public Transform cameraTransform; //カメラのTransform

    [Header("回転")]
    public float rotationSpeed; //速度

    [Header("スクリプト参照")]
    protected CameraController cameraController; //カメラ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //カメラの情報を取得
        GameObject cameraObj = GameObject.Find(cameraName);
        cameraTransform = cameraObj.GetComponent<Transform>();
        cameraController = cameraObj.GetComponent<CameraController>();
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
                //ジャンプ
                Jump3D();
            }
            
        }
        //ステータス
        {
            GetStatusChanger();
        }
        //攻撃
        {
            UseBasicAttack();  //通常
            UseSpSkill();      //特殊
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
    public void Jump3D()
    {
        //Spaceを押したときにジャンプする
        if (Input.GetKeyDown(KeyCode.Space) && !isJump) 
        {
            isJump = true;
            agent.enabled = false;
            rb.AddForce(Vector3.up * upperForce, ForceMode.Impulse);
        }
    }

    //通常攻撃
    protected virtual void UseBasicAttack()
    {
        if (!basicAttack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                basicAttack = true;
            }
        }
        else
        {
            //インターバル設定
            if (IntervalTimer[0] >= IntervalLimit[0]) 
            {
                if(!Input.GetMouseButtonDown(0))
                {
                    basicAttack = false;
                    IntervalTimer[0] = 0;
                }
            }
            else
            {
                //次の攻撃までのインターバルを計測
                IntervalTimer[0] += Time.deltaTime;
            }
        }
    }

    //特殊攻撃使用
    protected virtual void UseSpSkill()
    {
        if (!attackInput[1])
        {
            if (!spSkill)
            {
                //特殊攻撃使用
                if (Input.GetKeyDown(KeyCode.R))
                {
                    spSkill = true;
                    attackInput[1] = true;
                }
            }
            else
            {
                //インターバル設定
                if (IntervalTimer[1] >= IntervalLimit[1])
                {
                    if (!Input.GetKeyDown(KeyCode.R))
                    {
                        spSkill = false;
                        IntervalTimer[1] = 0;
                    }
                }
                else
                {
                    //次の攻撃までのインターバルを計測
                    IntervalTimer[1] += Time.deltaTime;
                }

                //使用中にもう一度入力すると中止
                if (Input.GetKeyDown(KeyCode.R))
                {
                    spSkill = false;
                    IntervalTimer[1] = 0;
                    attackInput[1] = true;
                }
            }
        }
        //再入力可能
        if (Input.GetKeyUp(KeyCode.R))
        {
            attackInput[1] = false;
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
            // Ray が何にも当たっていない or 離れた
            if (!string.IsNullOrEmpty(changerLayerName))
            {
                changerLayerName = "";

            }
        }
    }
}
