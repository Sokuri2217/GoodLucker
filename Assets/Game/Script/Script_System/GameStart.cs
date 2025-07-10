using UnityEngine;

public class GameStart : MonoBehaviour
{
    [Header("�v���C���[�I�u�W�F�N�g")]
    public GameObject[] playerPrefab = new GameObject[4];

    [Header("�X�N���v�g�Q��")]
    public GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("SelectManager").GetComponent<GameManager>();
        Instantiate(playerPrefab[gameManager.selectCharacter], this.transform);
    }
}
