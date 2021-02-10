using System.Collections;
using UnityEngine;

public class Player : Tail
{
    bool isMoving = true;
    float distance;
    float speed;
    float accel;
    float xLen;
    float yLen;
    Vector3 center;

    Vector3 direction;
    Vector3 preDirection;

    Transform Arrow;

    public GameObject Tail;
    public Tail last;
    private int tailCount;
    IEnumerator move;

    private void Start()
    {
        Arrow = transform.GetChild(0);
        move = MoveCoroutine();
    }

    public void Init(float distance, float speed, float accel, Vector3 center)
    {
        this.distance = distance;
        direction = new Vector3(0, 1f, 0);
        preDirection = direction;
        transform.position = center;

        this.speed = speed;
        this.accel = accel;
        this.center = center;

        int row = Environment.BoardRowCount;
        int column = Environment.BoardColumnCount;

        xLen = center.x + (column / 2) * distance + (distance / 2) * (column % 2 - 1);
        yLen = (row / 2) * distance + (distance / 2) * (row % 2 - 1);
    }

    public void SetDirection(string dir) {
        int x = dir[0] - '1';
        int y = dir[1] - '1';

        if (x != 0 || y != 0)
            if (x != 0)
            {
                Vector3 temp = new Vector3(1f, 0, 0) * x;
                if (direction == temp * -1 || direction == preDirection * -1)
                {
                    return;
                }
                else direction = temp;

                Arrow.localPosition = new Vector3(0.232f, 0, 0) * x;
                Arrow.rotation = Quaternion.Euler(0, 0, 90 * x * -1);
            }
            else
            {
                Vector3 temp = new Vector3(0, 1f, 0) * y;
                if (direction == temp * -1 || direction == preDirection * -1) return;
                else direction = temp;

                Arrow.localPosition = new Vector3(0, 0.232f, 0) * y;
                Arrow.rotation = Quaternion.Euler(0, 0, 90 * (y - 1));
            }
    }

    private void Update()
    {
        // For debug in PC
        int x = (int) Input.GetAxisRaw("Horizontal");
        int y = (int) Input.GetAxisRaw("Vertical");

        SetDirection((x + 1) + "" + (y + 1));
    }

    public void Stop()
    {
        isMoving = false;
        StopCoroutine(move);
    }

    public void StartMove()
    {
        isMoving = true;
        StartCoroutine(move);
    }

    public IEnumerator EnterOven()
	{
        int count = 0;
        Tail nowOven = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        while (count++ < tailCount + 1)
		{
            nowOven.OnEnterOven();

            if (nowOven.next != null)
			{
                nowOven = nowOven.next;
			}

            yield return new WaitForSeconds(speed);
        }

        yield return new WaitForSeconds(speed * 3);

        IGameManager.Instance().GameClear();
    }

    IEnumerator MoveCoroutine()
    {
        Vector3 tempPos;

        while (isMoving)
        {
            yield return new WaitForSeconds(speed);
            if (isMoving)
            {
                preDirection = direction;
                tempPos = transform.position + direction * distance;

                if (tempPos.x > center.x + xLen + 0.03)
                    Move(new Vector3(center.x - xLen, tempPos.y, 0));
                else if (tempPos.x < center.x - xLen - 0.03)
                    Move(new Vector3(center.x + xLen, tempPos.y, 0));
                else if (tempPos.y > center.y + yLen + 0.03)
                    Move(new Vector3(tempPos.x, center.y - yLen, 0));
                else if (tempPos.y < center.y - yLen - 0.03)
                    Move(new Vector3(tempPos.x, center.y + yLen, 0));
                else
                    Move(tempPos);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "Topping":
                IGameManager.Instance().GetTopping(col.GetComponent<Topping>());
                col.gameObject.SetActive(false);

                Tail newtail = Instantiate(Tail, prevPos, Quaternion.identity).GetComponent<Tail>();
                newtail.GetComponent<SpriteRenderer>().sprite = col.GetComponent<SpriteRenderer>().sprite;
                newtail.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                if (next == null) next = newtail;
                else last.next = newtail;
                last = newtail;

                tailCount++;

                speed *= accel;

                break;

            case "Tail":
                IGameManager.Instance().GameOver();

                break;

            case "Oven":
                StartCoroutine("EnterOven");
                break;
            case "Obstacle":
                IGameManager.Instance().GameOver();
                break;
            case "Coin":
                IGameManager.Instance().GetCoin(200);
                col.gameObject.SetActive(false);
                break;
        }
    }
}
