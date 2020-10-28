using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidSpawnerStrategy : IDefaultSpawnerStrategy
{
    protected override void AddSpawnerComponent(GameObject obj)
    {
        ToppingSpawner toppingSpawner = obj.AddComponent<ToppingSpawner>();
        toppingSpawner.spawnObject = topping;
        toppingSpawner.toppingSprites = toppingSprites;
        periodicSpawners.Add(toppingSpawner);
        
        
    }
}
