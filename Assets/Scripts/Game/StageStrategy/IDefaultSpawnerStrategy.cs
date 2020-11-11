using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDefaultSpawnerStrategy : MonoBehaviour
{
    public GameObject topping;
    public GameObject coin;

    public GameObject oven;
    public GameObject obstacle;

    public Sprite[] toppingSprites;

    protected float spawnDelay;
    protected float tileSize;
    protected Vector3 center;
    protected int maxXCount;

    protected RectTransform oToppingPosition;
    protected RectTransform xToppingPosition;

    // 주기적으로 오브젝트를 생성하는 스포너 (Topping, Coin 등)
    public List<DefaultSpawner> periodicSpawners = new List<DefaultSpawner>();

    // Oven, Obstacle 등 한 번 생성 후 사라지거나 하지 않고
    // 요청에 의해 생성되는 오브젝트 생성 위한 스포너 
    public WithRequestSpawner requestSpawner;

    public void InitSpawnPrefabs(GameObject topping, GameObject coin, GameObject oven, GameObject obstacle,
        Sprite[] toppingSprites, RectTransform oToppingPosition, RectTransform xToppingPosition, int xToppingCount)
    {
        this.topping = topping;
        this.coin = coin;
        this.oven = oven;
        this.obstacle = obstacle;
        this.toppingSprites = toppingSprites;
        this.oToppingPosition = oToppingPosition;
        this.xToppingPosition = xToppingPosition;
        this.maxXCount = maxXCount;
    }
    
    public void InitFactory(float spawnDelay, Transform centerPosition, float tileSize, int maxXCount)
    {
        this.spawnDelay = spawnDelay;
        this.center = centerPosition.position;
        this.tileSize = tileSize;
        this.maxXCount = maxXCount;
    }

    public void AttachSpawner(GameObject obj)
    {
        requestSpawner = obj.AddComponent<WithRequestSpawner>();
        requestSpawner.InitSpawner(spawnDelay, center, tileSize);

        AddSpawnerComponent(obj);

        InitPeriodicSpawners();
    }

    protected virtual void AddSpawnerComponent(GameObject obj)
    {
        
    }

    protected virtual void AddToppingSpawner(GameObject obj)
    {
        ToppingSpawner toppingSpawner = obj.AddComponent<ToppingSpawner>();
        toppingSpawner.spawnObject = topping;
        toppingSpawner.toppingSprites = toppingSprites;
        toppingSpawner.oToppingPosition = oToppingPosition;
        toppingSpawner.xToppingPosition = xToppingPosition;
        toppingSpawner.maxXTopping = maxXCount;
        periodicSpawners.Add(toppingSpawner);
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
