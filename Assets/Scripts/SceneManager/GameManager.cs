using System;
using System.Collections;
using System.Collections.Generic;
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
    private IDefaultSpawnerStrategy spawnerFactory;
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
    public float spawnDelay;
    public float destroydelay;
    public int toppingTotal = 10;

    [Header("Score")]
    public int oToppingScore;
    public int xToppingScore;
    public int initialScore;
    public int cntXTopping;
    public int cheeseGoal;
    private int score;
    private int cheese;
    private bool ovenOpened = false;
    public int[] toppingCounts;
    public Color oToppingScoreColor;
    public Color xToppingScoreColor;

    [Header("Arrow")]
    public Button up;
    public Button left;
    public Button right;
    public Button down;

    [Header("UI")]
    public Text txtScore;
    public Text maxScore;
    public Text userCoin;
    public GameObject overPanel;
    public Image CheeseGauge;
    public GameObject resultList;
    
    [Header("Clear Panel")]
    public GameObject clearPanel;
    public Text clearScoreTxt;
    public Text txtClearPanelBestScore;

    [Header("For Mode")]
    public Text StageID;
    public Text TimeLimit;
    public Text GoalTopping;
    public Text GoalToppingCNT;
    public Text GoalScore;
    public Text StageMode;
    public int stageNum = 0;
    private int timeLimit = 0;
    private int goalTopping = 0;
    private int goalToppingId = 0;
    private int goalToppingCNT = 0;
    private int minScore = 0;
    private int obstacleCount = 0;

    [Header("Data")] private UserData userData;
    public string[] toppingNameList;

    private GameMode mode = GameMode.INFINITE;
    private StageMode stageMode = global::StageMode.NONE;

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
    }

    void Start()
    {
        goalToppingId = 0;
        
        // 이 부분 정리 필요
        userData = (UserData) Assets.Scripts.Data.DataManager.GetDataFromJson<UserData>("userData");
        if (userData == null) userData = new UserData();

        maxScore.text = "최고 점수 " + userData.maxScore + "₩";
        userCoin.text = userData.coin.ToString();
            
        if (StageLoader.Instance() != null && StageLoader.Instance().mode != GameMode.INFINITE)
		{
            stageNum = Convert.ToInt32(StageLoader.Instance().stageId);
            timeLimit = StageLoader.Instance().timeLimit;
            goalTopping = StageLoader.Instance().goalTopping;
            minScore = StageLoader.Instance().minScore;
            cntXTopping = StageLoader.Instance().cntXTopping;
            obstacleCount = StageLoader.Instance().obstacleCount;
            mode = StageLoader.Instance().mode;
            stageMode = StageLoader.Instance().stageMode;
            goalToppingId = 0;

            StageID.text = stageNum.ToString();
            TimeLimit.text = timeLimit.ToString();
            GoalTopping.text = "목표" + goalTopping.ToString();
            GoalToppingCNT.text = "현재 "+ goalToppingCNT + "개";
            GoalScore.text = "목표 " + minScore.ToString() + "₩";
            StageMode.text = StageLoader.Instance().mode.ToString();
        } else
		{
            StageID.gameObject.SetActive(false);
            TimeLimit.gameObject.SetActive(false);
            GoalTopping.gameObject.SetActive(false);
            GoalScore.gameObject.SetActive(false);
            StageMode.gameObject.SetActive(false);
            GoalToppingCNT.gameObject.SetActive(false);
            stageMode = global::StageMode.NONE;
        }

        spawnerFactory = GetComponent<SpawnerFactory>().GetSpawnerStrategyByMode(gameObject, stageMode);
        toppingCounts = new int[toppingTotal];

        InitGame();
    }

    private void InitGame()
    {
        tileMaker = GetComponent<TileMaker>();
        tileMaker.MakeBoard(row, column, centerPosition.position);

        tileSize = tileMaker.TileSize;
        score = initialScore;

        spawnerFactory.InitFactory(spawnDelay, centerPosition, tileSize, cntXTopping);
        spawnerFactory.AttachSpawner(gameObject);
        
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
        toppingCounts[t.id]++;
        
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

        if (mode == GameMode.INFINITE && t.isCheese) {
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
        
        CheckGameClear();
    }

    public void Stop() 
    {
        player.Stop();
        spawnerFactory.StopPeriodicSpawn();
        
        Time.timeScale = 0;
    }

    public void StartGame() 
    {
        Time.timeScale = 1;

        player.StratMove();
        spawnerFactory.StartPeriodicSpawn();
    }

    public void CheckGameClear()
	{
        // original / avoid
        if (minScore > 0 && score >= minScore)
		{
            spawnerFactory.RequestSpawn(RequestEnum.OVEN, 1);
		}
        // goal topping
        else if (goalTopping > 0 && goalToppingCNT >= goalTopping)
		{
            spawnerFactory.RequestSpawn(RequestEnum.OVEN, 1);
		}
        else if (mode == GameMode.INFINITE && cheese >= cheeseGoal && !ovenOpened)
        {
            ovenOpened = true;
            spawnerFactory.RequestSpawn(RequestEnum.OVEN, 1);
        }
	}

    public void GetCoin()
    {
        userData.SaveCoin(200);
        userCoin.text = "유저 코인: " + userData.coin;
    }

    public void GameOver()
    {
        Stop();

        overPanel.SetActive(true);
    }

    public void GameClear()
    {
        Stop();
        clearPanel.SetActive(true);
        clearScoreTxt.text = score + "₩";
        
        // TODO:: Topping sprite, name 관리하는 무언가.. 만들기
        Sprite[] sprites = spawnerFactory.toppingSprites;

        for (int i = 0; i < resultList.transform.childCount; i++)
        {
            Transform child = resultList.transform.GetChild(i);

            child.GetChild(0).GetComponent<Image>().sprite = sprites[i];
            child.GetChild(1).GetComponent<Text>().text = toppingNameList[i];
            child.GetChild(2).GetComponent<Text>().text = toppingCounts[i] + "개";
            
            // TODO:: 스태틱 사용하지 않기
            if (ToppingSpawner.isOTopping[i])
            {
                Text toppingScoreText = child.GetChild(3).GetComponent<Text>();
                toppingScoreText.text = "+" + toppingCounts[i] * oToppingScore + "₩";
                toppingScoreText.color = oToppingScoreColor;
            } else {
                Text toppingScoreText = child.GetChild(3).GetComponent<Text>();
                toppingScoreText.text = toppingCounts[i] * xToppingScore + "₩";
                toppingScoreText.color = xToppingScoreColor;
            }
        }

        if (mode == GameMode.INFINITE)
        {
            userData.SaveClearData(score);
            txtClearPanelBestScore.text = userData.maxScore + "₩";
        }
        else
        {
            StoryData.SaveStageData(stageNum, score);
        }
    }

    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoMain()
    {
        SceneManager.LoadScene(mode == GameMode.INFINITE ? "Lobby" : "StageSelect");
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
