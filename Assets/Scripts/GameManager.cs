using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    TileMaker tileMaker;
    public Player player;

    public int row, column;
    public float speed;

    void Start()
    {
        tileMaker = GetComponent<TileMaker>();
        tileMaker.MakeBoard(row, column);

        player.Init(tileMaker.TileSize, speed);
    }
}
