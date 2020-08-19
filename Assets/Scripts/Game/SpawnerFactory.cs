using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFactory
{
    public static void CreateSpawner(StageMode mode, GameObject obj)
	{
        switch(mode)
		{
			case StageMode.ORIGINAL:
				obj.AddComponent<DefaultToppingSpawner>();
				break;
			case StageMode.AVOID:
				obj.AddComponent<AvoidToppingSpawner>();
				break;
			default:
				obj.AddComponent<DefaultToppingSpawner>();
				break;
		}
	}
}
