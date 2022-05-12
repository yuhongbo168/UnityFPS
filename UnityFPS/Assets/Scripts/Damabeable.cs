using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class Damabeable : MonoBehaviour
{

    [System.Serializable]
    public class DamageEvent:UnityEvent<Damager,Damabeable>
    { }

    [System.Serializable]
    public class DamageaBleEvent: UnityEvent<Damabeable> { }

    public float invulnerabilityDuration = 3f;

    public DamageEvent OnTakeDamage;
    public DamageEvent OnDie;
    public DamageaBleEvent OnSetHealth;

    public Vector2 centreOffset = new Vector2(0f, 1f);
    protected Vector2 m_DamageDirection;

    public int startingHealth = 5;

    protected int m_CurrentHealth;
    protected bool m_Invulnerable;
    protected float m_InulnerabilityTimer;
    protected bool m_ResetHealthOnSceneReload;

    public int CurrentHealth
    {
        get
        {
            return m_CurrentHealth;
        }
    }
    private void OnEnable()
    {
        m_CurrentHealth = startingHealth;
    }

    private void Update()
    {
        if (m_Invulnerable)
        {
            m_InulnerabilityTimer -= Time.deltaTime;

            if (m_InulnerabilityTimer <= 0f)
            {
                m_Invulnerable = false;
            }
        }
    }

    public Vector2 GetDamageDirection()
    {
        return m_DamageDirection;
    }
 
    public void EnableInvuInerability(bool ignoreTimer = false)
    {
        m_Invulnerable = true;
        m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
    }

    public void DisableInerability()
    {
        m_Invulnerable = false;
    }
    public void TakeDamage(Damager damager,bool ignoreInvincible = false)
    {

        if (m_Invulnerable||m_CurrentHealth<=0)
        {
            return;
        }

        if (!m_Invulnerable)
        {
            m_CurrentHealth -= damager.damage;
 
        }

        m_DamageDirection = transform.position + (Vector3)centreOffset - damager.transform.position;

        OnTakeDamage.Invoke(damager, this);
   
        if (m_CurrentHealth <= 0)
        {
            OnDie.Invoke(damager, this);
            m_ResetHealthOnSceneReload = true;
            EnableInvuInerability();
            
        }
    }

    public void SetHealth(int amount)
    {
        m_CurrentHealth = amount;

        if (m_CurrentHealth <= 0)
        {
            OnDie.Invoke(null, this);
            m_ResetHealthOnSceneReload = true;
            EnableInvuInerability();
        }
    }

    public void AddHealth(int amount)
    {
        m_CurrentHealth = (m_CurrentHealth + amount) > startingHealth ? startingHealth : m_CurrentHealth + amount;
    }
}
