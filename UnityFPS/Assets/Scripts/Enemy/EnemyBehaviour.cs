using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Collider2D))]
public class EnemyBehaviour : MonoBehaviour
{
    static Collider2D[] s_ColliderCache = new Collider2D[16];

    public string bloodName;
    public Color hitColor;
    public CapsuleCollider2D capsuleCollider;
    public Transform shootingPos;
    private BulletPool m_bulletPool;
    public float shootSpeed = 400f;
    public Bullet projectilePrefab;
   

    protected Coroutine m_FlickeringCoroutine;
    public List<Color> m_OriginalColor;

    public SpriteRenderer[] sprites;

    public float flickeringDuration;
   
    public Transform enemyTransform;
    private readonly Vector3 localScale = new Vector3(-1, 1, 1);

    public bool filp;
    public Transform target
    {
        get { return m_Target; }
    }

    [Header("Movement")]
    public float speed;
    public float gravity = 10.0f;

    [Header("FindTarget")]
    public float viewDirection = 0.0f;
    public float viewDistance;



    [Range(0.0f,360f)]
    public float viewFov=20f;
    public float meleeRange = 3.0f;

    public float timeBeforeTargetLost = 3.0f;

    public bool spriteFaceLeft = false;

    protected ContactFilter2D m_Filter;

    protected Bounds m_LocalBounds;

    protected SpriteRenderer m_SpriteRenderer;
    protected CharacterController m_CharacterController;
    protected Collider2D m_Collider;
    public Animator m_Animator;

    public CoinItem coinItem;


    public Damager meleeDamager;
    public Damager contactDamager;

    protected Transform m_Target;

    protected Vector2 m_SpriteForward;

    protected Vector2 m_moveVector;
    protected Vector3 m_TargetShootPosition;

    protected float m_TimeSinceLastTargetView;

    protected readonly int m_HashSpottedPara = Animator.StringToHash("Spotted");
    protected readonly int m_HashShootingPara = Animator.StringToHash("Shooting");
    protected readonly int m_HashTargetLostPara = Animator.StringToHash("TargetLost");
    protected readonly int m_HashMeleeAttackPara = Animator.StringToHash("MeleeAttack");
    protected readonly int m_HashHitPara = Animator.StringToHash("Hit");
    protected readonly int m_HashDeathPara = Animator.StringToHash("Death");
    protected readonly int m_HashGroundedPara = Animator.StringToHash("Grounded");

    public Vector3 moveVector
    {
        get { return m_moveVector; }
    }
    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Collider = GetComponent<Collider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
      
        m_SpriteForward = filp ? Vector2.left : Vector2.right;

        if (filp)
        {
            enemyTransform.localScale = localScale;
            //m_SpriteForward = -m_SpriteForward;
        }

        if (projectilePrefab!=null)
        {
            m_bulletPool = BulletPool.GetObjectPool(projectilePrefab.gameObject, 8);
        }

        sprites = GetComponentsInChildren<SpriteRenderer>();

