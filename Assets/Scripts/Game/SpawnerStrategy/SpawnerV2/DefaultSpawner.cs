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

        MakePool();
        InitMore();
    }

    protected virtual void InitMore()
    {
        return;
    }
    
    public void InitValue(float spawnDelay, Vector3 center, float tileSize)
    {
        this.row = GameManager.Instance().row;
        this.column = GameManager.Instance().column;
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
            // newObj.SetActive(false);
            newObj.GetComponent<BoxCollider2D>().enabled = false;
            
            pool.Add(newObj);
        }
    }

    private Vector3 FindPosition()
    {
        float x = center.x + Random.Range(0, row) * tileSize;
        float y = center.y + Random.Range(0, column) * tileSize * -1;

        y += (column / 2) * tileSize + (tileSize / 2) * (column % 2 - 1);
        x -= ((row / 2) * tileSize + (tileSize / 2) * (row % 2 - 1));

        Vector3 pos = new Vector3(x, y, 0);

        if (CheckPosition(pos)) return pos;
        
        return FindPosition();
    }
    private bool CheckPosition(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, transform.forward, 20f);
        
        if (hit) return false;
        
        return true;
    }

    protected void SpawnObject(GameObject obj)
    {
        Vector3 newPosition = FindPosition();

        obj.transform.position = newPosition;
        
        InitSpawnedObject(obj);
        
        obj.SetActive(true);
        obj.GetComponent<BoxCollider2D>().enabled = true;
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
