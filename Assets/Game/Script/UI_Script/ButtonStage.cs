using UnityEditor;
using UnityEngine;

public class ButtonStage : MonoBehaviour
{
    [Header("スクリプト参照")]
    public UIStage stage;           //ステージUI
    public GameManager gameManager; //選択状態を保存

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        //メニュー情報を取得
        stage = GameObject.Find("StageUI").GetComponent<UIStage>();
        gameManager = GameObject.Find("SelectManager").GetComponent<GameManager>();
    }

    //現在のステージから離れる
    public void ExitStage()
    {
        //確認画面を表示
        stage.reallyPanel.SetActive(true);
        //停止画面を非表示
        stage.gameStopPanel.SetActive(false);
    }
    //離脱理由(リタイア)
    public void Retire()
    {
        stage.exit[0] = true;
    }
    //離脱理由(リタイア)
    public void Retry()
    {
        stage.exit[1] = true;
    }
    //取り消し
    public void NotExit()
    {
        //離脱理由をリセット
        for (int i = 0; i < 2; i++)
            stage.exit[i] = false;
        //停止画面を表示
        stage.gameStopPanel.SetActive(true);
        //確認画面を非表示
        stage.reallyPanel.SetActive(false);
    }
    //実行
    public void YesExit()
    {
        //時間を通常に戻す
        Time.timeScale = 1.0f;
        //シーン移動用ボタンスクリプトを取得
        ButtonScene buttonScene = GetComponent<ButtonScene>();
        //リタイアの場合
        if (stage.exit[0])
        {
            buttonScene.sceneName = "Menu";
        }
        //リトライの場合
        else if(stage.exit[1])
        {
            //現在のシーンを再読み込み
            buttonScene.sceneName = stage.currentSceneName;
        }
    }
}
