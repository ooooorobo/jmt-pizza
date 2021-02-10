using System;
using System.Collections;
using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

public class IGameManager : MonoBehaviour
{
    private TileMaker tileMaker;
    public IDefaultSpawnerStrategy spawnerFactory;
    public JoystickController joyController;
    private GameChecker gameChecker;
    
    [Header("Player")] 
    public Player player;
    public GameObject playerPrefab;

    [Header("Board")] 
    public Transform centerPosition;
    public float tileSize;

    [Header("Score")] 
    public int score;
    public int cheese;
    public bool ovenOpened = false;
    public int goalToppingCount = 0;
    public int[] toppingCounts;

    [Header("Arrow")] public Button btnUp;
    public Button btnLeft;
    public Button btnRight;
    public Button btnDown;

    [Header("UI")] 
    public Text txtScore;
    public Text userCoin;
    public Image CheeseGauge;
    public Transform canvas;

    [Header("Over Panel")] public GameObject overPanel;
    public Text overBestScore;
    public Text overTotalScore;
    public Text overReason;

    [Header("Data")] 
    public UserData userData;


    private IEnumerator addGaugeCoroutine;

    private static IGameManager instance;

    public static IGameManager Instance()
    {
        if (instance == null)
        {
            return null;
        }
        else return instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        userData = (UserData) DataManager.GetDataFromJson<UserData>("userData");
        if (userData == null) userData = new UserData();

        StageLoader stageLoader = StageLoader.Instance();
        Environment.GameMode gameMode = stageLoader ? stageLoader.mode : Environment.GameMode.INFINITE;
        Environment.StageMode stageMode = stageLoader ? stageLoader.stageMode : Environment.StageMode.ORIGINAL;

        GameCheckerFactory gameCheckerFactory = GetComponent<GameCheckerFactory>();
        gameChecker = gameCheckerFactory.GetGameCheckerByMode(gameMode, stageMode, canvas);

        tileMaker = GetComponent<TileMaker>();
        tileMaker.MakeBoard(Environment.BoardRowCount, Environment.BoardColumnCount, centerPosition.position);
        tileSize = tileMaker.TileSize;

        score = Environment.InfiniteInitialScore;

        player = Instantiate(playerPrefab, centerPosition.position, Quaternion.identity).GetComponent<Player>();
        player.Init(tileSize, Environment.InfinitePlayerInitialSpeed, Environment.InfinitePlayerAccelerateSpeed,
            centerPosition.position);

        btnUp.onClick.AddListener(() => player.SetDirection("12"));
        btnDown.onClick.AddListener(() => player.SetDirection("10"));
        btnLeft.onClick.AddListener(() => player.SetDirection("01"));
        btnRight.onClick.AddListener(() => player.SetDirection("21"));

        joyController.Init(player);
        
        toppingCounts = new int[Environment.InfiniteToppingTotalCount];

        gameChecker.InitGame();
    }

    // 게임 기본 세팅
    public void GetTopping(Topping t)
    {
        toppingCounts[t.id] += 1;
        
        if (t.isO)
        {
            ChangeScore(Environment.InfiniteOToppingScore);
            player.GetComponent<Animator>().SetTrigger("Happy");
        }
        else
        {
            ChangeScore(Environment.InfiniteXToppingScore);
            player.GetComponent<Animator>().SetTrigger("Sad");
        }

        if (t.isCheese)
        {
            cheese++;

            if (addGaugeCoroutine != null)
                StopCoroutine(addGaugeCoroutine);

            addGaugeCoroutine = UIManager.FillAmount(CheeseGauge,
                (float) cheese / Environment.InfiniteTargetToppingGoalMin, 0.0035f);
            StartCoroutine(addGaugeCoroutine);
        }

        if (t.id == Environment.InfiniteTargetToppingId)
        {
            goalToppingCount++;
        }

        gameChecker.CheckGameClear();
    }


    // O/X 토핑 획득 시 - 점수 변경
    public void ChangeScore(int change)
    {
        score += change;
        txtScore.text = score + "￦";

        gameChecker.CheckGameOver();
    }


// 코인 획득 시 - 플레이어 코인 변경
    public void GetCoin(int coin)
    {
        userData.SaveCoin(200);
        userCoin.text = "유저 코인: " + userData.coin;
    }
    
    // 게임 일시정지 시
    public void StopGame()
    {
        player.Stop();
        spawnerFactory.StopPeriodicSpawn();
        
        Time.timeScale = 0;
    }

    // 게임 재개 시
    public void ResumeGame()
    {
        Time.timeScale = 1;

        player.StartMove();
        spawnerFactory.StartPeriodicSpawn();
    }
    
    // 게임 클리어 후 로직
    public void GameClear()
    {
        StopGame();
        userData.SaveClearData(score);

        gameChecker.SetGameClearPanelUi();
    }
    
    // 게임 오버 후 로직
    public void GameOver()
    {
        StopGame();
        
        overBestScore.text = userData.maxScore + "₩";
        overTotalScore.text = score + "₩";
        
        overReason.text = score < 1 ? "맛없는 피자가 되었어요 ㅠㅠ" : "따라오던 토핑과 부딪혔어요 ㅠㅠ";
        
        overPanel.SetActive(true);
    }
}