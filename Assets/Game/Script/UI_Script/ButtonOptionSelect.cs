using UnityEngine;

public class ButtonOptionSelect : ButtonBase
{
    [Header("�X�N���v�g�Q��")]
    private UIMenu menu;   //���j���[���

    protected override void Start()
    {
        base.Start();
        //���j���[�����擾
        menu = GameObject.Find("MenuUI").GetComponent<UIMenu>();
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

    }
    //�Z���N�g�p�l�������{�^����\��
    public void OpenButton()
    {
        menu.closeSelectPanel.SetActive(true);
    }
}
