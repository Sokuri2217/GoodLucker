using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance { get; private set; } //インスタンス保持
    [Header("コンポーネント参照")]
    public AudioSource seSource; //SE

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        seSource = GetComponent<AudioSource>();
    }
}
