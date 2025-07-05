using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("�J�����ړ�")]
    public float mouseSpeed; //�}�E�X���x
    public Transform playerTransfom; //�v���C���[��Transform
    public float rotation;   //��]���x

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // �}�E�X�J�[�\������ʒ����ɌŒ�
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    public void CameraMove()
    {
        //�J�����ړ�(�}�E�X)
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, -90f, 30f); // �㉺�̊p�x����

        transform.localRotation = Quaternion.Euler(rotation, 0f, 0f); // �㉺�iX���j
        playerTransfom.Rotate(Vector3.up * mouseX); // ���E�iY���j
    }
}
