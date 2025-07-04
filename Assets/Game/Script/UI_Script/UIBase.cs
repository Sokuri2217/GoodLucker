using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class UIBase : MonoBehaviour
{
    [Header("フェード")]
    public GameObject fadeImage; //色を黒に設定したImage
    public Image fadeColor;      //fadeImageの色情報
    public float startCount;     //ゲーム開始までのカウント

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //シーン開始時に生成
        GameObject createFade = Instantiate(fadeImage, transform);
        fadeColor = GameObject.Find("FadeImage(Clone)").GetComponent<Image>();
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
