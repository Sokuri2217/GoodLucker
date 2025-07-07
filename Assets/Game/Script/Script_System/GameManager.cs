using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } //インスタンス保持

    [Header("セレクト状況")]
    public int selectCharacter; //使用キャラ
    public int selectStage;     //選択ステージ

    [Header("ステータス")]
    public float maxStatus; //各ステータスの最大値
    public float[] status;  //STR,DEF,AGI,LUK

    //シングルトン
    public void Awake()
    {
        // すでにインスタンスが存在する場合は削除
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンをまたいでオブジェクトを保持
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectCharacter = 0;
        selectStage = 0;
    }
}
