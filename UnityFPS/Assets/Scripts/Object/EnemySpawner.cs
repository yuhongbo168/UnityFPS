using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : ObjectPool<EnemySpawner, Enemy, Vector2>
{
    public int totalEnemiesToBeSpawned;
    public int concurrentEnemiesToBeSpawned;
    public float spawnArea = 1.0f;
    public float spawnDelay;
    public float removalDelay;
    // public DataSettings dataSettings;

    protected int m_TotalSpawnedEnemyCount;
    protected int m_CurrentSpawnedEnemyCount;
    protected Coroutine m_SpawnTimerCooutine;
    protected WaitForSeconds m_SpawnWait;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    

    private void Start()
    {

        for (int i = 0; i < initialCount; i++)
        {
            Enemy newEnemy = CreateNewObject();
            items.Add(newEnemy);
        }

        int spawnCount = Mathf.Min(totalEnemiesToBeSpawned - m_TotalSpawnedEnemyCount, concurrentEnemiesToBeSpawned);

        for (int i = 0; i < spawnCount; i++)
        {
            Pop(transform.position + transform.right * Random.Range(-spawnArea * 0.5f, spawnArea * 0.5f));
        }

        m_CurrentSpawnedEnemyCount = spawnCount;
        m_TotalSpawnedEnemyCount += concurrentEnemiesToBeSpawned;
        m_SpawnWait = new WaitForSeconds(spawnDelay);

    }

    public override void Push(Enemy poolObject)
    {
        poolObject.inPool = true;
        m_CurrentSpawnedEnemyCount--;
        poolObject.Sleep();
        StartSpawnTimer();
    }

    protected void StartSpawnTimer()
    {
        if (m_SpawnTimerCooutine == null)
        {
            m_SpawnTimerCooutine = StartCoroutine(SpawnTimer());
        }
    }

    protected IEnumerator SpawnTimer()
    {
        while (m_CurrentSpawnedEnemyCount < concurrentEnemiesToBeSpawned && m_TotalSpawnedEnemyCount < totalEnemiesToBeSpawned)
        {
            yield return m_SpawnWait;
            Pop(transform.position + transform.right * Random.Range(-spawnArea * 0.5f, spawnArea * 0.5f));
            m_CurrentSpawnedEnemyCount++;
            m_TotalSpawnedEnemyCount++;
        }

        m_SpawnTimerCooutine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea, 0.4f, 0));
    }

}

public class Enemy : PoolObject<EnemySpawner, Enemy, Vector2>
{
    public Damabeable damageable;
    public EnemyBehaviour enemyBehaviour;

    protected WaitForSeconds m_RemoveWait;
    public override void SetRefrence()
    {
        damageable = instance.GetComponent<Damabeable>();
        enemyBehaviour = instance.GetComponent<EnemyBehaviour>();

        damageable.OnDie.AddListener(ReturnToPoolEvent);

        m_RemoveWait = new WaitForSeconds(pool.removalDelay);
    }

    public override void WeakUp(Vector2 info)
    {
        enemyBehaviour.SetMoveVector(Vector2.zero);
        instance.transform.position = info;
        instance.SetActive(true);
        damageable.SetHealth(damageable.startingHealth);
        damageable.DisableInerability();
        enemyBehaviour.contactDamager.EnableDamage();
        ScenceLinkSMB<EnemyBehaviour>.Initialise(enemyBehaviour.GetComponentInChildren<Animator>(), enemyBehaviour);
           
    }

    public override void Sleep()
    {
        instance.SetActive(false);
        damageable.EnableInvuInerability();
    }

    protected void ReturnToPoolEvent(Damager dmgr,Damabeable dmgbl)
    {
        pool.StartCoroutine(ReturnToPoolAfterDelay());
    }

    protected IEnumerator ReturnToPoolAfterDelay()
    {
        yield return m_RemoveWait;
        ReturnToPool();
    }
}

