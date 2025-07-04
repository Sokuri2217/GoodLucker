using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScene : ButtonBase
{
    [Header("シーン名")]
    public string sceneName;

    [Header("遅延")]
    public float delay; //シーン移動するまでの遅延

    [Header("フェード")]
    public Image fadeImage; //黒色のImageを設定

    public void ButtonClick()
    {
        //fadeImageを取得
        fadeImage = GameObject.Find("FadeImage(Clone)").GetComponent<Image>();
        //処理実行
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        //フェード用タイマー
        float fadeTimer = 0.0f;
        //色情報を取得
        Color color = fadeImage.color;

        while (fadeTimer<delay)
        {
            //徐々にfadeImageの透明度を下げる
            color.a = Mathf.Lerp(0.0f, 1.0f, fadeTimer / delay);
            fadeImage.color = color;
            //時間計測
            fadeTimer += Time.deltaTime;
            yield return null;
        }

        //完全に黒にする
        color.a = 1.0f;
        fadeImage.color = color;

        //シーン移動
        SceneManager.LoadScene(sceneName);
    }
}
