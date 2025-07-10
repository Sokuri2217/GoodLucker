using JetBrains.Annotations;
using UnityEngine;

public class AssassinController : PlayerController
{
    [Header("�R���|�[�l���g�Q��")]
    private Transform topViewPos; //�����낷�J����

    [Header("�J�����Q��")]
    public GameObject topViewCamera; //�����낷�J����
    public GameObject tpsCamera;     //�ʏ펋�_�J����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //�R���|�[�l���g�擾
        topViewCamera = GameObject.Find("TopViewCamera");
        tpsCamera = GameObject.Find("Camera");

        //�ŏ���FPS���_�ŃX�^�[�g
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

            //����(WASD)
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            //�J�����̌����ɍ��킹�āA�x�N�g����ݒ�
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
