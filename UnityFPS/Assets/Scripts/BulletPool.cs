using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool:ObjectPool<BulletPool, BulletObject>
{
   

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class BulletObject:PoolObject<BulletPool, BulletObject>
{
    public override void Sleep()
    {
        instance.SetActive(false);
    }

    public override void WeakUp()
    {
        instance.SetActive(true);
    }
}
