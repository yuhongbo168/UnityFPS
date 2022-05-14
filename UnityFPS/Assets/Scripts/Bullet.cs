using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Damager))]
public class Bullet : MonoBehaviour
{
    public string VFXName;
    public bool destroyWhenOutOfView = true;
    public bool spriteOriginallyFacesLeft;
    public LayerMask explodeMask;
    private ContactFilter2D m_ExpoldeContactFilter;
    public bool canRangeAttack;
    public bool oneAttack=true;
    public float explodeRange = 4f;

    [HideInInspector]
    public Camera mianCamera;
    public float timeBeforAutodestruct = -1.0f;

    protected SpriteRenderer m_SpriteRenderer;

    [HideInInspector]
    public BulletObject bulletPoolObject;

    const float k_OffScreentError = 0.01f;

    protected float m_Timer;

    private void Awake()
    {
        m_ExpoldeContactFilter.layerMask = explodeMask;
        m_ExpoldeContactFilter.useLayerMask = true;
        m_ExpoldeContactFilter.useTriggers = false;
    }
    private void OnEnable()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Timer = 0.0f;
    }
    private void FixedUpdate()
    {
        Vector3 screenPoint = mianCamera.WorldToViewportPoint(transform.position);

        bool onScreen = screenPoint.z > 0 && screenPoint.x > -k_OffScreentError && screenPoint.x < 1 + k_OffScreentError &&
            screenPoint.y > -k_OffScreentError && screenPoint.y < 1 + k_OffScreentError;

        if (!onScreen)
        {
            ReturnToPool();
        }

        if (timeBeforAutodestruct>0)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer>timeBeforAutodestruct)
            {
                bulletPoolObject.ReturnToPool();
            }
        }
    }

    public void ReturnToPool()
    {
        bulletPoolObject.ReturnToPool();
    }

    public void OnHitDamageable(Damager origin,Damabeable damageable)
    {
        FindSurface(origin.lastHIt);
        if (canRangeAttack)
        {
            Explode(origin);
        }
       
        ReturnToPool();
      
    }

    public void OnHitNonDamageable(Damager origin)
    {
        FindSurface(origin.lastHIt);
        if (canRangeAttack)
        {
            Explode(origin);
        }   
        ReturnToPool();
    }

    protected void FindTargetSurface(Collider2D collider)
    {
        Vector3 forward = spriteOriginallyFacesLeft ? Vector3.left : Vector3.right;

        if (m_SpriteRenderer != null)
        {
            if (m_SpriteRenderer.flipX)
            {
                forward.x = -forward.x;
            }
        }

        VFXController.Instance.Trigger(VFXName, transform.position, 0, false, null, null);
    }

    protected void FindSurface(Collider2D collider)
    {
        Vector3 forward = spriteOriginallyFacesLeft ? Vector3.left : Vector3.right;

        if (m_SpriteRenderer != null)
        {
            if (m_SpriteRenderer.flipX)
            {
                forward.x = -forward.x;
            }
        }

        VFXController.Instance.Trigger(VFXName, transform.position, 0, false, null, null);
    }

    protected void Explode(Damager damager)
    {
        
        
            Collider2D[] other = Physics2D.OverlapCircleAll(transform.position, explodeRange, m_ExpoldeContactFilter.layerMask);

            for (int i = 0; i < other.Length; i++)
            {
                Damabeable damabeable = other[i].GetComponent<Damabeable>();
                if (damabeable != null)
                {
                                      
                    damabeable.TakeDamage(damager, damabeable);

                }
                
            }
        

    
    }
}
