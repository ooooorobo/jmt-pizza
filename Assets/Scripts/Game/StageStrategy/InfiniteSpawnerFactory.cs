using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteSpawnerFactory : IDefaultSpawnerFactory
{
    protected override void AddSpawnerComponent(GameObject obj)
    {
        requestSpawner = obj.AddComponent<WithRequestSpawner>();
        requestSpawner.InitSpawner(spawnDelay, center, tileSize);
        
        ToppingSpawner toppingSpawner = obj.AddComponent<ToppingSpawner>();
        toppingSpawner.spawnObject = topping;
        toppingSpawner.toppingSprites = toppingSprites;
        periodicSpawners.Add(toppingSpawner);

        WithDestroySpawner coinSpawner = obj.AddComponent<WithDestroySpawner>();
        coinSpawner.spawnObject = coin;
        periodicSpawners.Add(coinSpawner);
    }
}
