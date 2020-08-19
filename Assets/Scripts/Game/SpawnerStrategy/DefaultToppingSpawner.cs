
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultToppingSpawner : MonoBehaviour, IToppingSpawner
{
    public int poolSize;

    int row, column;
    float tileSize;
    Vector3 center;

    public GameObject topping;
    public GameObject oven;
    public GameObject obstacle;
    public Sprite[] toppingSprites;

    private GameObject toppingParent;
    public Transform oParent;
    public Transform xParent;

    public List<Topping> toppingPool;
    public bool[] isOTopping;

    private int poolTail = 0;
    private float spawnDelay;
    private float destroyDelay;
    private bool canMake = true;
    private int maxXTopping;
    public int obstacleCount;

    private bool isOvenOpened = false;

    public void InitValue(float spawnDelay, float destroyDelay, Vector3 center, float tileSize, int maxXTopping, int obstacleCount)
	{
        this.row = GameManager.Instance().row;
        this.column = GameManager.Instance().column;
        this.tileSize = tileSize;
        this.center = center;

        this.spawnDelay = spawnDelay;
        this.destroyDelay = destroyDelay;
        this.maxXTopping = maxXTopping;
        this.obstacleCount = obstacleCount;
    }

    public void InitOToppingList()
	{
        for (int i = 0; i < isOTopping.Length; i++)
        {
            isOTopping[i] = true;
        }

        while (maxXTopping > 0)
        {
            int randIndex = Random.Range(1, isOTopping.Length);
            if (isOTopping[randIndex])
            {
                isOTopping[randIndex] = false;
                maxXTopping--;
            }
        }

    }

    public void InitSpawner(float spawnDelay, float destroyDelay, Vector3 center, float tileSize, int maxXTopping, int obstacleCount)
    {
        InitValue(spawnDelay, destroyDelay, center, tileSize, maxXTopping, obstacleCount);

        isOTopping = new bool[toppingSprites.Length];
        InitOToppingList();

        for (int i = 1; i < isOTopping.Length; i++)
        {
            // isOTopping[i] = (Random.Range(0, 2) == 1);

            GameObject oxTopping = Instantiate(topping, Vector3.zero, Quaternion.identity);

            if (isOTopping[i]) oxTopping.transform.parent = oParent;
            else oxTopping.transform.parent = xParent;

            oxTopping.transform.localPosition = new Vector3(0.4f, 0, 0) * oxTopping.transform.parent.childCount;
            oxTopping.GetComponent<SpriteRenderer>().sprite = toppingSprites[i];
        }

        toppingPool = new List<Topping>();
        toppingParent = new GameObject("toppingParent");

        this.obstacleCount = obstacleCount;
        SpawnObstacle(obstacleCount);

        MakePool();
    }

    public virtual void SpawnObstacle(int obsCount)
    {
        Debug.Log(obstacleCount);
        for (int i = 0; i < obstacleCount; i++)
        {
            Debug.Log(obstacleCount);
            Vector3 position = FindPosition();
            GameObject newObstacle = Instantiate(obstacle, position, Quaternion.identity);
        }
    }

    public void MakePool()
    {
        int count = 0;

        while (count++ < poolSize)
        {
            GameObject newTopping = Instantiate(topping, Vector3.zero, Quaternion.identity);
            newTopping.transform.parent = toppingParent.transform;
            toppingPool.Add(newTopping.GetComponent<Topping>());

            newTopping.SetActive(false);
        }
    }

    public void MakeOven() 
    {
        if (!isOvenOpened)
        {
            Vector3 position = FindPosition();
            GameObject ovenInstance = Instantiate(oven, position, Quaternion.identity);
            isOvenOpened = true;
        }
    }

    public void Stop()
    {
        canMake = false;
    }

    public void StartSpawn() 
    {
        canMake = true;
        StartCoroutine("MakeTopping", 2f);
    }

    public Vector3 FindPosition()
    {
        // TODO :: 레이캐스트 사용하여 랜덤 위치에 다른 토핑/머리/꼬리 등등 존재하지 않는지 확인한 후에 생성해야 함
        float x = center.x + Random.Range(0, row) * tileSize;
        float y = center.y + Random.Range(0, column) * tileSize * -1;

        y += (column / 2) * tileSize + (tileSize / 2) * (column % 2 - 1);
        x += -((row / 2) * tileSize + (tileSize / 2) * (row % 2 - 1));

        Vector3 pos = new Vector3(x, y, 0);

        if (CheckPosition(pos)) return pos;
        else return FindPosition();
    }

    public bool CheckPosition(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, transform.forward, 20f);
        if (hit) return false;
        else return true;
    }

    IEnumerator MakeTopping()
    {
        while (canMake)
        {
            GameObject temp = toppingPool[poolTail % toppingPool.Count].gameObject;

            if (!temp.activeInHierarchy)
            {
                temp.transform.position = FindPosition();
                int index = Random.Range(0, toppingSprites.Length);
                temp.GetComponent<SpriteRenderer>().sprite = toppingSprites[index];
                temp.GetComponent<Topping>().isO = isOTopping[index];
                temp.GetComponent<Topping>().id = index;

                if (index == 0) temp.GetComponent<Topping>().isCheese = true;
                else temp.GetComponent<Topping>().isCheese = false;
                
                temp.SetActive(true);
                poolTail++;

                StartCoroutine(temp.GetComponent<Topping>().Delay(destroyDelay));
            }
            else
            {
                MakePool();
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
