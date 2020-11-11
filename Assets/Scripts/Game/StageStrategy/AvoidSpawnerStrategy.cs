using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidSpawnerStrategy : IDefaultSpawnerStrategy
{
    protected override void AddSpawnerComponent(GameObject obj)
    {
        AddToppingSpawner(obj);
    }
}
