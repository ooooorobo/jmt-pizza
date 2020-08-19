using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidToppingSpawner : DefaultToppingSpawner
{
    public GameObject obstacle;

    public override void SpawnObstacle(int obsCount)
	{
        Debug.Log("spawn");
        for (int i = 0; i < obsCount; i++)
		{
            Vector3 position = FindPosition();
            GameObject newObstacle = Instantiate(obstacle, position, Quaternion.identity);
        }
    }
}
