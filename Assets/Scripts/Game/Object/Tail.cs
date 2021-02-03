using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    protected Vector3 prevPos;
    public Tail next;

    public void Move(Vector3 nextPos)
    {
        prevPos = transform.position;
        if (next != null) next.Move(prevPos);
        transform.position = nextPos;
    }

    public void OnEnterOven()
	{
        Color col = GetComponent<SpriteRenderer>().color;
        col.a = 0;
        GetComponent<SpriteRenderer>().color = col;

        GetComponent<BoxCollider2D>().enabled = false;
    }
}
