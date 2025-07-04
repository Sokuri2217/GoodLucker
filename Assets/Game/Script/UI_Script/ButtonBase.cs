using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("ボタンの拡大率")]
    public Vector3 originScale; //元値
    public float zoom;            //拡大率

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //初期値の保存
        originScale = transform.localScale;
    }

    // マウスがボタンに入った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        //ボタンを拡大する
        transform.localScale = originScale * zoom;
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
}
