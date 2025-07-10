using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("�J�����ړ�")]
    public float mouseSpeed;         //�}�E�X���x
    public Transform playerTransfom; //�v���C���[��Transform
    public Vector3 offset;           //�J�����̃I�t�Z�b�g(�v���C���[�ƃJ�����̑��Έʒu)
    public float minYAngle;          //�J������������������E�l
    public float maxYAngle;          //�J�����������������E�l
    public float horizontalRotation; //���������̉�]
    public float verticalRotation;   //���������̉�]

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransfom = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    public void CameraMove()
    {
        if (Time.timeScale == 0) return;

        // �}�E�X����
        horizontalRotation += Input.GetAxis("Mouse X") * mouseSpeed;
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, minYAngle, maxYAngle);

        // ��]�v�Z
        Quaternion rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        Vector3 desiredPosition = playerTransfom.position + rotation * offset;

        // �J�����̈ʒu�Ɖ�]��ݒ�
        transform.position = desiredPosition;
        transform.LookAt(playerTransfom.position + Vector3.up * 1.5f);
    }
}
