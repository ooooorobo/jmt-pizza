
using UnityEngine;
using UnityEngine.UI;

public class OriginalStageGameManager: GameChecker
{
    private StageLoader stageLoader;
    public override void InitGame()
    {
        clearPanel = Instantiate(clearPanelPrefab).transform;
        clearPanel.SetParent(canvas);
        clearPanel.localScale = new Vector3(1, 1, 1);

        stageLoader = StageLoader.Instance();
        
        gameManager = IGameManager.Instance();
        
        spawnerFactory = GetComponent<SpawnerFactory>()
            .GetSpawnerStrategyByMode(gameObject, Environment.StageMode.ORIGINAL);
        spawnerFactory.InitFactory(Environment.InfiniteToppingSpawnDelay, gameManager.centerPosition, gameManager.tileSize,
            stageLoader.cntXTopping);
        spawnerFactory.AttachSpawner(gameObject);
        gameManager.spawnerFactory = spawnerFactory;
        
        gameManager.txtScoreInfo.text = "목표 점수 " + stageLoader.minScore + "₩";
        
        ChangeGauge(null);
    }

    public override void CheckGameClear()
    {
        if (gameManager.score >= stageLoader.minScore && !gameManager.ovenOpened)
        {
            gameManager.ovenOpened = true;
            spawnerFactory.RequestSpawn(RequestEnum.OVEN, 1);
        }
    }

    public override void CheckGameOver()
    {
        if (gameManager.score < Environment.InfiniteScoreMinimum)
        {
            gameManager.GameOver();
        }
    }

    public override void SetGameClearPanelUi()
    {
        clearPanel.gameObject.SetActive(true);

        Button mainBtn = clearPanel.GetChild(3).GetComponent<Button>();
        Button restartBtn = clearPanel.GetChild(4).GetComponent<Button>();

        restartBtn.onClick.AddListener(() => MyStageManager.RestartWithStatic());
        mainBtn.onClick.AddListener(() => MyStageManager.LoadSceneWithStatic("StageSelect"));
    }

    public override void ChangeGauge(Topping t)
    {
        if (gameManager.addGaugeCoroutine != null)
            StopCoroutine(gameManager.addGaugeCoroutine);

        gameManager.addGaugeCoroutine = UIManager.FillAmount(gameManager.CheeseGauge,
            (float) gameManager.score / stageLoader.minScore, 0.0035f);
        StartCoroutine(gameManager.addGaugeCoroutine);
    }
}