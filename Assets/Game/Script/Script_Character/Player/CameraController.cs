using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("カメラ移動")]
    public float mouseSpeed; //マウス感度
    public Transform playerTransfom; //プレイヤーのTransform
    public float rotation;   //回転速度

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // マウスカーソルを画面中央に固定
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    public void CameraMove()
    {
        //カメラ移動(マウス)
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, -90f, 30f); // 上下の角度制限

        transform.localRotation = Quaternion.Euler(rotation, 0f, 0f); // 上下（X軸）
        playerTransfom.Rotate(Vector3.up * mouseX); // 左右（Y軸）
    }
}
