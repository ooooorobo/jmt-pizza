using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RequestEnum
{
    OVEN, OBSTACLE
}
public class SpawnerFactory : MonoBehaviour
{
    public GameObject topping;
    public GameObject coin;
    public GameObject oven;
    public GameObject obstacle;

    public Sprite[] toppingSprites;
    
    public IDefaultSpawnerStrategy GetSpawnerStrategyByMode(GameObject obj, StageMode stageMode)
    {
        IDefaultSpawnerStrategy strategy;
        
        switch (stageMode)
        {
            case StageMode.ORIGINAL:
                strategy = gameObject.AddComponent<InfiniteSpawnerStrategy>();
                break;
            case StageMode.AVOID:
                strategy = gameObject.AddComponent<AvoidSpawnerStrategy>();
                break;
            case StageMode.CLEAN_DUST:
                strategy = gameObject.AddComponent<CleanSpawnerStrategy>();
                break;
            default:
                strategy = gameObject.AddComponent<InfiniteSpawnerStrategy>();
                break;
        }

        strategy.InitSpawnPrefabs(topping, coin, oven, obstacle, toppingSprites);
        return strategy;
    }
}