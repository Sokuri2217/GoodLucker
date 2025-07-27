using Unity.VisualScripting;
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
    public float originDistance;     //プレイヤーとの基本距離
    public LayerMask objLayerMask;   //判定を取るオブジェクトのレイヤー
    public Vector3 moveVec;          //移動するベクトル
    public float moveSpeed;          //移動する速度

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransfom == null)
        {
            playerTransfom = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
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

        // プレイヤーから理想のカメラ位置へ向かっての方向ベクトルを計算
        Vector3 direction = (desiredPosition - playerTransfom.position).normalized;

        // プレイヤーとカメラの距離を取得（Rayの最大距離）
        float maxDistance = Vector3.Distance(playerTransfom.position, desiredPosition);

        RaycastHit hit;

        // 壁や障害物にぶつかったかチェック（collisionMaskはInspectorで指定）
        if (Physics.Raycast(playerTransfom.position, direction, out hit, maxDistance, objLayerMask))
        {
            // 障害物がある場合、カメラをぶつかる直前の位置にずらす
            desiredPosition = hit.point - direction * 0.1f; // 0.1f は少し余裕を持たせてめり込み防止
        }

        // カメラの位置と回転を設定
        transform.position = desiredPosition;
        transform.LookAt(playerTransfom.position + Vector3.up * 1.5f);
    }
}
