using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool canMove = false;
    public float speed;
    private float distance;
    private Vector3 direction = new Vector3(0, 1f, 0);
    public Vector3 nextPosition;
    private int col, row;

    IEnumerator checkDirCoroutine;
    IEnumerator moveCoroutine;

    public void InitPlayer(float distance, int col, int row)
    {
        this.distance = distance;
        canMove = true;

        this.col = col; this.row = row;
        nextPosition = new Vector3(col / 2, row / 2, 0) + (col % 2 == 1 ? new Vector3(1, 1, 0) : Vector3.zero);

        checkDirCoroutine = CheckDirection();
        moveCoroutine = Move();

        StartCoroutine(checkDirCoroutine);
        StartCoroutine(moveCoroutine);
    }

    IEnumerator CheckDirection()
    {
        while (canMove)
        {
            if (Input.GetKey("up"))
            {
                direction.x = 0f;
                direction.y = 1f;
            }
            else if (Input.GetKey("down"))
            {
                direction.x = 0f;
                direction.y = -1f;
            }
            else if (Input.GetKey("left"))
            {
                direction.x = -1f;
                direction.y = 0;
            }
            else if (Input.GetKey("right"))
            {
                direction.x = 1f;
                direction.y = 0;
            }

            yield return null;
        }
    }

    IEnumerator Move()
    {
        WaitForSeconds wait = new WaitForSeconds(speed);
        Vector3 temp = Vector3.zero;
        bool isReachEnd = false;

        while (canMove)
        {
            nextPosition += direction;

            if (nextPosition.x < 1 || nextPosition.x > col)
            {
                isReachEnd = true;
                temp = direction;
                direction *= (col - 1) * -1;
                nextPosition += direction;
            }
            else if (nextPosition.y < 1 || nextPosition.y > row)
            {
                isReachEnd = true;
                temp = direction;
                direction *= (row - 1) * -1;
                nextPosition += direction;
            }

            transform.position += direction * distance;

            if (isReachEnd)
            {
                direction = temp;
                nextPosition -= direction;
                isReachEnd = false;
            }

            yield return wait;
        }
    }
}
