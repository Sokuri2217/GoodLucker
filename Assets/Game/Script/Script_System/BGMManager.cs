using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; } //�C���X�^���X�ێ�

    [Header("�R���|�[�l���g�Q��")]
    public AudioSource bgmSource; //BGM

    //�V���O���g��
    public void Awake()
    {
        // ���łɃC���X�^���X�����݂���ꍇ�͍폜
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // �V�[�����܂����ŃI�u�W�F�N�g��ێ�
    }

    public void PlayBGM(AudioClip bgmClip)
    {
        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }
}
