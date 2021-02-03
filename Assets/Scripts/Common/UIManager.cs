using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    public static IEnumerator FillAmount(Image obj, float max, float step)
    {
        while (obj.fillAmount < max)
        {
            obj.fillAmount += step;

            yield return null;
        }
    }
}
