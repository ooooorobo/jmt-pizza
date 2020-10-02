using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Topping : IDestroy
{
    public int id;
    public bool isO;
    public bool isCheese;
    private SpriteRenderer sprite;

    private Animator effectAnimator;

    private void Start ()
	{
        sprite = GetComponent<SpriteRenderer>();
        effectAnimator = transform.GetChild(0).GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void SetDelay(float destoryDelay)
    {
        effectAnimator.SetTrigger("Created");

        IEnumerator delayCoroutine = Delay(destoryDelay);
        StartCoroutine(delayCoroutine);
    }
}