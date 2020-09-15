using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Topping : MonoBehaviour
{
    public int id;
    public bool isO;
    public bool isCheese;
    public float blinkTime = 2f;
    private SpriteRenderer sprite;
    private float blinkDelay = 0.3f;

    private Animator effectAnimator;

    private void Start ()
	{
        sprite = GetComponent<SpriteRenderer>();
        effectAnimator = transform.GetChild(0).GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public IEnumerator Delay(float destroyDelay)
    {
        effectAnimator.SetTrigger("Created");
        
        yield return new WaitForSeconds(destroyDelay - blinkTime);

        if (gameObject.activeInHierarchy)
            StartCoroutine("Blink");
    }

    public IEnumerator Blink ()
	{
        float dt = 0;
        bool isTrans = false;
        Color tempColor = sprite.color;

        while (dt < blinkTime)
		{
            dt += blinkDelay;

            if (isTrans)
                tempColor.a = 1f;
            else
                tempColor.a = 0.6f;

            isTrans = !isTrans;
            sprite.color = tempColor;

            yield return new WaitForSeconds(blinkDelay);
		}

        tempColor.a = 1f;
        sprite.color = tempColor;

        this.gameObject.SetActive(false);
    }
}