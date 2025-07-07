using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } //�C���X�^���X�ێ�

    [Header("�Z���N�g��")]
    public int selectCharacter; //�g�p�L����
    public int selectStage;     //�I���X�e�[�W

    [Header("�X�e�[�^�X")]
    public float maxStatus; //�e�X�e�[�^�X�̍ő�l
    public float[] status;  //STR,DEF,AGI,LUK

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
        selectCharacter = 0;
        selectStage = 0;
    }
}
