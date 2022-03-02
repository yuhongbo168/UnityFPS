using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damabeable : MonoBehaviour
{

    public int startingHealth = 5;

    protected int m_CurrentHealth;
    // Start is called before the first frame update

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(Damager damager,bool ignoreInvincible = false)
    {
        m_CurrentHealth -= damager.damage;
    }
}
