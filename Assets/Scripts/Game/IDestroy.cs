using System.Collections;
using UnityEngine;

public class IDestroy: MonoBehaviour
{
    public float blinkTime = 2f;
    public float blinkDelay = 0.3f;
    
    public IEnumerator Delay(float destroyDelay)
    {
        yield return new WaitForSeconds(destroyDelay - blinkTime);

        if (gameObject.activeInHierarchy)
            StartCoroutine("Blink");
    }
    
    private IEnumerator Blink()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
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

        gameObject.SetActive(false);
    }
}