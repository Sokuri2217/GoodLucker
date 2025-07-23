using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStage : UIBase
{
    [Header("現在のシーン名")]
    public string currentSceneName;
    [Header("ゲーム状態")]
    public bool isStop;
    [Header("パネル")]
    public GameObject mainPanel;     //基本UI
    public GameObject gameStopPanel; //一時停止
    public GameObject reallyPanel;   //最終確認
    public GameObject clearPanel;   //ゲームクリア
    public GameObject overPanel;   //ゲームオーバー
    [Header("GUI")]
    public GameObject[] status = new GameObject[4];   //ステータス変化の制限時間
    public GameObject changeInput;                    //入力キー
    [Header("離脱理由(リタイア・リトライ)")]
    public bool[] exit = new bool[2];
    [Header("オブジェクト生成上限")]
    public int[] createLimit=new int[4]; //StatusChanger(STR,DEF,AGI,LUK)
    public int[] createCount=new int[4]; //集計用
    [Header("コンポーネント参照")]
    public Image hp;                        //体力ゲージ
    public Image[] change = new Image[4];             //ステータスの変化状態
    public Sprite[] upDown = new Sprite[3]; //増減
    [Header("スクリプト参照")]
    public PlayerController playerController;
    [Header("ゲーム状態")]
    public bool isGame;    //プレイ可能
    public bool gameClear; //ゲームクリア
    public bool gameOver;  //ゲームオーバー
    [Header("クリア条件")]
    public int clearkillCount; //クリアに必要なボスの討伐数
    public int killBossCount;  //倒したボスの数

    //長押し防止用
    private bool isInput; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //オブジェクトを非表示
        for (int i = 0; i < 4; i++) 
        {
            status[i].SetActive(false);
        }
        //パネルを非表示
        gameStopPanel.SetActive(false);
        reallyPanel.SetActive(false);
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        //現在のシーン名を取得
        currentSceneName = SceneManager.GetActiveScene().name;
        // マウスカーソルを画面中央に固定
        Cursor.lockState = CursorLockMode.Locked;
        //ゲーム状態の設定
        isGame = true;
        gameClear = false;
        gameOver = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //スクリプト取得
        if (playerController == null)
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        //HPゲージ
        {
            CheckHpState();
        }
        //ステータス
        {
            CheckStatusState();
        }
        //Escapeを押したとき
        {
            if (!gameClear && !gameOver) 
            {
                if (Input.GetKeyDown(KeyCode.Escape) && !isInput)
                {
                    isInput = true;
                    //確認画面が出ているときは動かさない
                    if (!reallyPanel.activeSelf)
                    {
                        CheckGameState();
                    }
                }

                //再入力出来るようにする
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    isInput = false;
                }
            }
        }
        //プレイ結果
        {
            //ゲームクリア条件
            if(killBossCount>=clearkillCount)
            {
                gameClear = true;
            }
            //ゲームオーバー条件はPlayerControllerに記入

            if(isGame)
            {
                CheckResultState();
            }
        }
       
    }

    //HP
    public void CheckHpState()
    {
        //プレイヤーの残り体力に応じて描画
        hp.fillAmount = (float)playerController.currentHp / (float)playerController.maxHp;
        //残り割合に応じて、色を変化
        Color color = hp.color;
        //赤
        if (hp.fillAmount <= 0.2f)
        {
            color = new Color32(255, 0, 0, 255);
        }
        //黄
        else if (hp.fillAmount <= 0.6f)
        {
            color = new Color32(255, 255, 0, 255);
        }
        //白
        else
        {
            color = new Color32(229, 229, 229, 255);
        }
        hp.color = color;
    }

    //ステータス
    public void CheckStatusState()
    {
        for (int i = 0; i < 4; i++) 
        {
            if (playerController.addStatus[i] > 1.0f || playerController.addStatus[i] < 1.0f)
            {
                //対象のオブジェクトを表示
                status[i].SetActive(true);
                //コンポーネントを取得
                Image statusImage = status[i].GetComponent<Image>();
                //制限時間に応じて、ゲージを描画
                statusImage.fillAmount = playerController.addStatusTimer[i] / playerController.addStatusLimit[i];
                //時間切れでオブジェクトを非表示
                if (statusImage.fillAmount <= 0.001f)
                {
                    status[i].SetActive(false);
                }

                //増減アイコンを表示
                if(playerController.addStatus[i] < 1.0f)
                {
                    change[i].sprite = upDown[1];
                }
                else if (playerController.addStatus[i] > 1.0f)
                {
                    change[i].sprite = upDown[0];
                }
                else
                {
                    change[i].sprite = upDown[2];
                }
            }
        }

        //入力キーの表示
        changeInput.SetActive(playerController.useChanger);
    }

    //ゲームの状態をチェック
    public void CheckGameState()
    {
        switch (isStop)
        {
            //停止中
            case true:
                isStop = false;
                //表示パネルの切り替え
                gameStopPanel.SetActive(false);
                mainPanel.SetActive(true);
                // マウスカーソルを画面中央に固定
                Cursor.lockState = CursorLockMode.Locked; 
                //時間を通常に戻す
                Time.timeScale = 1.0f;
                //ゲーム状態をプレイ可能にする
                isGame = true;
                break;
            //プレイ中
            case false:
                isStop = true;
                //ゲーム状態をプレイ不可にする
                isGame = false;
                //表示パネルの切り替え
                mainPanel.SetActive(false);
                gameStopPanel.SetActive(true);
                // マウスカーソルの固定を外す
                Cursor.lockState = CursorLockMode.None;
                //時間を止める
                Time.timeScale = 0.0f;
                break;
        }
    }
    //死亡処理
    public void GameOver()
    {
        gameOver = true;
    }

    //プレイ結果
    public void CheckResultState()
    {
        //体力が0以下になったらゲームオーバー
        if (gameOver)  
        {
            isGame = false;
            overPanel.SetActive(true);
            mainPanel.SetActive(false);
            // マウスカーソルの固定を外す
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.0f;
        }
        //ステージ上の全てのボスを倒すとクリア
        else if (gameClear)
        {
            isGame = false;
            clearPanel.SetActive(true);
            mainPanel.SetActive(false);
            //マウスカーソルの固定を外す
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.0f;
        }
    }
}
