using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ObjectPool<TPool, TObject,TInfo>: ObjectPool<TPool, TObject>
    where TPool : ObjectPool<TPool, TObject,TInfo>
    where TObject : PoolObject<TPool, TObject,TInfo>, new()
{
    private void Start()
    {
        for (int i = 0; i < initialCount; i++)
        {
            TObject newPoolObject = CreateNewObject();
            items.Add(newPoolObject);

        }

    }

    public virtual TObject Pop(TInfo info)
    {
       
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].inPool)
            {
                items[i].inPool = false;
                items[i].WeakUp(info);
                return items[i];
            }
        }

        TObject newObject = CreateNewObject();
        items.Add(newObject);
        newObject.inPool = false;
        newObject.WeakUp(info);
        return newObject;

    }



}
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
        newitem.SetRefrence(this as TPool);
        newitem.Sleep();
        return newitem;
    }

    public virtual TObject Pop()
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
        items.Add(newitem);
        newitem.WeakUp();
        return newitem;
    }

    public virtual void Push(TObject poolObject)
    {
        poolObject.inPool = true;
        poolObject.Sleep();
    }

}

public abstract class PoolObject<TPool,TObject,TInfo>:PoolObject<TPool, TObject>
    where TPool : ObjectPool<TPool, TObject, TInfo>
    where TObject : PoolObject<TPool, TObject, TInfo>, new()
{
    public virtual void WeakUp(TInfo info)
    {

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

    public void SetRefrence(TPool newpool)
    {
        pool = newpool;
        SetRefrence();
    }

    public virtual void SetRefrence()
    {
        
    }

    public virtual void ReturnToPool()
    {
        TObject thisOjbect = this as TObject;
        pool.Push(thisOjbect);
    }
}
