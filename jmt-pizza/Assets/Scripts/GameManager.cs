using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    BoardManager boardManager;
    PlayerController playerController;

    public GameObject player;

    private bool isPlaying = false;

    private void Awake()
    {
        boardManager = GetComponent<BoardManager>();
        playerController = player.GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        boardManager.SetBoard();
        StartCoroutine(CheckGameStart());
    }

    IEnumerator CheckGameStart()
    {
        while (!isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                isPlaying = true;

            yield return null;
        }

        Debug.Log("Start Play");

        // 플레이어 이동 시작
        playerController.InitPlayer(boardManager.FloorSize, boardManager.columns, boardManager.rows);

        // 오브젝트 스폰 시작
        boardManager.canSpawn = true;
        StartCoroutine(boardManager.Spawn(boardManager.topping, 0.1f));
    }

}
