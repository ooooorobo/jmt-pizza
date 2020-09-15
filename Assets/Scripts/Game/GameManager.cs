﻿using System;
using System.Collections;
using Assets.Scripts.Data;
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
    IToppingSpawner toppingSpawner;
    public JoystickController joyController;
    public Player player;

    [Header("Board")]
    public int column;
    public int row;
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
    public int cntXTopping;
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
    public Text clearScoreTxt;
    public Text maxScore;
    public GameObject clearPanel;
    public GameObject overPanel;
    public Image CheeseGauge;

    [Header("For Mode")]
    public Text StageID;
    public Text TimeLimit;
    public Text GoalTopping;
    public Text GoalToppingCNT;
    public Text GoalScore;
    public Text StageMode;
    public string stageId = "";
    private int timeLimit = 0;
    private int goalTopping = 0;
    private int goalToppingId = 0;
    private int goalToppingCNT = 0;
    private int minScore = 0;
    private int obstacleCount = 0;

    [Header("Data")] private UserData userData;

    private GameMode mode = GameMode.INFINITE;

    IEnumerator addGaugeCoroutine;


    private float tileSize;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        goalToppingId = 0;
        
        userData = new UserData();
        userData.SetInitialData();

        maxScore.text = "최고 점수: " + userData.maxScore + "₩";
            
        if (StageLoader.Instance() != null && StageLoader.Instance().mode != GameMode.INFINITE)
		{
            stageId = StageLoader.Instance().stageId;
            timeLimit = StageLoader.Instance().timeLimit;
            goalTopping = StageLoader.Instance().goalTopping;
            minScore = StageLoader.Instance().minScore;
            cntXTopping = StageLoader.Instance().cntXTopping;
            obstacleCount = StageLoader.Instance().obstacleCount;
            mode = StageLoader.Instance().mode;

            goalToppingId = 0;

            StageID.text = "stage id: " + stageId;
            TimeLimit.text = "시간 제한: " + timeLimit.ToString();
            GoalTopping.text = "토핑 " + goalTopping.ToString() + "개 목표";
            GoalToppingCNT.text = "현재 "+ goalToppingCNT + "개";
            GoalScore.text = "목표 " + minScore.ToString() + "원";
            StageMode.text = StageLoader.Instance().mode.ToString();
        } else
		{
            StageID.gameObject.SetActive(false);
            TimeLimit.gameObject.SetActive(false);
            GoalTopping.gameObject.SetActive(false);
            GoalScore.gameObject.SetActive(false);
            StageMode.gameObject.SetActive(false);
            GoalToppingCNT.gameObject.SetActive(false);
        }

        InitGame();
        
    }

    private void InitGame()
    {
        tileMaker = GetComponent<TileMaker>();
        tileMaker.MakeBoard(column, row, centerPosition.position);

        tileSize = tileMaker.TileSize;
        score = initialScore;

        // SpawnerFactory.CreateSpawner(StageLoader.Instance().stageMode, gameObject);
        toppingSpawner = GetComponent<IToppingSpawner>();
        toppingSpawner.InitSpawner(toppingdelay, destroydelay, centerPosition.position, tileSize, cntXTopping, obstacleCount);

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

        if (t.isO)
		{
            score += oToppingScore;
            player.GetComponent<Animator>().SetTrigger("Happy");
		}
        else
		{
            score += xToppingScore;
            player.GetComponent<Animator>().SetTrigger("Sad");
        }

        if (t.isCheese) {
            cheese++;
            if (addGaugeCoroutine != null)
                StopCoroutine(addGaugeCoroutine);
            addGaugeCoroutine = AddGauge((float)cheese / cheeseGoal);
            StartCoroutine(addGaugeCoroutine);
        }
        if (t.id == goalToppingId)
		{
            goalToppingCNT++;
            GoalToppingCNT.text = "현재 " + goalToppingCNT + "개";
        }

        txtScore.text = score + "￦";

        if (score < 0)
            GameOver();

        if (cheese == cheeseGoal && !ovenOpened && mode == GameMode.INFINITE)
        {
            toppingSpawner.MakeOven();
            ovenOpened = true;
        }

        CheckGameClear();
    }

    public void Stop() 
    {
        player.Stop();
        toppingSpawner.Stop();

        Time.timeScale = 0;
    }

    public void StartGame() 
    {
        Time.timeScale = 1;

        player.StratMove();
        toppingSpawner.StartSpawn();
    }

    public void CheckGameClear()
	{
        // original / avoid
        if (minScore > 0 && score >= minScore)
		{
            toppingSpawner.MakeOven();
		}
        // goal topping
        else if (goalTopping > 0 && goalToppingCNT >= goalTopping)
		{
            toppingSpawner.MakeOven();
		}
	}

    public void GameOver()
    {
        Stop();

        overPanel.SetActive(true);
    }

    public void GameClear()
    {
        Stop();
        clearScoreTxt.text = score + clearScoreTxt.text;
        clearPanel.SetActive(true);
        
        userData.SaveClearData(score);
    }

    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoMain()
    {
        SceneManager.LoadScene("Story");
    }

    IEnumerator AddGauge (float maximum)
	{
        while (CheeseGauge.fillAmount < maximum)
		{
            CheeseGauge.fillAmount += 0.0035f;

            yield return null;
		}
	}
}
