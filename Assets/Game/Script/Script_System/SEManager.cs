using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance { get; private set; } //�C���X�^���X�ێ�
    [Header("�R���|�[�l���g�Q��")]
    public AudioSource seSource; //SE

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        seSource = GetComponent<AudioSource>();
    }
}
