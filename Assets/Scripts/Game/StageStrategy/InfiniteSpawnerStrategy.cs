using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteSpawnerStrategy : IDefaultSpawnerStrategy
{
    protected override void AddSpawnerComponent(GameObject obj)
    {
        AddToppingSpawner(obj);

        WithDestroySpawner coinSpawner = obj.AddComponent<WithDestroySpawner>();
        coinSpawner.spawnObject = coin;
        periodicSpawners.Add(coinSpawner);
    }
}
