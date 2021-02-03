using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSpawner : MonoBehaviour
{
    public int poolSize = 20;

    int row, column;
    float tileSize;
    Vector3 center;

    public GameObject spawnObject;
    private Transform parentObject;

    public List<GameObject> pool;

    private float spawnDelay;
    
    private bool canMake = true;
    private int poolTail = 0;

    IEnumerator spawnCoroutine;
    
    public void InitSpawner(float spawnDelay, Vector3 center, float tileSize)
    {
        InitValue(spawnDelay, center, tileSize);

        pool = new List<GameObject>();
        parentObject = new GameObject("spawnParent").transform;
        parentObject.position = center;

        MakePool();
        InitMore();
    }

    protected virtual void InitMore()
    {
        return;
    }
    
    public void InitValue(float spawnDelay, Vector3 center, float tileSize)
    {
        this.row = Environment.BoardRowCount;
        this.column = Environment.BoardColumnCount;
        this.tileSize = tileSize;
        this.center = center;

        this.spawnDelay = spawnDelay;

        spawnCoroutine = SpawnCoroutine();
    }
    
    public virtual void StartSpawn() 
    {
        canMake = true;
        if (spawnCoroutine == null)
            spawnCoroutine = SpawnCoroutine();
        StartCoroutine(spawnCoroutine);
    }

    public void Stop()
    {
        canMake = false;
        StopCoroutine(spawnCoroutine);
    }

    protected virtual void MakePool()
    {
        int count = 0;

        while (count++ < poolSize)
        {
            GameObject newObj = Instantiate(spawnObject, new Vector3(-100, -100, 0), Quaternion.identity);

            newObj.transform.parent = parentObject;
            newObj.name = "object" + count * (poolTail + 1);
            newObj.GetComponent<BoxCollider2D>().enabled = false;
            InitPoolObject(newObj);
            
            pool.Add(newObj);
        }
    }

    protected virtual void InitPoolObject(GameObject obj)
    {
        return;
    }

    private Vector3 FindPosition(int count)
    {
        if (count > 10) return new Vector3(-200, -200, 0);

        float x = center.x + Random.Range(0, column) * tileSize;
        float y = center.y + Random.Range(0, row) * tileSize * -1;

        y += (row / 2) * tileSize + (tileSize / 2) * (row % 2 - 1);
        x -= ((column / 2) * tileSize + (tileSize / 2) * (column % 2 - 1));

        Vector3 pos = new Vector3(x, y, 0);

        if (CheckPosition(pos)) return pos;
        
        return FindPosition(count + 1);
    }
    private bool CheckPosition(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, transform.forward, 20f);
        
        if (hit) return false;
        
        return true;
    }

    protected void SpawnObject(GameObject obj)
    {
        Vector3 newPosition = FindPosition(0);

        obj.transform.position = newPosition;
        
        obj.SetActive(true);
        obj.GetComponent<BoxCollider2D>().enabled = true;

        InitSpawnedObject(obj);
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(spawnDelay);

        while (canMake)
        {
            GameObject temp = pool[poolTail++ % pool.Count].gameObject;

            if (!temp.activeInHierarchy)
            {
                SpawnObject(temp);
            }
            else
            {
                MakePool();
            }
            
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    protected virtual void InitSpawnedObject(GameObject obj)
    {
        return;
    }
}