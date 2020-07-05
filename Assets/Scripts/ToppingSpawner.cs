using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingSpawner : MonoBehaviour
{
    public GameObject topping;
    public Topping[] toppingPool;

    private int poolTail = 0;
    private float delay;
    private bool canMake = true;

    private int maxPool = 10;

    private void Awake()
    {
        GameObject toppingParent = new GameObject("toppingParent");
        toppingParent.transform.position = Vector3.zero;
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

    public void StartPooling(float delay)
    {
        this.delay = delay;

        StartCoroutine("MakeTopping", 2f);
    }

    public Vector3 FindPosition()
    {
        // TODO :: 레이캐스트 사용하여 랜덤 위치에 다른 토핑/머리/꼬리 등등 존재하지 않는지 확인한 후에 생성해야 함
        int row = GameManager.Instance().row;
        int column = GameManager.Instance().column;
        float tileSize = GameManager.Instance().tileSize;

        float x = Random.Range(0, row) * tileSize;
        float y = Random.Range(0, column) * tileSize * -1;

        x += (-1 * (column / 2) * tileSize) - (tileSize / 2) * (column % 2 - 1);
        y += (row / 2) * tileSize + (tileSize / 2) * (row % 2 - 1);

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
                temp.SetActive(true);
                poolTail++;
            }

            yield return new WaitForSeconds(delay);
        }
    }
}
