using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Damager : MonoBehaviour
{

    [System.Serializable]
    public class DamagableEvent : UnityEvent<Damager,Damabeable>
    { }

    [System.Serializable]
    public class NonDamagableEvent : UnityEvent<Damager>
    { }

    public int damage = 1;

    public bool offsetBasedOnSpriteFacing = true;
    public Vector2 offset = new Vector2(1.5f, 1f);
    public Vector2 size = new Vector2(2.5f, 1f);
    public SpriteRenderer spriteRenderer;
    public bool canHitTriggers;
    public LayerMask hittableLayers;
    public bool ignoreInvincibility = false;

    public DamagableEvent OnDamageableHit;
    public NonDamagableEvent OnNonDamageableHit;

    protected Transform m_DamagerTransform;
    protected bool m_SpriteOriginallyFlipped;
    protected ContactFilter2D m_AttackContactFilter;
    protected Collider2D[] m_AttackOverlapResults = new Collider2D[10];
    protected Collider2D m_LastHit;

    private void Awake()
    {

        m_AttackContactFilter.layerMask = hittableLayers;
        m_AttackContactFilter.useLayerMask = true;
        m_AttackContactFilter.useTriggers = canHitTriggers;
     
        if (offsetBasedOnSpriteFacing && spriteRenderer != null)
        {
            m_SpriteOriginallyFlipped = spriteRenderer.flipX;
        }

        m_DamagerTransform = transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 scale = m_DamagerTransform.lossyScale;

        Vector2 facingOffset = Vector2.Scale(offset, scale);
        if (offsetBasedOnSpriteFacing && spriteRenderer !=null && spriteRenderer.flipX != m_SpriteOriginallyFlipped)
        {
            facingOffset = new Vector2(-offset.x * scale.x, offset.y * scale.y);
        }

        Vector2 scaledSize = Vector2.Scale(size, scale);

        Vector2 pointA = (Vector2)m_DamagerTransform.position + facingOffset - scaledSize * 0.5f;
        Vector2 pointB = pointA + scaledSize;

        int hitCount = Physics2D.OverlapArea(pointA, pointB,m_AttackContactFilter,m_AttackOverlapResults);

        for (int i = 0; i < hitCount; i++)
        {
            m_LastHit = m_AttackOverlapResults[i];
            Damabeable damageable = m_LastHit.GetComponent<Damabeable>();

            if (damageable)
            {
                OnDamageableHit.Invoke(this, damageable);
                damageable.TakeDamage(this, ignoreInvincibility);
            }
            else
            {
                OnNonDamageableHit.Invoke(this);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
