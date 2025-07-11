using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("スライダー")]
    public Slider se;
    public Slider bgm;

    [Header("コンポーネント参照")]
    public AudioSource seManager;
    public AudioSource bgmManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //コンポーネント取得
        seManager= GameObject.Find("SEManager").GetComponent<AudioSource>();
        bgmManager= GameObject.Find("BGMManager").GetComponent<AudioSource>();
        //現在の音量に合わせて、Sliderのvalueを設定
        se.value = seManager.volume;
        bgm.value = bgmManager.volume;
    }

    // Update is called once per frame
    void Update()
    {
        //Sliderのvalueに合わせて、音量を変更
        seManager.volume = se.value;
        bgmManager.volume = bgm.value;
    }
}
