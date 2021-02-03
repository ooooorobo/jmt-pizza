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
    
    [Header("Positions")]
    public RectTransform oToppingPosition;
    public RectTransform xToppingPosition;

    public int maxXCount;
    
    public Sprite[] toppingSprites;
    
    public IDefaultSpawnerStrategy GetSpawnerStrategyByMode(GameObject obj, Environment.StageMode stageMode)
    {
        IDefaultSpawnerStrategy strategy;
        
        switch (stageMode)
        {
            case Environment.StageMode.ORIGINAL:
                strategy = gameObject.AddComponent<InfiniteSpawnerStrategy>();
                break;
            case Environment.StageMode.AVOID:
                strategy = gameObject.AddComponent<AvoidSpawnerStrategy>();
                break;
            case Environment.StageMode.CLEAN_DUST:
                strategy = gameObject.AddComponent<CleanSpawnerStrategy>();
                break;
            default:
                strategy = gameObject.AddComponent<InfiniteSpawnerStrategy>();
                break;
        }

        strategy.InitSpawnPrefabs(topping, coin, oven, obstacle, toppingSprites, oToppingPosition, xToppingPosition, maxXCount);
        return strategy;
    }
}