using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Board")]
    public int row;
    public int column;

    [Header("Player")]
    public float speed;
    public float accelerate;

    [Header("Spawn Topping")]
    public float toppingdelay;
    public float destroydelay;

    [Header("Score")]
    public int oToppingScore;
    public int xToppingScore;
    public int initialScore;
    private int score;

    [Header("UI")]
    public Text txtScore;

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
        score = initialScore;

        tileMaker = GetComponent<TileMaker>();
        tileMaker.MakeBoard(row, column);

        this.tileSize = tileMaker.TileSize;

        toppingSpawner = GetComponent<ToppingSpawner>();
        toppingSpawner.StartPooling(toppingdelay, destroydelay);

        player.Init(tileSize, speed, accelerate);
    }

    public void ChangeScore(bool isO)
    {
        score += isO ? oToppingScore : xToppingScore;
        txtScore.text = "Score: " + score;
        if (score < 0)
            GameOver();
    }

    public void GameOver()
    {
        player.GameOver();
        toppingSpawner.Stop();

        Debug.Log("Game Over");
    }
}
