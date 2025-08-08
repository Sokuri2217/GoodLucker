using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : UIBase
{
    [Header("ステージ名")]
    public string[] sceneName;
    [Header("選択中")]
    public Image selectedCharacter;    //キャラクター
    public Image selectedStage;        //ステージ
    public Sprite[] characterIcon;     //キャラアイコン
    public Sprite[] stageIcon;         //ステージアイコン
    public Image explanationCharacter; //キャラクター説明
    public Image explanationStage;     //ステージ説明
    public Sprite[] characterExpla;    //キャラSprite
    public Sprite[] stageExpla;        //ステージSprite
    [Header("セレクトパネル")]
    public GameObject[] selectPanel = new GameObject[2];
    [Header("オプションパネル")]
    public GameObject optionPanel;      //本体
    public GameObject systemPanel;      //ゲームシステム
    public GameObject controlPanel;     //操作方法
    public GameObject soundPanel;       //音量
    public GameObject currentOpenPanel; //表示中のパネル
    [Header("説明欄")]
    public GameObject explanationWindow; //枠
    public Image[] statusBar;            //ステータスを棒の長短で表現(STR,DEF,AGI,LUK)
    [Header("ボタン")]
    public GameObject closeSelectPanel;
    [Header("スクリプト参照")]
    public ButtonScene buttonScene;
    public StatusSetting statusSetting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //コンポーネント取得
        selectedCharacter = GameObject.Find("SelectedCharacter").GetComponent<Image>();
        selectedStage = GameObject.Find("SelectedStage").GetComponent<Image>();
        //スクリプト取得
        buttonScene = GameObject.Find("StartButton").GetComponent<ButtonScene>();
        statusSetting = GameObject.Find("Warrior").GetComponent<StatusSetting>();
        //セレクトパネルを非表示
        for (int i = 0; i < 2; i++) 
        {
            selectPanel[i].SetActive(false);
        }
        //セレクトパネルを閉じるボタンを非表示
        closeSelectPanel.SetActive(false);
        //音量調整パネルを非表示
        soundPanel.SetActive(false);
        //オプション関連を非表示
        optionPanel.SetActive(false);
        systemPanel.SetActive(false);
        controlPanel.SetActive(false);
        //選択内容の説明欄を非表示
        explanationWindow.SetActive(false);
        //ステータスをWarriorに設定
        for (int i = 0; i < 4; i++) 
        {
            gameManager.status[i] = statusSetting.status[i];
        }
    }

    // Update is called once per frame
    public void Update()
    {
        //選択状態を可視化
        ChangeSprite();
        //シーン設定
        SceneSetting();
        //オプション画面を表示
        OpenOption();
        //オプション画面を非表示
        CloseOption();
    }

    //選択状態を可視化
    void ChangeSprite()
    {
        //GamaManagerから選択状態を取得しSpriteに反映
        selectedCharacter.sprite = characterIcon[(gameManager.selectCharacter)];
        selectedStage.sprite = stageIcon[(gameManager.selectStage)];
    }
    //シーン設定
    void SceneSetting()
    {
        buttonScene.sceneName = sceneName[(gameManager.selectStage)];
    }
    //オプション画面を表示
    void OpenOption()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentOpenPanel == optionPanel ||
                currentOpenPanel == null)  
            {
                if (!optionPanel.activeSelf)
                {
                    //非表示中なら開く
                    optionPanel.SetActive(true);
                    currentOpenPanel = optionPanel;
                }
                else
                {
                    //表示中なら閉じる
                    optionPanel.SetActive(false);
                    currentOpenPanel = null;
                }
            }
        }
    }

    //オプション画面を非表示
    void CloseOption()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentOpenPanel != null && currentOpenPanel != optionPanel)
            {
                //表示中のパネルを非表示にし、オプション画面を開く
                currentOpenPanel.SetActive(false);
                optionPanel.SetActive(true);
                //表示中パネルの設定
                currentOpenPanel = optionPanel;
            }
            else if (currentOpenPanel == optionPanel)
            {
                optionPanel.SetActive(false);
                currentOpenPanel = null;
            }
        }
    }

}
