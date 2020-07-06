using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingSpawner : MonoBehaviour
{
    int row, column;
    float tileSize;
    public GameObject topping;
    public Topping[] toppingPool;
    public Sprite[] toppingSprites;

    private int poolTail = 0;
    private float spawnDelay;
    private float destroyDelay;
    private bool canMake = true;

    private int maxPool = 200;

    private void Awake()
    {
        GameObject toppingParent = new GameObject("toppingParent");
        toppingPool = new Topping[maxPool];

        int count = 0;

        while (count++ < maxPool)
        {
            GameObject newTopping = Instantiate(topping, Vector3.zero, Quaternion.identity);
            newTopping.transform.parent = toppingParent.transform;
            toppingPool[count - 1] = newTopping.GetComponent<Topping>();

            newTopping.SetActive(false);
        }
    }

    public void StartPooling(float spawnDelay, float destroyDelay)
    {
        this.row = GameManager.Instance().row;
        this.column = GameManager.Instance().column;
        this.tileSize = GameManager.Instance().tileSize;

        this.spawnDelay = spawnDelay;
        this.destroyDelay = destroyDelay;

        StartCoroutine("MakeTopping", 2f);
    }

    public void Stop()
    {
        canMake = false;
    }

    public Vector3 FindPosition()
    {
        // TODO :: 레이캐스트 사용하여 랜덤 위치에 다른 토핑/머리/꼬리 등등 존재하지 않는지 확인한 후에 생성해야 함
        float x = Random.Range(0, row) * tileSize;
        float y = Random.Range(0, column) * tileSize * -1;

        y += (column / 2) * tileSize + (tileSize / 2) * (column % 2 - 1);
        x += -((row / 2) * tileSize + (tileSize / 2) * (row % 2 - 1));

        return new Vector3(x, y, 0);
    }

    IEnumerator MakeTopping()
    {
        while (canMake)
        {
            GameObject temp = toppingPool[poolTail % maxPool].gameObject;

            if (!temp.activeInHierarchy)
            {
                temp.transform.position = FindPosition();
                temp.GetComponent<SpriteRenderer>().sprite = toppingSprites[Random.Range(0, toppingSprites.Length)];
                temp.SetActive(true);
                poolTail++;

                StartCoroutine(temp.GetComponent<Topping>().Delay(destroyDelay));
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
