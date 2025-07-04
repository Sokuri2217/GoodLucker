using UnityEngine;
using System.Collections;

public class ButtonEnd : ButtonBase
{
    [Header("遅延")]
    public float delay;

    public void ButtonClick()
    {
        StartCoroutine(LoadEnd());

    }

    private IEnumerator LoadEnd()
    {
        yield return new WaitForSeconds(delay);

        //終了処理
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }
}
