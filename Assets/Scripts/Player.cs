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
        while (isMoving)
        {
            transform.position += direction * distance;

            yield return new WaitForSeconds(speed);
        }
    }
}
