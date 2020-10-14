using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDefaultSpawnerFactory : MonoBehaviour
{
    public GameObject topping;
    public GameObject coin;

    public GameObject oven;
    public GameObject obstacle;

    public Sprite[] toppingSprites;

    protected float spawnDelay;
    protected Vector3 center;
    protected float tileSize;

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

    public void AttachSpawner(GameObject obj)
    {
        AddSpawnerComponent(obj);

        InitPeriodicSpawners();
    }

    protected virtual void AddSpawnerComponent(GameObject obj)
    {
        
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
    
    public void RequestSpawn(RequestEnum request, int amount)
    {
        switch (request)
        {
            case RequestEnum.OVEN:
                requestSpawner.StartSpawn(oven, amount);
                break;
            case RequestEnum.OBSTACLE:
                requestSpawner.StartSpawn(obstacle, amount);
                break;
        }
    }
}
