﻿using System.Collections;
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

    private float spawnDelay;
    private Vector3 center;
    private float tileSize;

    // 주기적으로 오브젝트를 생성하는 스포너 (Topping, Coin 등)
    public List<DefaultSpawner> periodicSpawners = new List<DefaultSpawner>();
	
    // Oven, Obstacle 등 한 번 생성 후 사라지거나 하지 않고
    // 요청에 의해 생성되는 오브젝트 생성 위한 스포너 
    public WithRequestSpawner requestSpawner;
	
    public void InitFactory(float spawnDelay, Transform centerPosition, float tileSize)
    {
        this.spawnDelay = spawnDelay;
        this.center = centerPosition.position;
        this.tileSize = tileSize;
    }

    public void CreateSpawner(GameObject obj)
    {
        requestSpawner = obj.AddComponent<WithRequestSpawner>();
		
        ToppingSpawner toppingSpawner = obj.AddComponent<ToppingSpawner>();
        toppingSpawner.spawnObject = topping;
        toppingSpawner.toppingSprites = toppingSprites;
        periodicSpawners.Add(toppingSpawner);

        WithDestroySpawner coinSpawner = obj.AddComponent<WithDestroySpawner>();
        coinSpawner.spawnObject = coin;
        periodicSpawners.Add(coinSpawner);

        InitPeriodicSpawners(); 
    }

    private void InitPeriodicSpawners()
    {
        foreach (DefaultSpawner spawner in  periodicSpawners)
        {
            spawner.InitSpawner(spawnDelay, center, tileSize);
        }
    }

    public void StartPeriodicSpawn()
    {
        foreach (DefaultSpawner spawner in  periodicSpawners)
        {
            spawner.StartSpawn();
        }
    }
	
    public void StopPeriodicSpawn()
    {
        foreach (DefaultSpawner spawner in  periodicSpawners)
        {
            spawner.Stop();
        }
    }

    public void RequestSpawn(RequestEnum request)
    {
        switch (request)
        {
            case RequestEnum.OVEN:
                requestSpawner.StartSpawn(oven);
                break;
            case RequestEnum.OBSTACLE:
                requestSpawner.StartSpawn(obstacle);
                break;
        }
    }


}