        foreach (var item in sprites)
        {
            m_OriginalColor.Add(item.color);
        }

    }
    private void OnEnable()
    {
        m_Collider.enabled = true;
    }

    private void Start()
    {

        ScenceLinkSMB<EnemyBehaviour>.Initialise(m_Animator, this);

        m_LocalBounds = new Bounds();
        int count = m_CharacterController.rd.GetAttachedColliders(s_ColliderCache);

        for (int i = 0; i < count; i++)
        {
            m_LocalBounds.Encapsulate(transform.InverseTransformBounds(s_ColliderCache[i].bounds));
        }

        m_Filter = new ContactFilter2D();
        m_Filter.layerMask = m_CharacterController.groundedLayerMask;
        m_Filter.useLayerMask = true;
        m_Filter.useTriggers = false;

        if (meleeDamager!=null)
        {
            meleeDamager.DisableDamage();
        }
        
    }

    private void FixedUpdate()
    {    
        //m_moveVector.y = Mathf.Max(m_moveVector.y - gravity * Time.deltaTime, -gravity);

        m_CharacterController.Move(m_moveVector * Time.deltaTime);

        m_CharacterController.CheckCapsuleEndCollisions();

        UpdateTimers();

        m_Animator.SetBool(m_HashGroundedPara, m_CharacterController.IsGrounded);

//         if (m_CharacterController.IsGrounded)
//         {
//             m_CharacterController.rd.gravityScale=0.1f;
//         }
//         else
//         {
//             m_CharacterController.rd.gravityScale = 2f;
//         }     

    }

    public void GroundedVerticalMovement()
    {
        m_moveVector.y = Mathf.Max(m_moveVector.y - gravity * Time.deltaTime, -gravity);
    }

    public void UpdateJump()
    {
        m_moveVector.y -= 40 * Time.deltaTime;
    }

    public bool CheckForObstacle(float forwardDistance)
    {
        if (Physics2D.CircleCast(m_Collider.bounds.center,m_Collider.bounds.extents.y - 0.5f,m_SpriteForward,1f,m_Filter.layerMask.value))
        {          
            return true;
        }

        Vector3 castingPosition = (Vector2)(transform.position + m_LocalBounds.center) + m_SpriteForward * m_LocalBounds.extents.x;
        Debug.DrawLine(castingPosition, castingPosition + Vector3.down * (m_LocalBounds.extents.y + 0.2f));

        if (!Physics2D.CircleCast(castingPosition,0.18f,Vector2.down,m_LocalBounds.extents.y + 0.3f,m_CharacterController.groundedLayerMask.value))
        {
            return true;
        }

        return false;
    }

    void UpdateTimers()
    {
        if (m_TimeSinceLastTargetView > 0.0f)
        {
            m_TimeSinceLastTargetView -= Time.deltaTime;
        }
    }
    public void SetHorizontalSpeed(float horizontalSpeed)
    {
        m_moveVector.x = horizontalSpeed * m_SpriteForward.x;
    }

    public void UpdateFacing()
    {
        bool faceLeft = m_moveVector.x < 0f;
        bool faceRight = m_moveVector.x > 0f;

        if (faceLeft)
        {
            SetFacingData(-1);
        }

        if (faceRight)
        {
            SetFacingData(1);
        }
    }

    public void RemeberTargetPos()
    {
        if (m_Target == null)
        {
            return;
        }

        m_TargetShootPosition = m_Target.transform.position;
        
    }
    public void SetFacingData(int facing)
    {
        if (facing == -1)
        {
            //             m_SpriteRenderer.flipX = !spriteFaceLeft;
            //             m_SpriteForward = spriteFaceLeft ? Vector2.right : Vector2.left;

            filp = false;
            enemyTransform.localScale = localScale;
            m_SpriteForward = Vector2.left;
        }
        else if (facing == 1)
        {
            //m_SpriteRenderer.flipX = spriteFaceLeft;
            //m_SpriteForward = spriteFaceLeft ? Vector2.left : Vector2.right;
            filp = true;
            enemyTransform.localScale = Vector3.one;
            m_SpriteForward = Vector2.right;
        }
    }
    public void ScanForPlayer()
    {
        Vector3 dir = Character.PlayerCharacter.transform.position - transform.position;

        if (dir.sqrMagnitude > viewDistance * viewDistance)
        {
            return;
        }

        Vector3 testForward = Quaternion.Euler(0, 0, spriteFaceLeft ? Mathf.Sign(m_SpriteForward.x) * -viewDirection : Mathf.Sign(m_SpriteForward.x) * viewDirection) * m_SpriteForward;
        
        float angle = Vector3.Angle(testForward, dir);

        if (angle > viewFov * 0.5f)
        {
            return;
        } 

        m_Target = Character.PlayerCharacter.transform;
        m_TimeSinceLastTargetView = timeBeforeTargetLost;

        m_Animator.SetTrigger(m_HashSpottedPara);

    }

    public void OrientToTarget()
    {
        if (m_Target == null)
        {
            return;
        }

        Vector3 toTarget = m_Target.position - transform.position;

        

        if (Vector2.Dot(toTarget, m_SpriteForward) < 0)
        {
           
            SetFacingData(Mathf.RoundToInt(-m_SpriteForward.x));
            //SetFacingData(Mathf.RoundToInt(-1));

        }
    }
    public void ForgetTarget()
    {
        m_Animator.SetTrigger(m_HashTargetLostPara);
        m_Target = null;
    }
    public void ChekTargetStillVisible()
    {
        if (m_Target == null )
        {
            return;
        }

        Vector3 toTarget = m_Target.position - transform.position;

        if (toTarget.sqrMagnitude < viewDistance * viewDistance)
        {
            Vector3 testForward = Quaternion.Euler(0, 0, filp ? -viewDirection : viewDirection) * m_SpriteForward;
   
            if (filp)
            {
                testForward.x = -testForward.x;
            }

            float angle = Vector3.Angle(testForward, toTarget);
    
            if (angle <= viewFov * 0.5f)
            {             
                m_TimeSinceLastTargetView = timeBeforeTargetLost;              
            }

        }

        //ForgetTarget();

        if (m_TimeSinceLastTargetView <= 0.0f)
        {

            ForgetTarget();
        }
    }

    public void CheckMeleeAttack()
    {
        if (m_Target == null)
        {
            return;
        }
        if ((m_Target.transform.position - transform.position).sqrMagnitude < (meleeRange * meleeRange))
        {
            m_Animator.SetTrigger(m_HashMeleeAttackPara);         
        }      
    }

    public void Hit(Damager damager, Damabeable damageable)
    {
        if (damageable.CurrentHealth <= 0)
        {
            return;
        }

        if (m_FlickeringCoroutine != null)
        {
            StopCoroutine(m_FlickeringCoroutine);
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = m_OriginalColor[i];
            }
        }

        m_FlickeringCoroutine = StartCoroutine(Flicker(damageable));
    }

    public void Die(Damager damager, Damabeable damabeable)
    {
        var footPosition = capsuleCollider.size.y * 0.3f;
        //Vector3 moveforward = new Vector3(1,0,0);
        var newPosition = transform.position + new Vector3(0, footPosition, 0);

        bool flip = damabeable.GetDamageDirection().x > 0 ? false : true;

        VFXController.Instance.Trigger(bloodName, newPosition, 0, flip, null, null);
        this.gameObject.SetActive(false);

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = m_OriginalColor[i];
        }

        if (coinItem!=null)
        {
            coinItem.SpawnItem(transform.position);
        }
        
    }

    public void Shooting()
    {
        var footPosition = capsuleCollider.size.y * 0.3f;
        //Vector3 moveforward = new Vector3(1,0,0);
        var newPosition = transform.position + new Vector3(0, footPosition, 0);

        BulletObject bulletObject = m_bulletPool.Pop(shootingPos.transform.position);
        Vector3 direction = (m_TargetShootPosition - shootingPos.transform.position).normalized;
        bulletObject.rigidbody2D.AddForce(direction * shootSpeed);
        //bulletObject.spriteRenderer.flipX = filp ^ bulletObject.buller.spriteOriginallyFacesLeft;
       
        //VFXController.Instance.Trigger("PowerupGlow4_01", shootingPos.position, 0, false, null, null);
    }

    protected IEnumerator Flicker(Damabeable damageable)
    {
        float timer = 0f;
        float sinceLastChange = 0.0f;

        Color[] transparent = new Color[m_OriginalColor.Count];
        for (int i = 0; i < m_OriginalColor.Count; i++)
        {
            //transparent[i] = m_OriginalColor[i];
            transparent[i] = hitColor;
            //transparent[i].a = 0.6f;
        }

        int state = 1;

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = transparent[i];
        }

        while (timer < damageable.invulnerabilityDuration)
        {
            yield return null;
            timer += Time.deltaTime;
            sinceLastChange += Time.deltaTime;
            if (sinceLastChange > flickeringDuration)
            {
                sinceLastChange -= flickeringDuration;
                state = 1 - state;
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].color = state == 1 ? transparent[i] : m_OriginalColor[i];
                }
            }
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = m_OriginalColor[i];
        }
    }

    public void ResetSpriteColor()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = m_OriginalColor[i];
        }
    }
 
    public void SetMoveVector(Vector2 newMoveVector)
    {
        m_moveVector = newMoveVector;
    }

    public void EndAttack()
    {
        if (meleeDamager != null )
        {
            meleeDamager.gameObject.SetActive(false);
            meleeDamager.DisableDamage();
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Vector3 forward = spriteFaceLeft ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, spriteFaceLeft ? -viewDirection : viewDirection) * forward;

        //         if (GetComponent<SpriteRenderer>().flipX)
        //         {
        //             forward.x = -forward.x;
        //         }

        if (filp)
        {
            forward.x = -forward.x;
        }

        Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

        Handles.color = new Color(0, 1.0f, 0, 0.2f);
        //Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);

        Handles.color = new Color(1.0f, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.back, meleeRange);

    }


#endif


}
