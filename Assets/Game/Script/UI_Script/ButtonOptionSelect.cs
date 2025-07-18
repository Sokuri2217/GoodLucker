using UnityEngine;

public class ButtonOptionSelect : ButtonBase
{
    [Header("番号設定")]
    public int selectCharacter; //使用キャラ
    public int selectStage;     //選択ステージ

    [Header("スクリプト参照")]
    private UIMenu menu;                 //メニュー画面
    private GameManager gameManager;     //選択状態を保存

    protected override void Start()
    {
        base.Start();
        //メニュー情報を取得
        menu = GameObject.Find("MenuUI").GetComponent<UIMenu>();
        gameManager = GameObject.Find("SelectManager").GetComponent<GameManager>();
    }

    public void Update()
    {
        if (isZoom && (selectCharacter >= 0 || selectStage >= 0))  
        {
            //キャラクター選択
            if (selectCharacter >= 0)
            {
                //カーソルが重なった時に説明欄を表示する
                menu.explanationCharacter.sprite = menu.characterExpla[(selectCharacter)];
                ChangeStatusSprite();
            }
            //ステージ選択
            else if (selectStage >= 0) 
            {
                //カーソルが重なった時に説明欄を表示する
                menu.explanationStage.sprite = menu.stageExpla[(selectStage)];
            }
        }
    }

    public void ChangeStatusSprite()
    {
        StatusSetting statusSetting = GetComponent<StatusSetting>();
        //ボタンを押したとき
        if (click)
        {
            //ステータス設定
            for (int i = 0; i < 4; i++)
            {
                gameManager.status[i] = statusSetting.status[i];
            }
            click = false;
        }
        else
        {
            //ステータスバー反映
            for (int i = 0; i < 4; i++)
            {
                menu.statusBar[i].fillAmount = (float)statusSetting.status[i] / (float)gameManager.maxStatus;
            }
        }
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

    //システムパネルの表示
    public void OpenSystem()
    {
        //直前に表示しているパネルを非表示にし、新しいパネルを表示
        menu.systemPanel.SetActive(true);
        menu.currentOpenPanel.SetActive(false);
        //表示中パネルの設定
        menu.currentOpenPanel = menu.systemPanel;
    }

    //音量パネルの表示
    public void OpenSound()
    {
        //直前に表示しているパネルを非表示にし、新しいパネルを表示
        menu.soundPanel.SetActive(true);
        menu.currentOpenPanel.SetActive(false);
        //表示中パネルの設定
        menu.currentOpenPanel = menu.soundPanel;
    }

    //操作説明パネルを表示
    public void OpenControl()
    {
        //直前に表示しているパネルを非表示にし、新しいパネルを表示
        menu.controlPanel.SetActive(true);
        menu.currentOpenPanel.SetActive(false);
        //表示中パネルの設定
        menu.currentOpenPanel = menu.controlPanel;
    }
}
