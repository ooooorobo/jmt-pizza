using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool isMoving = true;
    float distance;
    float speed;
    Vector3 direction;

    Transform Arrow;

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
                transform.position = new Vector3(-maxX, tempPos.y, 0);
            else if (tempPos.x < -maxX - 0.03)
                transform.position = new Vector3(maxX, tempPos.y, 0);
            else if (tempPos.y > maxY + 0.03)
                transform.position = new Vector3(tempPos.x, -maxY, 0);
            else if (tempPos.y < -maxY - 0.03)
                transform.position = new Vector3(tempPos.x, maxY, 0);
            else
                transform.position = tempPos;

            yield return new WaitForSeconds(speed);
        }
    }
}
