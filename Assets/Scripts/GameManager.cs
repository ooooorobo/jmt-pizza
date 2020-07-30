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
    public JoystickController joyController;
    public Player player;

    [Header("Board")]
    public int row;
    public int column;
    public Transform centerPosition;

    [Header("Player")]
    public GameObject playerPrefab;
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
    private bool ovenOpened = false;

    [Header("Arrow")]
    public Button up;
    public Button left;
    public Button right;
    public Button down;

    [Header("UI")]
    public Text txtScore;
    public Text cheeseScore;
    public Text clearScoreTxt;
    public GameObject clearPanel;

    private float tileSize;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            instance.InitGame();
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        tileMaker = GetComponent<TileMaker>();
        tileMaker.MakeBoard(row, column, centerPosition.position);

        tileSize = tileMaker.TileSize;
        score = initialScore;

        toppingSpawner = GetComponent<ToppingSpawner>();
        toppingSpawner.InitSpawner(toppingdelay, destroydelay, centerPosition.position, tileSize);

        player = Instantiate(playerPrefab, centerPosition.position, Quaternion.identity).GetComponent<Player>();
        player.Init(tileSize, speed, accelerate, centerPosition.position);

        up.onClick.AddListener(() => player.SetDirection("12"));
        down.onClick.AddListener(() => player.SetDirection("10"));
        left.onClick.AddListener(() => player.SetDirection("01"));
        right.onClick.AddListener(() => player.SetDirection("21"));

        joyController.Init(player);
    }

    public void ChangeScore(Topping t )
    {
        score += t.isO ? oToppingScore : xToppingScore;
        if (t.isCheese) cheese++;

        txtScore.text = "Score: " + score;
        cheeseScore.text = "Cheese: " + cheese;

        if (score < 0)
            GameOver();

        if (cheese == cheeseGoal && !ovenOpened)
        {
            toppingSpawner.MakeOven();
            ovenOpened = true;
        }
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
