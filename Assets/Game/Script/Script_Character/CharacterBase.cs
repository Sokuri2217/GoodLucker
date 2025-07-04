using UnityEngine;
using UnityEngine.AI;

public class CharacterBase : MonoBehaviour
{
    [Header("�X�e�[�^�X")]
    public int maxHp;     //�ő�̗�
    public int currentHp; //���݂̗̑�
    public int attack;    //�U����
    public int defence;   //�h���
    public int speed;     //�ړ����x
    public int luck;      //�^

    [Header("�㏸��")]
    public float addHp;      //�̗�
    public float addAttack;  //�U����
    public float addDefence; //�h���
    public float addSpeed;   //�ړ����x

    [Header("�R���|�[�l���g�Q��")]
    public Rigidbody rb;       //��������
    public NavMeshAgent agent; //�o�H�T��


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //�R���|�[�l���g�擾
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        //������
        currentHp = maxHp;   //�̗�
        agent.speed = speed; //�ړ����x
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
