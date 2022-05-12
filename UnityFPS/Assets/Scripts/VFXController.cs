using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
public class VFX
{
    public GameObject prefab;
    public float lifetime = 1;

    [HideInInspector]
    public VFXInstancePool pool;
}

public class VFXInstance:PoolObject<VFXInstancePool,VFXInstance>,System.IComparable<VFXInstance>
{
    public float expires;
    public ParticleSystem[] particleSystem;
    public Transform transform;
    public Transform parent;

    public override void SetRefrence()
    {
        transform = instance.transform;
        particleSystem = instance.GetComponentsInChildren<ParticleSystem>();
    }

    public int CompareTo(VFXInstance other)
    {
        return expires.CompareTo(other.expires);
    }

    public void SetPosition(Vector3 position)
    {
        //transform.localPosition = position;
        transform.position = position;
    }

    public override void Sleep()
    {
        for (int i = 0; i < particleSystem.Length; i++)
        {
            particleSystem[i].Stop();
        }
        instance.SetActive(false);
    }

    public override void WeakUp()
    {
        instance.SetActive(true);
        for (int i = 0; i < particleSystem.Length; i++)
        {
            particleSystem[i].Play();
        }

    }

}
public class VFXInstancePool:ObjectPool<VFXInstancePool,VFXInstance>
{

}

public class VFXController : MonoBehaviour
{

    public static VFXController Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            instance = FindObjectOfType<VFXController>();

            if (instance != null)
            {
                return instance;
            }

            return CreateInstance();

        }
    }

    protected static VFXController CreateInstance()
    {
        VFXController newInstance = Resources.Load<VFXController>("VFXController");
        instance = Instantiate(newInstance);
        return instance;
    }


    public VFX[] vfxconfig;

    Dictionary<int, VFX> m_FxPools = new Dictionary<int, VFX>();
    PriorityQueue<VFXInstance> m_RunningFX = new PriorityQueue<VFXInstance>();

    protected static VFXController instance;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (var vfx in vfxconfig)
        {
            vfx.pool = gameObject.AddComponent<VFXInstancePool>();
            vfx.pool.initialCount = 2;
            vfx.pool.prefab = vfx.prefab;


            m_FxPools[StringToHash(vfx.pool.prefab.name)] = vfx;
        }


    }

    public void Trigger(string name, Vector3 position, float startDelay, bool flip, Transform parent, TileBase tileOverride = null)
    {
        Trigger(StringToHash(name), position, startDelay, flip, parent);
    }

    public void Trigger(int hash, Vector3 position, float startDelay, bool flip, Transform parent, TileBase tileOverride = null)
    {
        VFX vfx;

        if (!m_FxPools.TryGetValue(hash, out vfx))
        {
            Debug.LogError("VFX does not exist.");
        }
        else
        {
            CreateInstance(vfx, position, flip,parent, tileOverride);
        }
    }

    void CreateInstance(VFX vfx, Vector3 position, bool flip, Transform parent, TileBase tileOverride)
    {
        VFXInstancePool poolTouse = null;

        poolTouse = vfx.pool;

        var instance = poolTouse.Pop();

        instance.expires = Time.time + vfx.lifetime;

        if (flip)
        {
            instance.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            instance.transform.localScale = new Vector3(1, 1, 1);
        }
        instance.parent = parent;
        instance.SetPosition(position);
        m_RunningFX.Push(instance);
       
    }



    // Update is called once per frame
    void Update()
    {
        while (!m_RunningFX.Empty && m_RunningFX.First.expires <=Time.time)
        {
            var instance = m_RunningFX.Pop();
            instance.pool.Push(instance);
        }

        var instances = m_RunningFX.items;
        for (int i = 0; i < instances.Count; i++)
        {
            var vfx = instances[i];
            if (vfx.parent!=null)
            {
                vfx.transform.position = vfx.parent.position;
            }
        }
    }

    public static int StringToHash(string name)
    {
        return name.GetHashCode();
    }
}
