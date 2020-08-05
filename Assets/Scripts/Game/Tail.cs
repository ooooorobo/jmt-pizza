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
}
