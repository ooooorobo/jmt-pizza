using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public int cheeseGoal;
    private int score;
    private int cheese;

    [Header("UI")]
    public Text txtScore;
    public Text cheeseScore;
    public Text clearScoreTxt;
    public GameObject clearPanel;

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
        toppingSpawner.InitSpawner(toppingdelay, destroydelay);

        player.Init(tileSize, speed, accelerate);
    }

    public void ChangeScore(Topping t )
    {
        score += t.isO ? oToppingScore : xToppingScore;
        if (t.isCheese) cheese++;

        txtScore.text = "Score: " + score;
        cheeseScore.text = "Cheese: " + cheese;

        if (score < 0)
            GameOver();
        
        if (cheese == cheeseGoal) toppingSpawner.MakeOven();
    }

    public void Stop() 
    {
        player.Stop();
        toppingSpawner.Stop();
    }

    public void StartGame() 
    {
        player.StratMove();
        toppingSpawner.StartSpawn();
    }

    public void GameOver()
    {
        Stop();

        Debug.Log("Game Over");
    }

    public void GameClear()
    {
        Stop();
        clearScoreTxt.text = score + clearScoreTxt.text;
        clearPanel.SetActive(true);
    }

    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
