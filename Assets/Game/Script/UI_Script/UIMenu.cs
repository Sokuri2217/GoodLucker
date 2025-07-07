using UnityEngine;
using UnityEngine.UI;

public class UIMenu : UIBase
{
    [Header("�X�e�[�W��")]
    public string[] sceneName;
    [Header("�I��")]
    public Image selectedCharacter;    //�L�����N�^�[
    public Image selectedStage;        //�X�e�[�W
    public Sprite[] characterIcon;     //�L�����A�C�R��
    public Sprite[] stageIcon;         //�X�e�[�W�A�C�R��
    public Image explanationCharacter; //�L�����N�^�[����
    public Image explanationStage;     //�X�e�[�W����
    public Sprite[] characterExpla;    //�L����Sprite
    public Sprite[] stageExpla;        //�X�e�[�WSprite
    [Header("�Z���N�g�p�l��")]
    public GameObject[] selectPanel = new GameObject[2];
    [Header("������")]
    public GameObject explanationWindow; //�g
    public Image[] statusBar;            //�X�e�[�^�X��_�̒��Z�ŕ\��(STR,DEF,AGI,LUK)
    [Header("�{�^��")]
    public GameObject closeSelectPanel;
    [Header("�X�N���v�g�Q��")]
    public ButtonScene buttonScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //�R���|�[�l���g�擾
        selectedCharacter = GameObject.Find("SelectedCharacter").GetComponent<Image>();
        selectedStage = GameObject.Find("SelectedStage").GetComponent<Image>();
        //�X�N���v�g�擾
        buttonScene = GameObject.Find("StartButton").GetComponent<ButtonScene>();
        //�Z���N�g�p�l�����\��
        for (int i = 0; i < 2; i++) 
        {
            selectPanel[i].SetActive(false);
        }
        //�Z���N�g�p�l�������{�^�����\��
        closeSelectPanel.SetActive(false);
        //�I����e�̐��������\��
        explanationWindow.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //�I����Ԃ�����
        ChangeSprite();
        //�V�[���ݒ�
        SceneSetting();
    }

    //�I����Ԃ�����
    void ChangeSprite()
    {
        //GamaManager����I����Ԃ��擾��Sprite�ɔ��f
        selectedCharacter.sprite = characterIcon[(gameManager.selectCharacter)];
        selectedStage.sprite = stageIcon[(gameManager.selectStage)];
    }
    //�V�[���ݒ�
    void SceneSetting()
    {
        buttonScene.sceneName = sceneName[(gameManager.selectStage)];
    }
}
