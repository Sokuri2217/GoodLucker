using UnityEngine;
using UnityEngine.UI;

public class UIMenu : UIBase
{
    [Header("セレクトパネル")]
    public GameObject[] selectPanel = new GameObject[2];
    [Header("ボタン")]
    public GameObject closeSelectPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //セレクトパネルを非表示
        for(int i=0;i<2;i++)
        {
            selectPanel[i].SetActive(false);
        }
        //セレクトパネルを閉じるボタンを非表示
        closeSelectPanel.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
