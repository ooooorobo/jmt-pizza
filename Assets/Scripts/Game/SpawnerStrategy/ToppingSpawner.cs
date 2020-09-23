using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingSpawner : DefaultSpawner
{
    public Sprite[] toppingSprites;
    private bool[] isOTopping;
    private int maxXTopping;

    protected override void InitMore()
    {
        isOTopping = new bool[toppingSprites.Length];
        InitOToppingList();
    }

    public void InitOToppingList()
    {
        for (int i = 0; i < isOTopping.Length; i++)
        {
            isOTopping[i] = true;
        }

        while (maxXTopping > 0)
        {
            int randIndex = Random.Range(1, isOTopping.Length);
            if (isOTopping[randIndex])
            {
                isOTopping[randIndex] = false;
                maxXTopping--;
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
        obj.SetActive(true);
        top.SetDelay(5f);
    }
}
