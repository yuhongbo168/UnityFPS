using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool:ObjectPool<BulletPool, BulletObject,Vector3>
{
   

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class BulletObject:PoolObject<BulletPool, BulletObject,Vector3>
{

    public Transform transform;
    public Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;
    public Bullet buller;

    public override void Sleep()
    {
        instance.SetActive(false);
    }

    public override void WeakUp(Vector3 newPos)
    {
        transform.position = newPos;
        instance.SetActive(true);
    }

    public override void SetRefrence()
    {
        transform = instance.transform;
        rigidbody2D = instance.GetComponent<Rigidbody2D>();
        spriteRenderer = instance.GetComponent<SpriteRenderer>();
        buller = instance.AddComponent<Bullet>();
        buller.mianCamera = Object.FindObjectOfType<Camera>();
        
    }
}
