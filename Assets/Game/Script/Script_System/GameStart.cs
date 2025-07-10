using UnityEngine;

public class GameStart : MonoBehaviour
{
    [Header("プレイヤーオブジェクト")]
    public GameObject[] playerPrefab = new GameObject[4];

    [Header("スクリプト参照")]
    public GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("SelectManager").GetComponent<GameManager>();
        Instantiate(playerPrefab[gameManager.selectCharacter], this.transform);
    }
}
