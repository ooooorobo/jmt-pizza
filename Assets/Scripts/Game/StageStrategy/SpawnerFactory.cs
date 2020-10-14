using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RequestEnum
{
    OVEN, OBSTACLE
}
public class SpawnerFactory : MonoBehaviour
{
    public IDefaultSpawnerStrategy GetSpawnerStrategyByMode(GameObject obj, StageMode stageMode)
    {
        switch (stageMode)
        {
            case StageMode.ORIGINAL:
                return obj.AddComponent<InfiniteSpawnerStrategy>();
            case StageMode.AVOID:
                return obj.AddComponent<AvoidSpawnerStrategy>();
            case StageMode.CLEAN_DUST:
                return obj.AddComponent<CleanSpawnerStrategy>();
            default:
                return obj.AddComponent<InfiniteSpawnerStrategy>();
        }
    }
}