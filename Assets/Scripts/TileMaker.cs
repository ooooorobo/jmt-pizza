using UnityEngine;

public class TileMaker : MonoBehaviour
{

    int row, column;    // column이 세로 row가 가로
    float tileSize;
    public float TileSize { get { return tileSize; } }
    float startX, startY;

    public GameObject tile;
    public Sprite[] tileColors;


    public void MakeBoard(int column, int row)
    {
        this.row = row;
        this.column = column;

        tileSize = tile.GetComponent<BoxCollider2D>().size.x;

        // 좌로, 위로 얼마나 올라가서 starting point 잡을지 계산
        // row/column이 홀수일 경우, 2로 나눈 몫 만큼의 tilesize만 주면 됨
        // 그런데 짝수일 경우 2로 나눈 몫의 tilesize/2 만큼의 차이가 생기게 됨
        startX = (-1 * (column / 2) * tileSize) - (tileSize / 2) * (column % 2 - 1);
        startY = (row / 2) * tileSize + (tileSize / 2) * (row % 2 - 1);

        Transform tileParent = new GameObject("tileparent").transform;

        // tile을 row, column만큼 spawn
        for (int x = 0; x < column; x++)
        {
            for (int y = 0; y < row; y++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(startX + x * tileSize, startY - y * tileSize, 0), Quaternion.identity);
                newTile.GetComponent<SpriteRenderer>().sprite = tileColors[(x * column + y) % 2];
                newTile.transform.parent = tileParent;
            }
        }

        //tileParent.position += new Vector3(0, 0, 0);
    }
}