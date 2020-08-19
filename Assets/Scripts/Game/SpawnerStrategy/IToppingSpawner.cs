using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IToppingSpawner
{
     void InitSpawner(float spawnDelay, float destroyDelay, Vector3 center, float tileSize, int maxXTopping, int obstacleCount);
     void InitValue(float spawnDelay, float destroyDelay, Vector3 center, float tileSize, int maxXTopping, int obstacleCount);
     void InitOToppingList();
     void MakePool();
     void MakeOven();
     void Stop();
     void StartSpawn();
     void SpawnObstacle(int obsCount);

     bool CheckPosition(Vector3 pos);
}