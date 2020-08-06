using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingSpawner : MonoBehaviour
{
    public int poolSize;

    int row, column;
    float tileSize;
    Vector3 center;
    public GameObject topping;
    public GameObject oven;
    private GameObject toppingParent;
    public Transform oParent;
    public Transform xParent;
    public List<Topping> toppingPool;
    public Sprite[] toppingSprites;
    public bool[] isOTopping;

    private int poolTail = 0;
    private float spawnDelay;
    private float destroyDelay;
    private bool canMake = true;


    public void InitSpawner(float spawnDelay, float destroyDelay, Vector3 center, float tileSize)
    {
        this.row = GameManager.Instance().row;
        this.column = GameManager.Instance().column;
        this.tileSize = tileSize;
        this.center = center;

        this.spawnDelay = spawnDelay;
        this.destroyDelay = destroyDelay;

        isOTopping = new bool[toppingSprites.Length];
        isOTopping[0] = true; // cheese is always true

        for (int i = 1; i < isOTopping.Length; i++)
        {
            isOTopping[i] = (Random.Range(0, 2) == 1);

            GameObject oxTopping = Instantiate(topping, Vector3.zero, Quaternion.identity);

            if (isOTopping[i]) oxTopping.transform.parent = oParent;
            else oxTopping.transform.parent = xParent;

            oxTopping.transform.localPosition = new Vector3(0.4f, 0, 0) * oxTopping.transform.parent.childCount;
            oxTopping.GetComponent<SpriteRenderer>().sprite = toppingSprites[i];
        }

        toppingPool = new List<Topping>();
        toppingParent = new GameObject("toppingParent");

        MakePool();
    }

    private void MakePool()
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
        Vector3 position = FindPosition();
        GameObject ovenInstance = Instantiate(oven, position, Quaternion.identity);
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
