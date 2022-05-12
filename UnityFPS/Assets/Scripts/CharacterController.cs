using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Rigidbody2D rd;
    public CapsuleCollider2D capsuleCollider;
    public LayerMask groundedLayerMask;

    public float groundedRaycastDistance = 0.1f;

    public ContactFilter2D ContactFilter { get { return m_ContactFilter; } }

    ContactFilter2D m_ContactFilter;


    private CharacterController m_pc;

    private Vector2 m_NextMovement;

    private Vector2 m_PriveousVector;
    private Vector2 m_CrreuntMovementVector;

    private RaycastHit2D[] m_HitBuffet = new RaycastHit2D[5];
    private RaycastHit2D[] m_FoundHits = new RaycastHit2D[3];

    public Collider2D[] GroundColliders { get { return m_GroundedColliders; } }
    private Collider2D[] m_GroundedColliders = new Collider2D[3];
    Vector2[] m_RaycastPositions = new Vector2[3];

    public bool IsGrounded { get; protected set; }
    
    public Vector2 Velocity
    {
        get;protected set;
    }

    public CharacterController Pc
    {
        get
        {
            return m_pc;
        }
    }

 
    public void Teleport(Vector2 position)
    {
        Vector2 delta = position - m_CrreuntMovementVector;
        m_PriveousVector += delta;
        m_CrreuntMovementVector = position;
        rd.MovePosition(position);
    }

    public void Move(Vector2 newMovement)
    {
        m_NextMovement += newMovement;
    }

    private void Awake()
    {
        m_pc = GetComponent<CharacterController>();
        rd = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        m_CrreuntMovementVector = rd.position;
        m_PriveousVector = rd.position;

        m_ContactFilter.layerMask = groundedLayerMask;
        m_ContactFilter.useLayerMask = true;
        m_ContactFilter.useTriggers = false;

        Physics2D.queriesStartInColliders = false;

    }

    private void FixedUpdate()
    {
        m_PriveousVector = rd.position;
        m_CrreuntMovementVector = m_PriveousVector + m_NextMovement;
        Velocity = (m_CrreuntMovementVector - m_PriveousVector) / Time.deltaTime;

        rd.MovePosition(m_CrreuntMovementVector);
        m_NextMovement = Vector2.zero;

        CheckCapsuleEndCollisions();

        
       
    }

   public void CheckCapsuleEndCollisions()
    {
        Vector2 raycastDirection;
        Vector2 raycastStart;
        float raycastDistance;

        if (capsuleCollider == null)
        {
            raycastStart = rd.position + Vector2.up;
            raycastDistance = 1f + groundedRaycastDistance;

            raycastDirection = Vector2.down;

            m_RaycastPositions[0] = raycastStart + Vector2.left*0.4f;
            m_RaycastPositions[1] = raycastStart;
            m_RaycastPositions[2] = raycastStart + Vector2.right * 0.4f;

        }
        else
        {
            raycastStart = rd.position + capsuleCollider.offset;
            raycastDistance = capsuleCollider.size.x * 0.5f + groundedRaycastDistance * 2f;
    
            raycastDirection = Vector2.down;
            Vector2 raycastStartBottomCentre = raycastStart + Vector2.down * (capsuleCollider.size.y * 0.5f /*- capsuleCollider.size.x * 0.5f*/);

            m_RaycastPositions[0] = raycastStartBottomCentre + Vector2.left * capsuleCollider.size.x * 0.5f ;
            m_RaycastPositions[1] = raycastStartBottomCentre;
            m_RaycastPositions[2] = raycastStartBottomCentre + Vector2.right * capsuleCollider.size.x * 0.5f;


        }


        for (int i = 0; i < m_RaycastPositions.Length; i++)
        {
            int count = Physics2D.Raycast(m_RaycastPositions[i], raycastDirection, m_ContactFilter, m_HitBuffet, raycastDistance);

            m_FoundHits[i] = count > 0 ? m_HitBuffet[0] : new RaycastHit2D();
            m_GroundedColliders[i] = m_FoundHits[i].collider;


        }

       


        Vector2 groundNormal = Vector2.zero;
        int hitCount = 0;

        for (int i = 0; i < m_FoundHits.Length; i++)
        {
            if (m_FoundHits[i].collider != null)
            {
                groundNormal += m_FoundHits[i].normal;
                hitCount++;
            }
        }
       

        if (hitCount > 0)
        {
            groundNormal.Normalize();
        }    

        Vector2 relativeVelocity = Velocity;

        if (Mathf.Approximately(groundNormal.x,0f)&&Mathf.Approximately(groundNormal.y,0f))
        {
            IsGrounded = false;
        }
        else
        {
            IsGrounded = relativeVelocity.y <= 0f;
        }


    }

}
