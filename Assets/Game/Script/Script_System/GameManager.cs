using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } //�C���X�^���X�ێ�

    [Header("�Z���N�g��")]
    public int selectCharacter; //�g�p�L����
    public int selectStage;     //�I���X�e�[�W

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
        selectCharacter = 0;
        selectStage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
