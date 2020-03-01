using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Sprite[] floorTiles;
    public Sprite[] toppingSprites;
    public GameObject player;
    public GameObject floor;
    public GameObject topping;


    public Transform board;

    public int columns = 11;
    public int rows = 11;

    private Vector3 initialPoint;
    private float floorSize;
    public float FloorSize
    {
        get { return floorSize; }
    }

    public float toppingSpawnTime;

    public bool canSpawn = false;

    private void Awake()
    {
        floorSize = floor.GetComponent<BoxCollider2D>().size.x;
        initialPoint = (new Vector3(columns / 2, rows / 2, 0)
            - (columns % 2 == 0 ? new Vector3(0.5f, 0.5f, 0) : Vector3.zero))
            * (-1) * floorSize;

    }

    public IEnumerator Spawn(GameObject obj, float spawnTime)
    {
        while (canSpawn)
        {
            Vector3 pos = initialPoint + new Vector3(Random.Range(0, columns), Random.Range(0, rows), 0) * floorSize;
            Instantiate(obj, pos, Quaternion.identity);

            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void SetBoard()
    {
        board = new GameObject("Board").transform;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject tile = Instantiate(floor, initialPoint + new Vector3(x, y, 0) * floorSize, Quaternion.identity);
                tile.GetComponent<SpriteRenderer>().sprite = floorTiles[(x + y) % 2];
                tile.transform.SetParent(board);
            }
        }
    }

}
