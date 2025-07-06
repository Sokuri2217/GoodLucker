using UnityEngine;

public class ButtonSound : ButtonBase
{
    [Header("���ʒ����p�l��")]
    public GameObject sound;

    protected override void Start()
    {
        base.Start();

        //�����ݒ�
        //�p�l�����\��
        sound.SetActive(false);
    }

    //�p�l����\��
    public void OpenSoundPanel()
    {
        sound.SetActive(true);
    }

    //�p�l�����\��
    public void CloseSoundPanel()
    {
        sound.SetActive(false);
    }
}
