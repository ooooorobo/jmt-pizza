using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tail
{
    bool isMoving = true;
    float distance;
    float speed;
    Vector3 direction;

    Transform Arrow;

    public GameObject Tail;
    public Tail last;

    private void Start()
    {
        Arrow = transform.GetChild(0);
    }

    public void Init(float distance, float speed)
    {
        this.distance = distance;
        direction = new Vector3(0, 1f, 0);

        this.speed = speed;

        StartCoroutine("Move");
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x != 0 || y != 0)
            if (x != 0)
            {
                direction = new Vector3(1f, 0, 0) * x;
                Arrow.localPosition = new Vector3(0.35f, 0, 0) * x;
                Arrow.rotation = Quaternion.Euler(0, 0, 90 * x * -1);
            }
            else
            {
                direction = new Vector3(0, 1f, 0) * y;
                Arrow.localPosition = new Vector3(0, 0.35f, 0) * y;
                Arrow.rotation = Quaternion.Euler(0, 0, 90 * (y - 1));
            }
    }

    public void GameOver()
    {
        isMoving = false;
    }

    IEnumerator Move()
    {
        int row = GameManager.Instance().row;
        int column = GameManager.Instance().column;

        float maxX = (row / 2) * distance + (distance / 2) * (row % 2 - 1);
        float maxY = (column / 2) * distance + (distance / 2) * (column % 2 - 1);

        Vector3 tempPos;

        while (isMoving)
        {
            tempPos = transform.position + direction * distance;

            if (tempPos.x > maxX + 0.03)
                Move(new Vector3(-maxX, tempPos.y, 0));
            else if (tempPos.x < -maxX - 0.03)
                Move(new Vector3(maxX, tempPos.y, 0));
            else if (tempPos.y > maxY + 0.03)
                Move(new Vector3(tempPos.x, -maxY, 0));
            else if (tempPos.y < -maxY - 0.03)
                Move(new Vector3(tempPos.x, maxY, 0));
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
                col.gameObject.SetActive(false);
                Tail newtail = Instantiate(Tail, prevPos, Quaternion.identity).GetComponent<Tail>();
                newtail.GetComponent<SpriteRenderer>().sprite = col.GetComponent<SpriteRenderer>().sprite;

                if (next == null) next = newtail;
                else last.next = newtail;
                last = newtail;
                break;

            case "Tail":
                GameManager.Instance().GameOver();
                break;
        }
    }
}
