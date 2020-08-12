using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        Arrow = transform.GetChild(0);
    }

    public void Init(float distance, float speed, float accel, Vector3 center)
    {
        this.distance = distance;
        direction = new Vector3(0, 1f, 0);
        preDirection = direction;

        this.speed = speed;
        this.accel = accel;
        this.center = center;

        int row = GameManager.Instance().row;
        int column = GameManager.Instance().column;

        xLen = center.x + (row / 2) * distance + (distance / 2) * (row % 2 - 1);
        yLen = (column / 2) * distance + (distance / 2) * (column % 2 - 1);
    }

    public void SetDirection(string dir) {
        int x = dir[0] - '1';
        int y = dir[1] - '1';
        Debug.Log(x+""+y);
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
    }

    public void StratMove()
    {
        isMoving = true;
        StartCoroutine("Move", speed);
    }

    IEnumerator Move()
    {
        Vector3 tempPos;

        while (isMoving)
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

            yield return new WaitForSeconds(speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "Topping":
                GameManager.Instance().ChangeScore(col.GetComponent<Topping>());
                col.gameObject.SetActive(false);

                Tail newtail = Instantiate(Tail, prevPos, Quaternion.identity).GetComponent<Tail>();
                newtail.GetComponent<SpriteRenderer>().sprite = col.GetComponent<SpriteRenderer>().sprite;

                if (next == null) next = newtail;
                else last.next = newtail;
                last = newtail;

                speed -= accel;

                break;

            case "Tail":
                GameManager.Instance().GameOver();
                break;

            case "Oven":
                GameManager.Instance().GameClear();
                break;
        }
    }
}
