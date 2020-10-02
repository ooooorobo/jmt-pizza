using System.Collections;
using UnityEngine;

public class WithDestroySpawner: DefaultSpawner
{
    private IDestroy destroy;
    public float destroyDelay = 5f;
    
    protected override void InitPoolObject(GameObject obj)
    {
        destroy = obj.AddComponent<IDestroy>();
    }
    protected override void InitSpawnedObject(GameObject obj)
    {
        IEnumerator delayCoroutine = destroy.Delay(destroyDelay);
        StartCoroutine(delayCoroutine);
    }
}