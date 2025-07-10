using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStage : UIBase
{
    [Header("���݂̃V�[����")]
    public string currentSceneName;
    [Header("�Q�[�����")]
    public bool isStop;
    [Header("�p�l��")]
    public GameObject gameStopPanel; //�ꎞ��~
    public GameObject reallyPanel;   //�ŏI�m�F
    [Header("���E���R(���^�C�A�E���g���C)")]
    public bool[] exit = new bool[2];

    //�������h�~�p
    private bool isInput; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //�S�Ẵp�l�����\��
        gameStopPanel.SetActive(false);
        reallyPanel.SetActive(false);
        //���݂̃V�[�������擾
        currentSceneName = SceneManager.GetActiveScene().name;
        // �}�E�X�J�[�\������ʒ����ɌŒ�
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //Escape���������Ƃ�
        if (Input.GetKeyDown(KeyCode.Escape) && !isInput)  
        {
            isInput = true;
            //�m�F��ʂ��o�Ă���Ƃ��͓������Ȃ�
            if (!reallyPanel.activeSelf) 
            {
                CheckGameState();
            }
        }

        //�ē��͏o����悤�ɂ���
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            isInput = false;
        }
    }

    //�Q�[���̏�Ԃ��`�F�b�N
    public void CheckGameState()
    {
        switch (isStop)
        {
            //��~��
            case true:
                isStop = false;
                gameStopPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked; // �}�E�X�J�[�\������ʒ����ɌŒ�
                //���Ԃ�ʏ�ɖ߂�
                Time.timeScale = 1.0f;
                break;
            //�v���C��
            case false:
                isStop = true;
                gameStopPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None; // �}�E�X�J�[�\���̌Œ���O��
                //���Ԃ��~�߂�
                Time.timeScale = 0.0f;
                break;
        }
    }
}
