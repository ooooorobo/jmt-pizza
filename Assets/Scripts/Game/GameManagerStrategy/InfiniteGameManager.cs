using System.Collections;
using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteGameManager: MonoBehaviour, IGameManager
{
    private TileMaker tileMaker;
    private IDefaultSpawnerStrategy spawnerFactory;
    public JoystickController joyController;
    
    [Header("Player")]
    public Player player;
    public GameObject playerPrefab;

    [Header("Board")]
    public Transform centerPosition;
    private float tileSize;

    [Header("Score")]
    private int score;
    private int cheese;
    private bool ovenOpened = false;
    private int goalToppingCount = 0;
    public int[] toppingCounts;
    
    [Header("Arrow")]
    public Button btnUp;
    public Button btnLeft;
    public Button btnRight;
    public Button btnDown;

    [Header("UI")]
    public Text txtScore;
    public Text userCoin;
    public GameObject resultList;
    public Image CheeseGauge;
    
    [Header("Over Panel")] 
    public GameObject overPanel;
    public Text overBestScore;
    public Text overTotalScore;
    public Text overReason;
    
    [Header("Clear Panel")]
    public GameObject clearPanel;
    public Text clearScoreTxt;
    public Text txtClearPanelBestScore;


    [Header("Data")] 
    private UserData userData;
    
    
    private IEnumerator addGaugeCoroutine;


    private static InfiniteGameManager instance;
    public static InfiniteGameManager Instance()
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

        spawnerFactory = GetComponent<SpawnerFactory>().GetSpawnerStrategyByMode(gameObject, Environment.StageMode.ORIGINAL);
        toppingCounts = new int[Environment.InfiniteToppingTotalCount];

        InitGame();        
    }
    
    public void InitGame()
    {
        tileMaker = GetComponent<TileMaker>();
        tileMaker.MakeBoard(Environment.BoardRowCount, Environment.BoardColumnCount, centerPosition.position);

        tileSize = tileMaker.TileSize;
        score = Environment.InfiniteInitialScore;

        spawnerFactory.InitFactory(Environment.InfiniteToppingSpawnDelay, centerPosition, tileSize, Environment.InfiniteXToppingCount);
        spawnerFactory.AttachSpawner(gameObject);
        
        player = Instantiate(playerPrefab, centerPosition.position, Quaternion.identity).GetComponent<Player>();
        player.Init(tileSize, Environment.InfinitePlayerInitialSpeed, Environment.InfinitePlayerAccelerateSpeed, centerPosition.position);

        btnUp.onClick.AddListener(() => player.SetDirection("12"));
        btnDown.onClick.AddListener(() => player.SetDirection("10"));
        btnLeft.onClick.AddListener(() => player.SetDirection("01"));
        btnRight.onClick.AddListener(() => player.SetDirection("21"));

        joyController.Init(player);
    }

    public void GetTopping(Topping t)
    {
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
        
        if (t.isCheese) {
            cheese++;
            
            if (addGaugeCoroutine != null)
                StopCoroutine(addGaugeCoroutine);
            
            addGaugeCoroutine = UIManager.FillAmount(CheeseGauge, (float)cheese / Environment.InfiniteTargetToppingGoalMin, 0.0035f);
            StartCoroutine(addGaugeCoroutine);
        }
        if (t.id == Environment.InfiniteTargetToppingId)
        {
            goalToppingCount++;
        }

        CheckGameClear();
    }

    public void ChangeScore(int change)
    {
        score += change;
        txtScore.text = score + "￦";
        
        if (score < Environment.InfiniteScoreMinimum)
        {
            GameOver();
        }
    }

    public void GetCoin(int coin)
    {
        userData.SaveCoin(200);
        
        userCoin.text = "유저 코인: " + userData.coin;
    }

    public void StopGame()
    {
        player.Stop();
        spawnerFactory.StopPeriodicSpawn();
        
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        player.StratMove();
        spawnerFactory.StartPeriodicSpawn();
    }

    public void CheckGameClear()
    {
        if (goalToppingCount == Environment.InfiniteTargetToppingGoalMin && !ovenOpened)
        {
            ovenOpened = true;
            spawnerFactory.RequestSpawn(RequestEnum.OVEN, 1);
        }
    }

    public void GameClear()
    {
        StopGame();
        userData.SaveClearData(score);

        clearPanel.SetActive(true);
        clearScoreTxt.text = score + "₩";
        
        // TODO:: Topping sprite, name 관리하는 무언가.. 만들기
        Sprite[] sprites = spawnerFactory.toppingSprites;

        for (int i = 0; i < resultList.transform.childCount; i++)
        {
            Transform child = resultList.transform.GetChild(i);

            child.GetChild(0).GetComponent<Image>().sprite = sprites[i];
            child.GetChild(1).GetComponent<Text>().text = Environment.ToppingNameList[i];
            child.GetChild(2).GetComponent<Text>().text = toppingCounts[i] + "개";
            
            // TODO:: 스태틱 사용하지 않기
            if (ToppingSpawner.isOTopping[i])
            {
                Text toppingScoreText = child.GetChild(3).GetComponent<Text>();
                toppingScoreText.text = "+" + toppingCounts[i] * Environment.InfiniteOToppingScore + "₩";
                toppingScoreText.color = Environment.ColorOToppingScore();
            } else {
                Text toppingScoreText = child.GetChild(3).GetComponent<Text>();
                toppingScoreText.text = toppingCounts[i] * Environment.InfiniteXToppingScore + "₩";
                toppingScoreText.color = Environment.ColorXToppingScore();
            }
        }

        userData.SaveClearData(score);
        txtClearPanelBestScore.text = userData.maxScore + "₩";

    }

    public void GameOver()
    {
        StopGame();
        
        overBestScore.text = userData.maxScore + "₩";
        overTotalScore.text = score + "₩";
        
        overReason.text = score < 1 ? "맛없는 피자가 되었어요 ㅠㅠ" : "따라오던 토핑과 부딪혔어요 ㅠㅠ";
        
        overPanel.SetActive(true);
    }
}