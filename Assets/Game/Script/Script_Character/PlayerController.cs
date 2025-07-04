using UnityEngine;

public class PlayerController : CharacterBase
{
    [Header("ステータス")]
    public float dashSpeed;  //ダッシュ倍率
    public float upperForce; //ジャンプ力

    [Header("フラグ")]
    public bool isJump;      //ジャンプ中フラグ
    public bool onGround;    //設置フラグ

    [Header("コンポーネント参照")]
    public Transform cameraTransform;          //カメラのTransform

    [Header("スクリプト参照")]
    private CameraController cameraController; //カメラ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //カメラのスクリプトを取得
        cameraController=GameObject.Find("Main Camera").GetComponent<CameraController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //移動
        {
            Move3D(addSpeed); //水平方向
            Jump3D();         //ジャンプ
        }
    }

    public void Move3D(float add)
    {
        //入力(WASD)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //ダッシュ入力(左Shift)
        dashSpeed = Input.GetKey(KeyCode.LeftShift) ? 1.2f : 1.0f;

        //カメラの向きに合わせて、ベクトルを設定
        Vector3 vec = cameraController.transform.forward * vertical + cameraController.transform.right * horizontal;
        transform.position += vec * speed * dashSpeed * add * Time.deltaTime;
    }

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
}
