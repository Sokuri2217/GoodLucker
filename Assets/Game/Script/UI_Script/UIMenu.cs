using UnityEngine;
using UnityEngine.UI;

public class UIMenu : UIBase
{
    [Header("�Z���N�g�p�l��")]
    public GameObject[] selectPanel = new GameObject[2];
    [Header("�{�^��")]
    public GameObject closeSelectPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //�Z���N�g�p�l�����\��
        for(int i=0;i<2;i++)
        {
            selectPanel[i].SetActive(false);
        }
        //�Z���N�g�p�l�������{�^�����\��
        closeSelectPanel.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
