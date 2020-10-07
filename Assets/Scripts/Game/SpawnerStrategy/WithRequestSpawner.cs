using UnityEngine;

public class WithRequestSpawner : DefaultSpawner
{
    protected override void MakePool()
    {
        return;
    }

    public override void StartSpawn()
    {
        Debug.Log("Please give GameObject parameter");
    }

    public void StartSpawn(GameObject obj, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject temp = Instantiate(obj, new Vector3(-200, -200, 0), Quaternion.identity);
            temp.SetActive(false);

            SpawnObject(temp);
        }
    }
}
