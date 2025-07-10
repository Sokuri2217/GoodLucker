using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStage : UIBase
{
    [Header("現在のシーン名")]
    public string currentSceneName;
    [Header("ゲーム状態")]
    public bool isStop;
    [Header("パネル")]
    public GameObject gameStopPanel; //一時停止
    public GameObject reallyPanel;   //最終確認
    [Header("離脱理由(リタイア・リトライ)")]
    public bool[] exit = new bool[2];

    //長押し防止用
    private bool isInput; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //全てのパネルを非表示
        gameStopPanel.SetActive(false);
        reallyPanel.SetActive(false);
        //現在のシーン名を取得
        currentSceneName = SceneManager.GetActiveScene().name;
        // マウスカーソルを画面中央に固定
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //Escapeを押したとき
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
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            isInput = false;
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
                gameStopPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked; // マウスカーソルを画面中央に固定
                //時間を通常に戻す
                Time.timeScale = 1.0f;
                break;
            //プレイ中
            case false:
                isStop = true;
                gameStopPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None; // マウスカーソルの固定を外す
                //時間を止める
                Time.timeScale = 0.0f;
                break;
        }
    }
}
