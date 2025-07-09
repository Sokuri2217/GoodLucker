using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBase : MonoBehaviour
{
    [Header("フェード")]
    public GameObject fadeImage; //色を黒に設定したImage
    public Image fadeColor;      //fadeImageの色情報
    public float startCount;     //ゲーム開始までのカウント

    [Header("BGM")]
    public AudioClip bgm;

    [Header("スクリプト参照")]
    protected GameManager gameManager;
    public BGMManager bgmManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //シーン開始時に生成
        GameObject createFade = Instantiate(fadeImage, transform);
        //コンポーネント取得
        fadeColor = GameObject.Find("FadeImage(Clone)").GetComponent<Image>();
        //スクリプト取得
        gameManager = GameObject.Find("SelectManager").GetComponent<GameManager>();
        bgmManager = GameObject.Find("BGMManager").GetComponent<BGMManager>();
        //BGM再生
        bgmManager.PlayBGM(bgm);
        //フェードイン
        StartCoroutine(StartGame());

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected IEnumerator StartGame()
    {
        //フェード用タイマー
        float fadeTimer = 0.0f;
        //色情報を取得
        Color color = fadeColor.color;

        while (fadeTimer < startCount)
        {
            //徐々にfadeImageの透明度を上げる
            color.a = Mathf.Lerp(1.0f, 0.0f, fadeTimer / startCount);
            fadeColor.color = color;
            //時間計測
            fadeTimer += Time.deltaTime;
            yield return null;
        }

        //完全に透明にする
        color.a = 0.0f;
        fadeColor.color = color;
    }
}
