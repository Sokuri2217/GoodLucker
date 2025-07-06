using UnityEngine;

public class ButtonOptionSelect : ButtonBase
{
    [Header("�ԍ��ݒ�")]
    public int selectCharacter; //�g�p�L����
    public int selectStage;     //�I���X�e�[�W

    [Header("�X�N���v�g�Q��")]
    private UIMenu menu;             //���j���[���
    private GameManager gameManager; //�I����Ԃ�ۑ�

    protected override void Start()
    {
        base.Start();
        //���j���[�����擾
        menu = GameObject.Find("MenuUI").GetComponent<UIMenu>();
        gameManager = GameObject.Find("SelectManager").GetComponent<GameManager>();
    }

    //�L�����N�^�[�I�����
    public void CharacterSelect()
    {
        menu.selectPanel[0].SetActive(true);

    }

    //�X�e�[�W�I�����
    public void StageSelect()
    {
        menu.selectPanel[1].SetActive(true);
    }

    //�p�l�����\���ɂ���
    public void CloseSelect()
    {
        for (int i = 0; i < 2; i++)
        {
            //�\�����̃p�l��������Δ�\���ɂ���
            if (menu.selectPanel[i].activeSelf)
            {
                menu.selectPanel[i].SetActive(false);
            }
        }
        menu.closeSelectPanel.SetActive(false);
        menu.explanationWindow.SetActive(false);

    }
    //�Z���N�g�p�l���ɋ��ʂ���I�u�W�F�N�g��\��
    public void OpenSelectMenu()
    {
        menu.closeSelectPanel.SetActive(true);
        menu.explanationWindow.SetActive(true);
    }

    //�g�p�L�����ݒ�
    public void SetCharacter()
    {
        gameManager.selectCharacter = selectCharacter;
    }

    //�I���X�e�[�W�ݒ�
    public void SetStage()
    {
        gameManager.selectStage = selectStage;
    }
}
