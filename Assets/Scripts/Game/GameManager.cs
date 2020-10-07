using System;
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
    private SpawnerFactory spawnerFactory;
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
    public Text userCoin;
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
    public int stageNum = 0;
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

        spawnerFactory = GetComponent<SpawnerFactory>();
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        goalToppingId = 0;
        
        // 이 부분 정리 필요
        userData = (UserData) Assets.Scripts.Data.DataManager.GetDataFromJson<UserData>("userData");
        if (userData == null) userData = new UserData();

        maxScore.text = "최고 점수: " + userData.maxScore + "₩";
        userCoin.text = "유저 코인: " + userData.coin;
            
        if (StageLoader.Instance() != null && StageLoader.Instance().mode != GameMode.INFINITE)
		{
            stageNum = Convert.ToInt32(StageLoader.Instance().stageId);
            timeLimit = StageLoader.Instance().timeLimit;
            goalTopping = StageLoader.Instance().goalTopping;
            minScore = StageLoader.Instance().minScore;
            cntXTopping = StageLoader.Instance().cntXTopping;
            obstacleCount = StageLoader.Instance().obstacleCount;
            mode = StageLoader.Instance().mode;

            goalToppingId = 0;

            StageID.text = "stage id: " + stageNum;
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
        tileMaker.MakeBoard(row, column, centerPosition.position);

        tileSize = tileMaker.TileSize;
        score = initialScore;

        spawnerFactory.InitFactory(spawnDelay, centerPosition, tileSize);
        spawnerFactory.CreateSpawner(gameObject);
        
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
        clearScoreTxt.text = score + clearScoreTxt.text;
        clearPanel.SetActive(true);

        if (mode == GameMode.INFINITE)
        {
            userData.SaveClearData(score);
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
