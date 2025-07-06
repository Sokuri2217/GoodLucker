using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("ボタンの拡大率")]
    public Vector3 originScale; //元値
    public float zoom;          //拡大率
    [Header("SE")]
    public AudioClip onCursor; //カーソルがボタンと重なった音
    public AudioClip isClick; //ボタンをクリックした音
    [Header("スクリプト参照")]
    public SEManager seManager; //SE

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //スクリプト情報を取得
        seManager = GameObject.Find("SEManager").GetComponent<SEManager>();
        //初期値の保存
        originScale = transform.localScale;
    }

    // マウスがボタンに入った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        //ボタンを拡大する
        transform.localScale = originScale * zoom;
        //SE
        seManager.seSource.PlayOneShot(onCursor);
    }

    // マウスがボタンから離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        //ボタンのサイズを元に戻す
        transform.localScale = originScale;
    }

    //クリックしたときの挙動
    public void OnClick()
    {
        Button button = GetComponent<Button>();
        button.interactable = false;
    }
    public void ClickSE()
    {
        //SE
        seManager.seSource.PlayOneShot(isClick);
    }
}
