using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffects : ObjectPool<TestEffects,EffectPool, Vector3>
{
    public float spawnArea=1f;
    public float spawnDeley;
    public int totalSpawnEnemyCount;
    public float removalDelay;
    public int currentSpawnEnemyCount;

    protected int m_currentSpawnEnemyCount;
    protected Coroutine m_SpawnTimerCoroutine;
    protected WaitForSeconds m_SpawnWait;

    private void Start()
    {

        for (int i = 0; i < initialCount; i++)
        {
            EffectPool newEnemy = CreateNewObject();
            items.Add(newEnemy);
        }

        for (int i = 0; i < totalSpawnEnemyCount; i++)
        {
            Pop(transform.position + transform.right * Random.Range(-spawnArea * 0.5f, spawnArea * 0.5f));
            m_currentSpawnEnemyCount++;
        }

        m_SpawnWait = new WaitForSeconds(spawnDeley);
    }

    public override void Push(EffectPool poolObject)
    {
        poolObject.inPool = true;
        m_currentSpawnEnemyCount--;
        poolObject.Sleep();

        StartSpawnTimer();
    }

    protected void StartSpawnTimer()
    {
        if (m_SpawnTimerCoroutine == null)
        {
            m_SpawnTimerCoroutine = StartCoroutine(SpawnTimer());
        }
    }

    protected IEnumerator SpawnTimer()
    {
        while (m_currentSpawnEnemyCount < 2)
        {
            yield return m_SpawnWait;
            Pop(transform.position + transform.right * Random.Range(-spawnArea * 5, spawnArea * 5));

            m_currentSpawnEnemyCount++;
        }

        m_SpawnTimerCoroutine = null;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea, 0.4f, 0f));
    }
}

public class EffectPool: PoolObject <TestEffects, EffectPool, Vector3>
{
    public Damabeable damageable;
    public EnemyBehaviour enemyBehaviour;

    protected WaitForSeconds m_RemoveWait;

    public override void WeakUp(Vector3 info)
    {
        instance.transform.position = info;
        damageable.SetHealth(damageable.startingHealth);
        damageable.DisableInerability();
        enemyBehaviour.ResetSpriteColor();
        instance.SetActive(true);
        ScenceLinkSMB<EnemyBehaviour>.Initialise(enemyBehaviour.GetComponentInChildren<Animator>(), enemyBehaviour);
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }

    public override void SetRefrence()
    {
        damageable = instance.GetComponent<Damabeable>();
        enemyBehaviour = instance.GetComponent<EnemyBehaviour>();

        
        damageable.OnDie.AddListener(ReturnToPoolEvent);

        m_RemoveWait = new WaitForSeconds(pool.removalDelay);
    }

    private void ReturnToPoolEvent(Damager damager,Damabeable damageable)
    {
        pool.StartCoroutine(ReturnToPoolAfterDelay());
    }

    protected IEnumerator ReturnToPoolAfterDelay()
    {
        yield return m_RemoveWait;
        ReturnToPool();
    }
}