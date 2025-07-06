using UnityEditor;
using UnityEngine;

public class ButtonStage : MonoBehaviour
{
    [Header("�X�N���v�g�Q��")]
    public UIStage stage;           //�X�e�[�WUI
    public GameManager gameManager; //�I����Ԃ�ۑ�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        //���j���[�����擾
        stage = GameObject.Find("StageUI").GetComponent<UIStage>();
        gameManager = GameObject.Find("SelectManager").GetComponent<GameManager>();
    }

    //���݂̃X�e�[�W���痣���
    public void ExitStage()
    {
        //�m�F��ʂ�\��
        stage.reallyPanel.SetActive(true);
        //��~��ʂ��\��
        stage.gameStopPanel.SetActive(false);
    }
    //���E���R(���^�C�A)
    public void Retire()
    {
        stage.exit[0] = true;
    }
    //���E���R(���^�C�A)
    public void Retry()
    {
        stage.exit[1] = true;
    }
    //������
    public void NotExit()
    {
        //���E���R�����Z�b�g
        for (int i = 0; i < 2; i++)
            stage.exit[i] = false;
        //��~��ʂ�\��
        stage.gameStopPanel.SetActive(true);
        //�m�F��ʂ��\��
        stage.reallyPanel.SetActive(false);
    }
    //���s
    public void YesExit()
    {
        //���Ԃ�ʏ�ɖ߂�
        Time.timeScale = 1.0f;
        //�V�[���ړ��p�{�^���X�N���v�g���擾
        ButtonScene buttonScene = GetComponent<ButtonScene>();
        //���^�C�A�̏ꍇ
        if (stage.exit[0])
        {
            buttonScene.sceneName = "Menu";
        }
        //���g���C�̏ꍇ
        else if(stage.exit[1])
        {
            //���݂̃V�[�����ēǂݍ���
            buttonScene.sceneName = stage.currentSceneName;
        }
    }
}
