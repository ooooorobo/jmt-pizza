using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToppingSpawner : DefaultSpawner
{
    public Sprite[] toppingSprites;
    private bool[] isOTopping;
    public int maxXTopping;
    
    public RectTransform oToppingPosition;
    public RectTransform xToppingPosition;

    protected override void InitMore()
    {
        isOTopping = new bool[toppingSprites.Length];
        InitOToppingList();
    }

    public void InitOToppingList()
    {
        List<Image> images = new List<Image>();
        int oCount = 0;
        int xCount = 0;

        Vector3 localScale = new Vector3(0.7f, 0.7f, 1);
        Vector3 positionGap = new Vector3(0.35f, 0, 0);
        Vector3 basePosition = new Vector3(0.4f, 0, 0);
        
        for (int i = 0; i < isOTopping.Length; i++)
        {
            isOTopping[i] = true;

        }
        
        while (maxXTopping > 0)
        {
            int randIndex = Random.Range(1, isOTopping.Length);

            // X Topping
            if (isOTopping[randIndex])
            {
                isOTopping[randIndex] = false;
                maxXTopping--;
            }
        }

        // 치즈가 1번이므로 제외하고 시작
        for (int i = 1; i < isOTopping.Length; i++)
        {
            Image nowImage = new GameObject().AddComponent<Image>();
            images.Add(nowImage);
            
            nowImage.sprite = toppingSprites[i];
            nowImage.SetNativeSize();

            // O Topping
            if (isOTopping[i])
            {
                nowImage.rectTransform.SetParent(oToppingPosition);
                nowImage.rectTransform.position = basePosition + oToppingPosition.position + positionGap * oCount;
                nowImage.rectTransform.localScale = localScale;

                oCount++;
            }
            // X Topping
            else
            {
                nowImage.rectTransform.SetParent(xToppingPosition);
                nowImage.rectTransform.position = basePosition + xToppingPosition.position + positionGap * xCount;
                nowImage.rectTransform.localScale = localScale;

                xCount++;
            }

        }

    }

    protected override void InitSpawnedObject(GameObject obj)
    {
        int index = Random.Range(0, toppingSprites.Length);
        obj.GetComponent<SpriteRenderer>().sprite = toppingSprites[index];
        Topping top = obj.GetComponent<Topping>();
        top.isO = isOTopping[index];
        top.id = index;
        top.isCheese = index == 0;
        top.SetDelay(5f);
    }
}
