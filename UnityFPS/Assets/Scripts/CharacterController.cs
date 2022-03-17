using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Rigidbody2D rd;
    public CapsuleCollider2D capsuleCollider;

    public float groundedRaycastDistance = 0.1f;

    private CharacterController m_pc;

    private Vector2 m_NextMovement;

    private Vector2 m_PriveousVector;
    private Vector2 m_CrreuntMovementVector;

    private RaycastHit2D[] m_HitBuffet = new RaycastHit2D[5];
    private RaycastHit2D m_FoundHits = new RaycastHit2D();
    private Collider2D m_GroundedColliders = new Collider2D();
    Vector2 m_RaycastPositions = new Vector2();

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

    public void SetMove()
    {

    }

    public void Move(Vector2 newMovement)
    {
        m_NextMovement += newMovement;
    }

    private void Awake()
    {
        m_pc = GetComponent<CharacterController>();
        rd = GetComponent<Rigidbody2D>();
        
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        m_PriveousVector = rd.position;
        m_CrreuntMovementVector = m_PriveousVector + m_NextMovement;
        Velocity = (m_CrreuntMovementVector - m_PriveousVector) / Time.deltaTime;

        rd.MovePosition(m_CrreuntMovementVector);
        m_NextMovement = Vector2.zero;
    }

    void CheckCapsuleEndCollisions()
    {
        Vector2 raycastDirection;
        Vector2 raycastStart;
        float raycastDistance;

        if (capsuleCollider == null)
        {
            raycastStart = rd.position + Vector2.up;
            raycastDistance = 1f + groundedRaycastDistance;

            raycastDirection = Vector2.down;
            m_RaycastPositions = raycastStart;

        }
        else
        {
            raycastStart = rd.position + capsuleCollider.offset;
            raycastDistance = capsuleCollider.size.x * 0.5f + groundedRaycastDistance * 2f;
    
            raycastDirection = Vector2.down;
            Vector2 raycastStartBottomCentre = raycastStart + Vector2.down * (capsuleCollider.size.y * 0.5f - capsuleCollider.size.x * 0.5f);


        }

        int count;

    }
}
