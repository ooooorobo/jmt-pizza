using UnityEngine;
using UnityEngine.UI;

public class InfiniteGameManager: GameChecker
{
    public override void InitGame()
    {
        clearPanel = Instantiate(clearPanelPrefab).transform;
        clearPanel.SetParent(canvas.transform);
        clearPanel.localScale = new Vector3(1, 1, 1);
        
        gameManager = IGameManager.Instance();

        spawnerFactory = GetComponent<SpawnerFactory>()
            .GetSpawnerStrategyByMode(gameObject, Environment.StageMode.ORIGINAL);
        spawnerFactory.InitFactory(Environment.InfiniteToppingSpawnDelay, gameManager.centerPosition, gameManager.tileSize,
            Environment.InfiniteXToppingCount);
        spawnerFactory.AttachSpawner(gameObject);

        gameManager.spawnerFactory = spawnerFactory;

        gameManager.txtScoreInfo.text = "최고 점수 " + gameManager.userData.maxScore + "₩";
    }

    public override void CheckGameOver()
    {
        if (gameManager.score < Environment.InfiniteScoreMinimum)
        {
            gameManager.GameOver();
        }
    }

    public override void CheckGameClear()
    {
        if (gameManager.goalToppingCount == Environment.InfiniteTargetToppingGoalMin && !gameManager.ovenOpened)
        {
            gameManager.ovenOpened = true;
            spawnerFactory.RequestSpawn(RequestEnum.OVEN, 1);
        }
    }

    public override void SetGameClearPanelUi()
    {
        clearPanel.gameObject.SetActive(true);

        Text clearScoreTxt = clearPanel.GetChild(2).GetComponent<Text>();
        Text bestScoreTxt = clearPanel.GetChild(3).GetComponent<Text>();
        Transform resultList = clearPanel.GetChild(1);
        
        Button restartBtn = clearPanel.GetChild(4).GetComponent<Button>();
        Button mainBtn = clearPanel.GetChild(5).GetComponent<Button>();
         
        restartBtn.onClick.AddListener(() => MyStageManager.RestartWithStatic());
        mainBtn.onClick.AddListener(() => MyStageManager.LoadSceneWithStatic("Lobby"));
        
        clearScoreTxt.text = gameManager.score + "₩";
        
        // TODO:: Topping sprite, name 관리하는 무언가.. 만들기
        Sprite[] sprites = spawnerFactory.toppingSprites;

        int[] toppingCounts = gameManager.toppingCounts;

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

        bestScoreTxt.text = gameManager.userData.maxScore + "₩";

    }

    public override void ChangeGauge(Topping t)
    {
        if (t.isCheese)
        {
            gameManager.cheese++;

            if (gameManager.addGaugeCoroutine != null)
                StopCoroutine(gameManager.addGaugeCoroutine);

            gameManager.addGaugeCoroutine = UIManager.FillAmount(gameManager.CheeseGauge,
                (float) gameManager.cheese / Environment.InfiniteTargetToppingGoalMin, 0.0035f);
            StartCoroutine(gameManager.addGaugeCoroutine);
        }
    }
}