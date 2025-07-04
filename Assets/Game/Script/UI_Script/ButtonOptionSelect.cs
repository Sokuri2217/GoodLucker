using UnityEngine;

public class ButtonOptionSelect : ButtonBase
{
    [Header("スクリプト参照")]
    private UIMenu menu;   //メニュー画面

    protected override void Start()
    {
        base.Start();
        //メニュー情報を取得
        menu = GameObject.Find("MenuUI").GetComponent<UIMenu>();
    }

    //キャラクター選択画面
    public void CharacterSelect()
    {
        menu.selectPanel[0].SetActive(true);

    }

    //ステージ選択画面
    public void StageSelect()
    {
        menu.selectPanel[1].SetActive(true);
    }

    //パネルを非表示にする
    public void CloseSelect()
    {
        for (int i = 0; i < 2; i++)
        {
            //表示中のパネルがあれば非表示にする
            if (menu.selectPanel[i].activeSelf)
            {
                menu.selectPanel[i].SetActive(false);
            }
        }
        menu.closeSelectPanel.SetActive(false);

    }
    //セレクトパネルを閉じるボタンを表示
    public void OpenButton()
    {
        menu.closeSelectPanel.SetActive(true);
    }
}
