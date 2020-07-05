using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance()
    {
        if (instance == null)
        {
            return null;
        }
        else return instance;
    }

    TileMaker tileMaker;
    public Player player;

    public int row, column;
    public float speed;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        tileMaker = GetComponent<TileMaker>();
        tileMaker.MakeBoard(row, column);

        player.Init(tileMaker.TileSize, speed);
    }
}
