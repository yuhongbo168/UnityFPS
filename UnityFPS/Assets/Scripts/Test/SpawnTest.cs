using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : ObjectPool<SpawnTest,SpawnPool,Vector3>
{
    public float spawnArea=2;
    public int totalEnemyToBeSpawn;
    public int currntEnemyToBeSpawn;
    public float spawnDelay;

    private int m_TotalEnemyToBeSpawn;
    private int m_CurrentEnemyToBeSpawn;
    private Coroutine m_SpawnTimerCoroutine;

    private WaitForSeconds m_SpawnWait;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < initialCount; i++)
        {
            SpawnPool enemy = CreateNewObject();
            items.Add(enemy);
        }

        int spawnCount = Mathf.Min((totalEnemyToBeSpawn - m_TotalEnemyToBeSpawn), currntEnemyToBeSpawn);

        for (int i = 0; i < spawnCount; i++)
        {
            Pop(transform.position + transform.right * Random.Range(-spawnArea * 0.5f, spawnArea * 0.5f));
        }

        m_CurrentEnemyToBeSpawn = spawnCount;
        m_TotalEnemyToBeSpawn += currntEnemyToBeSpawn;
        m_SpawnWait = new WaitForSeconds(spawnDelay);

    }

    public override void Push(SpawnPool poolObject)
    {
        poolObject.inPool = true;
        m_CurrentEnemyToBeSpawn--;
        poolObject.Sleep();
        StartSpawnTimer();
    }

    protected void StartSpawnTimer()
    {
        if (m_SpawnTimerCoroutine==null)
        {
            StartCoroutine(SpawnTimer());
        }
        
    }

    protected IEnumerator SpawnTimer()
    {
        while (m_CurrentEnemyToBeSpawn < currntEnemyToBeSpawn && m_TotalEnemyToBeSpawn < totalEnemyToBeSpawn)
        {
            yield return m_SpawnWait;
            Pop(transform.position + transform.right * Random.Range(-spawnArea * 0.5f, spawnArea * 0.5f));
            m_CurrentEnemyToBeSpawn++;
            m_TotalEnemyToBeSpawn++;
        }

        m_SpawnTimerCoroutine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea, 0.2f, 0));
    }
}

public class SpawnPool:PoolObject<SpawnTest, SpawnPool,Vector3>
{
    public EnemyBehaviour enemyBehaviour;
    public Damabeable damabeable;

    protected WaitForSeconds m_RemoveWait;

    public override void Sleep()
    {
        instance.SetActive(false);
    }

    public override void WeakUp(Vector3 info)
    {
        instance.transform.position = info;
        damabeable = instance.GetComponent<Damabeable>();
        damabeable.SetHealth(damabeable.startingHealth);
        instance.SetActive(true);
        damabeable.DisableInerability();
        
        ScenceLinkSMB<EnemyBehaviour>.Initialise(enemyBehaviour.GetComponentInChildren<Animator>(), enemyBehaviour);

        
    }

    public override void SetRefrence()
    {
        enemyBehaviour = instance.GetComponent<EnemyBehaviour>();
        damabeable = instance.GetComponent<Damabeable>();

        damabeable.OnDie.AddListener(ReturnToPoolEvent);

        m_RemoveWait = new WaitForSeconds(pool.spawnDelay);
    }

    protected void ReturnToPoolEvent(Damager damager,Damabeable damabeabel)
    {
        pool.StartCoroutine(ReturnToPoolAfterDelay());
    }

    protected IEnumerator ReturnToPoolAfterDelay()
    {
        yield return m_RemoveWait;
        ReturnToPool();
    }
}
