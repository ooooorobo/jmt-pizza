using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IToppingSpawner
{
     void InitSpawner(float spawnDelay, float destroyDelay, Vector3 center, float tileSize, int maxXTopping);
     void MakePool();
     void MakeOven();
     void Stop();
     void StartSpawn();
     bool CheckPosition(Vector3 pos);
}