using UnityEngine;

public class ButtonOptionSelect : ButtonBase
{
    [Header("番号設定")]
    public int selectCharacter; //使用キャラ
    public int selectStage;     //選択ステージ

    [Header("スクリプト参照")]
    private UIMenu menu;             //メニュー画面
    private GameManager gameManager; //選択状態を保存

    protected override void Start()
    {
        base.Start();
        //メニュー情報を取得
        menu = GameObject.Find("MenuUI").GetComponent<UIMenu>();
        gameManager = GameObject.Find("SelectManager").GetComponent<GameManager>();
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
        menu.explanationWindow.SetActive(false);

    }
    //セレクトパネルに共通するオブジェクトを表示
    public void OpenSelectMenu()
    {
        menu.closeSelectPanel.SetActive(true);
        menu.explanationWindow.SetActive(true);
    }

    //使用キャラ設定
    public void SetCharacter()
    {
        gameManager.selectCharacter = selectCharacter;
    }

    //選択ステージ設定
    public void SetStage()
    {
        gameManager.selectStage = selectStage;
    }
}
