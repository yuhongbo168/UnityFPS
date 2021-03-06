using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool:ObjectPool<BulletPool, BulletObject,Vector2>
{

    static protected Dictionary<GameObject, BulletPool> s_PoolInstance = new Dictionary<GameObject, BulletPool>();

    private void Awake()
    {
        if (prefab!=null&&!s_PoolInstance.ContainsKey(prefab))
        {
            s_PoolInstance.Add(prefab, this);
        }
    }
    static public BulletPool GetObjectPool(GameObject prefab,int initialPoolCount = 10)
    {
        BulletPool objPool = null;
        if (!s_PoolInstance.TryGetValue(prefab,out objPool))
        {
            GameObject obj = new GameObject(prefab.name + "_Pool");
            objPool = obj.AddComponent<BulletPool>();
            objPool.prefab = prefab;
            objPool.initialCount = initialPoolCount;

            s_PoolInstance[prefab] = objPool;
        }
        return objPool;
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
