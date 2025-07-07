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
    [Header("説明欄")]
    public GameObject explanationWindow; //枠
    public Image[] statusBar;            //ステータスを棒の長短で表現(STR,DEF,AGI,LUK)
    [Header("ボタン")]
    public GameObject closeSelectPanel;
    [Header("スクリプト参照")]
    public ButtonScene buttonScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //コンポーネント取得
        selectedCharacter = GameObject.Find("SelectedCharacter").GetComponent<Image>();
        selectedStage = GameObject.Find("SelectedStage").GetComponent<Image>();
        //スクリプト取得
        buttonScene = GameObject.Find("StartButton").GetComponent<ButtonScene>();
        //セレクトパネルを非表示
        for (int i = 0; i < 2; i++) 
        {
            selectPanel[i].SetActive(false);
        }
        //セレクトパネルを閉じるボタンを非表示
        closeSelectPanel.SetActive(false);
        //選択内容の説明欄を非表示
        explanationWindow.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //選択状態を可視化
        ChangeSprite();
        //シーン設定
        SceneSetting();
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
}
