using UnityEngine;

public class GameCheckerFactory: MonoBehaviour
{
    [Header("Prefabs")] 
    public GameObject infiniteClearPanel;
    public GameObject stageClearPanel;
    
    public GameChecker GetGameCheckerByMode(Environment.GameMode gameMode, Environment.StageMode stageMode, Transform canvas)
    {
        GameChecker gameChecker = null;

        if (gameMode == Environment.GameMode.INFINITE)
        {
            gameChecker = gameObject.AddComponent<InfiniteGameManager>();
            gameChecker.clearPanelPrefab = infiniteClearPanel;
        }
        else if (gameMode == Environment.GameMode.STORY)
        {
            switch (stageMode)
            {
                case Environment.StageMode.ORIGINAL:
                    gameChecker = gameObject.AddComponent<OriginalStageGameManager>();
                    break;
                // case Environment.StageMode.AVOID:
                    // gameChecker = obj.AddComponent<Avoi
                default:
                    gameChecker = gameObject.AddComponent<OriginalStageGameManager>();
                    break;
            }
            
            gameChecker.clearPanelPrefab = stageClearPanel;
        }
        else
        {
            gameChecker = gameObject.AddComponent<InfiniteGameManager>();
            gameChecker.clearPanelPrefab = infiniteClearPanel;
        }

        gameChecker.canvas = canvas;

        return gameChecker;
    }
}