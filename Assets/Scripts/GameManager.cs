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
    ToppingSpawner toppingSpawner;
    public Player player;

    public int row, column;
    public float speed, toppingdelay;

    [HideInInspector]
    public float tileSize;

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

        toppingSpawner = GetComponent<ToppingSpawner>();
        toppingSpawner.StartPooling(toppingdelay);

        this.tileSize = tileMaker.TileSize;

        player.Init(tileSize, speed);


    }
}
