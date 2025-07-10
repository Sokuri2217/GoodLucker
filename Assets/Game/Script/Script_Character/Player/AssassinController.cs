using JetBrains.Annotations;
using UnityEngine;

public class AssassinController : PlayerController
{
    [Header("コンポーネント参照")]
    private Transform topViewPos; //見下ろすカメラ

    [Header("カメラ参照")]
    public GameObject topViewCamera; //見下ろすカメラ
    public GameObject tpsCamera;     //通常視点カメラ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //コンポーネント取得
        topViewCamera = GameObject.Find("TopViewCamera");
        tpsCamera = GameObject.Find("Camera");

        //最初はFPS視点でスタート
        topViewCamera.SetActive(false);
        topViewPos = topViewCamera.transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void UseBasicAttack()
    {
        base.UseBasicAttack();
    }

    protected override void UseSpSkill()
    {
        base.UseSpSkill();

        if(spSkill)
        {
            tpsCamera.SetActive(false);
            topViewCamera.SetActive(true);

            //入力(WASD)
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            //カメラの向きに合わせて、ベクトルを設定
            Vector3 vec = transform.forward * vertical + transform.right * horizontal;
            topViewCamera.transform.position += vec * 20 * dashSpeed * Time.deltaTime;
        }
        else
        {
            topViewPos.position = new Vector3(this.transform.position.x, topViewPos.position.y, this.transform.position.z);

            topViewCamera.SetActive(false);
            tpsCamera.SetActive(true);
        }
    }
}
