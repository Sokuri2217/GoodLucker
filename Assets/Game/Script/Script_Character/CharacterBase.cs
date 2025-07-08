using UnityEngine;
using UnityEngine.AI;

public class CharacterBase : MonoBehaviour
{
    [Header("�̗�")]
    public float maxHp;      //�ő�̗�
    public float currentHp;  //���݂̗̑�
    [Header("�X�e�[�^�X(�U���E�h��E���x�E�^)")]
    public float[] status; //�X�e�[�^�X

    [Header("�㏸��")]
    public float addHp;      //�̗�
    public float addAttack;  //�U����
    public float addDefence; //�h���
    public float addSpeed;   //�ړ����x

    [Header("�A�j���[�V����")]
    public string animatorName; //BlendTree��

    [Header("�R���|�[�l���g�Q��")]
    protected Rigidbody rb;       //��������
    protected NavMeshAgent agent; //�o�H�T��
    protected Animator animator;  //�A�j���[�V����


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //�R���|�[�l���g�擾
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
