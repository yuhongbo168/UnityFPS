using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<TPool,TObject> : MonoBehaviour
    where TPool:ObjectPool<TPool,TObject>
    where TObject:PoolObject<TPool,TObject>,new()
{

    public GameObject prefab;
    public int initialCount = 10;

    public List<TObject> items = new List<TObject>();
    // Start is called before the first frame update
    void Start()
    {

        
        for (int i = 0; i < initialCount; i++)
        {
            TObject newitem = CreateNewObject();
            items.Add(newitem);
        }
        
    }

    public TObject CreateNewObject()
    {
        TObject newitem = new TObject();
        newitem.instance = Instantiate(prefab);
        newitem.instance.transform.SetParent(transform);
        newitem.inPool = true;
        newitem.Sleep();
        return newitem;
    }

    public TObject Pop()
    {
        
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].inPool)
            {
                items[i].inPool = false;
                items[i].WeakUp();
                return items[i];
            }
        }

        TObject newitem = CreateNewObject();
        return newitem;
    }

   
}

public abstract class PoolObject<TPool,TObject>
    where TPool : ObjectPool<TPool, TObject>
    where TObject : PoolObject<TPool, TObject>, new()
{
    public GameObject instance;
    public bool inPool;

    public TPool pool;

    public virtual void Sleep()
    {

    }

    public virtual void WeakUp()
    {

    }
}
