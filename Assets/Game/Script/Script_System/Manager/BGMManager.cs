using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; } //インスタンス保持

    [Header("コンポーネント参照")]
    public AudioSource bgmSource; //BGM

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

    public void PlayBGM(AudioClip bgmClip)
    {
        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }
}
