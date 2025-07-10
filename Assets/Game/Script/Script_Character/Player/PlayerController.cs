using UnityEngine;

public class PlayerController : CharacterBase
{
    [Header("ステータス")]
    public float dashSpeed;         //ダッシュ倍率
    public float upperForce;        //ジャンプ力
    public float baAtIntervalLimit; //通常攻撃インターバル
    public float spSkIntervalLimit; //特殊攻撃インターバル
    public float baAtIntervalTimer; //通常計測用
    public float spSkIntervalTimer; //特殊計測用

    [Header("フラグ")]
    public bool isJump;                      //ジャンプ中
    public bool onGround;                    //接地
    public bool basicAttack;                 //通常攻撃
    public bool spSkill;                     //特殊攻撃
    public bool[] attackInput = new bool[2]; //各攻撃入力中

    [Header("カメラ参照")]
    public string cameraName;
    public Transform cameraTransform;          //カメラのTransform
    [Header("回転")]
    public float rotationSpeed; //速度

    [Header("スクリプト参照")]
    protected CameraController cameraController; //カメラ
    protected GameManager gameManager;

    protected enum StatusName
    {
        STR,
        DEF,
        AGI,
        LUK,
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //ステータス設定
        //体力
        currentHp = maxHp;
        //その他
        gameManager = GameObject.Find("SelectManager").GetComponent<GameManager>();
        for (int i = 0; i < ((int)StatusName.LUK + 1); i++) 
        {
            status[i] = gameManager.status[i];
            //移動速度
            if (i == (int)StatusName.AGI) 
                agent.speed = status[i];
        }
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
                Move3D(addSpeed); //水平方向
                Jump3D();         //ジャンプ
            }
            
        }
        //攻撃
        {
            UseBasicAttack();  //通常
            UseSpSkill();      //特殊
        }
    }

    //水平方向の移動
    public void Move3D(float add)
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
            transform.position += setVec * status[(int)StatusName.AGI] / 5 * dashSpeed * add * Time.deltaTime;

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
            if (baAtIntervalTimer >= baAtIntervalLimit) 
            {
                if(!Input.GetMouseButtonDown(0))
                {
                    basicAttack = false;
                    baAtIntervalTimer = 0;
                }
            }
            else
            {
                //次の攻撃までのインターバルを計測
                baAtIntervalTimer += Time.deltaTime;
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
                if (spSkIntervalTimer >= spSkIntervalLimit)
                {
                    if (!Input.GetKeyDown(KeyCode.R))
                    {
                        spSkill = false;
                        spSkIntervalTimer = 0;
                    }
                }
                else
                {
                    //次の攻撃までのインターバルを計測
                    spSkIntervalTimer += Time.deltaTime;
                }

                //使用中にもう一度入力すると中止
                if (Input.GetKeyDown(KeyCode.R))
                {
                    spSkill = false;
                    spSkIntervalTimer = 0;
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
}
