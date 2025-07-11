using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("�X���C�_�[")]
    public Slider se;
    public Slider bgm;

    [Header("�R���|�[�l���g�Q��")]
    public AudioSource seManager;
    public AudioSource bgmManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�R���|�[�l���g�擾
        seManager= GameObject.Find("SEManager").GetComponent<AudioSource>();
        bgmManager= GameObject.Find("BGMManager").GetComponent<AudioSource>();
        //���݂̉��ʂɍ��킹�āASlider��value��ݒ�
        se.value = seManager.volume;
        bgm.value = bgmManager.volume;
    }

    // Update is called once per frame
    void Update()
    {
        //Slider��value�ɍ��킹�āA���ʂ�ύX
        seManager.volume = se.value;
        bgmManager.volume = bgm.value;
    }
}
