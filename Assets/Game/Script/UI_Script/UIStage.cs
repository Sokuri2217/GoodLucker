using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStage : UIBase
{
    [Header("現在のシーン名")]
    public string currentSceneName;
    [Header("ゲーム状態")]
    public bool isStop;
    [Header("スキル選択")]
    public bool isSelectSkillInput;                    //スキル選択画面用
    public int currentSelectSkill;                     //選択中のスキル番号
    public int skillNum;                               //スキルの種類数
    public float slideSkillSpeed;                      //選択中スキル画像をスライドさせる速度
    public float slideSkillLimit;                      //選択中スキル画像をスライドさせる距離
    private Vector3[] originSkillPos = new Vector3[4];  //選択中スキル画像の初期座標
    private Vector3[] currentSkillPos = new Vector3[4]; //選択中スキル画像の現在の座標
    [Header("パネル")]
    public GameObject mainPanel;     //基本UI
    public GameObject spSkillPanel;  //特殊攻撃UI
    public GameObject gameStopPanel; //一時停止
    public GameObject reallyPanel;   //最終確認
    public GameObject clearPanel;    //ゲームクリア
    public GameObject overPanel;     //ゲームオーバー
    [Header("GUI")]
    public GameObject[] skill = new GameObject[4];      //各スキル選択
    public GameObject[] explaSkill = new GameObject[4]; //各スキル説明
    public GameObject[] skillTimer = new GameObject[4]; //各スキルのタイマー
    public GameObject[] status = new GameObject[4];     //ステータス変化の制限時間
    public GameObject changeInput;                      //入力キー
    [Header("離脱理由(リタイア・リトライ)")]
    public bool[] exit = new bool[2];
    [Header("オブジェクト生成上限")]
    public int[] createLimit=new int[4]; //StatusChanger(STR,DEF,AGI,LUK)
    public int[] createCount=new int[4]; //集計用
    [Header("コンポーネント参照")]
    public Image hp;                        //体力ゲージ
    public Image[] bossHp;                  //ボスの体力ゲージ
    public Image[] change = new Image[4];   //ステータスの変化状態
    public Sprite[] upDown = new Sprite[3]; //増減
    public AudioClip selectSkillSE;         //スキル選択SE
    public AudioClip clearBGM;              //クリアBGM
    public AudioClip overBGM;               //ゲームオーバーBGM
    [Header("スクリプト参照")]
    protected PlayerController playerController;
    protected BossController bossController;
    protected SEManager seManager;
    [Header("ゲーム状態")]
    public bool isGame;    //プレイ可能
    public bool gameClear; //ゲームクリア
    public bool gameOver;  //ゲームオーバー
    [Header("クリア条件")]
    public int clearkillCount; //クリアに必要なボスの討伐数
    public int killBossCount;  //倒したボスの数
    public string bossName;    //ボスの名前

    //長押し防止用
    private bool isInput; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //スクリプト取得
        bossController = GameObject.Find(bossName).GetComponent<BossController>();
        seManager = GameObject.Find("SEManager").GetComponent<SEManager>();
        //ゲーム状態の設定
        gameClear = false;
        gameOver = false;
        isGame = true;
        for (int i = 0; i < 4; i++) 
        {
            //オブジェクトを非表示
            status[i].SetActive(false);
            skillTimer[i].SetActive(false);
            //選択中スキル画像の初期座標を設定
            Transform skillImagePos = skill[i].transform;
            originSkillPos[i] = skillImagePos.position;
            currentSkillPos[i] = originSkillPos[i];
        }
        //初期スキルを設定
        currentSelectSkill = 0;
        //パネルを非表示
        spSkillPanel.SetActive(false);
        gameStopPanel.SetActive(false);
        reallyPanel.SetActive(false);
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        //現在のシーン名を取得
        currentSceneName = SceneManager.GetActiveScene().name;
        // マウスカーソルを画面中央に固定
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    public void Update()
    {
        if (gameClear || gameOver) return;

        //HPゲージ
        {
            CheckHpState();
        }
        //ステータス
        {
            CheckStatusState();
        }
        //決着がついていない状態だけ処理する
        if (!gameClear && !gameOver)
        {
            //Escapeを押したとき＆スキル選択中じゃないとき
            if (Input.GetKeyDown(KeyCode.Escape) && !playerController.spSkill && !isInput) 
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

            //スキルUIの表示
            spSkillPanel.SetActive(playerController.spSkill);
            //選択処理
            SelectSpSkill();
            //スキルGUI
            SkillTimerUI();

        }
        //プレイ結果
        {
            //ゲームクリア条件
            if (killBossCount >= clearkillCount && !gameClear)
            {
                gameClear = true;
            }
            //ゲームオーバー条件
            if (playerController.currentHp <= 0 && !gameOver)
            {
                gameOver = true;
            }

            //各結果パネル表示
            if (isGame)
            {
                CheckResultState();
            }
        }
    }

    //HP
    public void CheckHpState()
    {
        //プレイヤーの残り体力に応じて描画
        {
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
        //ボスの残り体力に応じて描画
        {
            for (int i = 0; i < clearkillCount; i++) 
            {
                bossHp[i].fillAmount = (float)bossController.currentHp / (float)bossController.maxHp;
                //残り割合に応じて、色を変化
                Color color = bossHp[i].color;
                //赤
                if (bossHp[i].fillAmount <= 0.2f)
                {
                    color = new Color32(255, 0, 0, 255);
                }
                //黄
                else if (bossHp[i].fillAmount <= 0.6f)
                {
                    color = new Color32(255, 255, 0, 255);
                }
                //白
                else
                {
                    color = new Color32(229, 229, 229, 255);
                }
                bossHp[i].color = color;
            }
        }
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

    //スキル選択
    public void SelectSpSkill()
    {
        //選択処理
        if (playerController.spSkill) 
        {
            Time.timeScale = 0.0f;
            //入力
            if (Input.GetKeyDown(KeyCode.W) && !isSelectSkillInput)
            {
                isSelectSkillInput = true;
                currentSelectSkill--;
                //選択がループするようにする
                if (currentSelectSkill < 0)
                {
                    currentSelectSkill = (skillNum - 1);
                }
                //選択音を鳴らす
                seManager.seSource.PlayOneShot(selectSkillSE);
            }
            else if (Input.GetKeyDown(KeyCode.S) && !isSelectSkillInput) 
            {
                isSelectSkillInput= true;
                currentSelectSkill++;
                //選択がループするようにする
                if (currentSelectSkill >= skillNum)
                {
                    currentSelectSkill = 0;
                }
                //選択音を鳴らす
                seManager.seSource.PlayOneShot(selectSkillSE);
            }
            //選択に必要なキーの入力が全てなくなった時
            if(!Input.GetKey(KeyCode.W)&& !Input.GetKey(KeyCode.S))
            {
                isSelectSkillInput = false;
            }

            //GUIの反映
            for (int i = 0; i < skillNum; i++)
            {
                //選択中のスキルを少し前にスライドする
                if (i == currentSelectSkill)
                {
                    if (currentSkillPos[i].x >= (originSkillPos[i].x - slideSkillLimit))
                    {
                        currentSkillPos[i].x -= slideSkillSpeed;
                    }
                }
                //選択されていないスキルは初期位置のままにする
                else
                {
                    currentSkillPos[i] = originSkillPos[i];
                }
                skill[i].transform.position = currentSkillPos[i];

                //スキル説明
                //一度全て非表示にする
                explaSkill[i].SetActive(false);
                //選択中にスキルの説明文のみ表示する
                explaSkill[currentSelectSkill].SetActive(true);
            }
        }
        else if(isGame)
        {
            Time.timeScale = 1.0f;
        }

        for (int i = 0; i < skillNum; i++)
        {
            //クールタイム処理
            Image image = skill[i].GetComponent<Image>();
            Color color = image.color;
            if (playerController.isUseSkill[i])
            {
                //使用中かつクールタイム中
                color = Color.black;
            }
            else
            {
                //使用可能
                color = Color.white;
            }
            image.color = color;
        }  
    }

    //スキルGUI
    public void SkillTimerUI()
    {
        for(int i = 0;i < skillNum;i++)
        {
            //有効中のスキルがあるとき
            if (playerController.activeSkill[i]) 
            {
                //タイマーを表示
                skillTimer[i].SetActive(true);
                //コンポーネント取得
                Image timerImage = skillTimer[i].GetComponent<Image>();
                //描画
                timerImage.fillAmount = playerController.activeSkillTimer[i] / playerController.activeSkillLimit[i];
            }
            else
            {
                //タイマーを非表示にする
                skillTimer[i].SetActive(false);
            }
        }
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

    //プレイ結果
    public void CheckResultState()
    {
        //体力が0以下になったらゲームオーバー
        if (gameOver)  
        {
            isGame = false;
            overPanel.SetActive(true);
            mainPanel.SetActive(false);
            //BGM
            bgmManager.PlayBGM(overBGM);
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
            //BGM
            bgmManager.PlayBGM(clearBGM);
            //マウスカーソルの固定を外す
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.0f;
        }
    }

    //プレイヤー読み込み
    public void LoadPlayer(GameObject player)
    {
        playerController = player.GetComponent<PlayerController>();
    }
}
