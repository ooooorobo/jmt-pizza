using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Topping : MonoBehaviour
{
    public IEnumerator Delay(float destroyDelay)
    {
        yield return new WaitForSeconds(destroyDelay);

        this.gameObject.SetActive(false);
    }
}