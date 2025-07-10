using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("カメラ移動")]
    public float mouseSpeed;         //マウス感度
    public Transform playerTransfom; //プレイヤーのTransform
    public Vector3 offset;           //カメラのオフセット(プレイヤーとカメラの相対位置)
    public float minYAngle;          //カメラが下を向ける限界値
    public float maxYAngle;          //カメラが上を向ける限界値
    public float horizontalRotation; //水平方向の回転
    public float verticalRotation;   //垂直方向の回転

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

        // マウス入力
        horizontalRotation += Input.GetAxis("Mouse X") * mouseSpeed;
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, minYAngle, maxYAngle);

        // 回転計算
        Quaternion rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        Vector3 desiredPosition = playerTransfom.position + rotation * offset;

        // カメラの位置と回転を設定
        transform.position = desiredPosition;
        transform.LookAt(playerTransfom.position + Vector3.up * 1.5f);
    }
}
