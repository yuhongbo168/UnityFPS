using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool:ObjectPool<BulletPool, BulletObject,Vector2>
{
   

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class BulletObject:PoolObject<BulletPool, BulletObject,Vector2>
{

    public Transform transform;
    public Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;
    public Bullet buller;

    public override void Sleep()
    {
        instance.SetActive(false);
    }

    public override void WeakUp(Vector2 newPos)
    {
        transform.position = newPos;
        instance.SetActive(true);
    }

    public override void SetRefrence()
    {
        
        transform = instance.transform;
        rigidbody2D = instance.GetComponent<Rigidbody2D>();
        spriteRenderer = instance.GetComponent<SpriteRenderer>();
        buller = instance.GetComponent<Bullet>();
        buller.bulletPoolObject = this;
        buller.mianCamera = Object.FindObjectOfType<Camera>();

    }
}
