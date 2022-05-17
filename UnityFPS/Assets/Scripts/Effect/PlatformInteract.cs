using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInteract : MonoBehaviour
{
    public bool disableCollision;
    private BoxCollider2D m_Collistion2D;
    private Animator m_Animator;
    private PlatformEffector2D m_PlatformEffector2D;

    private readonly int m_HashDisabledPapa = Animator.StringToHash("Disabled");
    
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Collistion2D = GetComponent<BoxCollider2D>();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            if (m_Animator != null)
            {
                if (disableCollision)
                {
                    m_Animator.SetBool(m_HashDisabledPapa, true);
                }
               
            }
            
        }
    }


    public void DisabledCollistion()
    {
        Debug.Log(m_Collistion2D);

        if (m_Collistion2D!=null)
        {
            
            Debug.Log(m_Collistion2D);
            m_Collistion2D.enabled = false;
        }
    }
 
    public void EnabledCollision()
    {
       
        if (m_Collistion2D != null)
        {
            m_Collistion2D.enabled = true;
            m_Animator.SetBool(m_HashDisabledPapa, false);
        }
    }

    
}
