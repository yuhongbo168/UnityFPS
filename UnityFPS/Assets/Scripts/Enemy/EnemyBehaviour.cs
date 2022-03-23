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


    [Header("Movement")]
    public float speed;
    public float gravity = 10.0f;

    public bool spriteFaceLeft = false;

    protected ContactFilter2D m_Filter;

    protected Bounds m_LocalBounds;

    protected SpriteRenderer m_SpriteRenderer;
    protected CharacterController m_CharacterController;
    protected Collider2D m_Collider;
    protected Animator m_Animator;

    protected Vector2 m_SpriteForward;

    protected Vector2 m_moveVector;

    public Vector3 moveVector
    {
        get { return m_moveVector; }
    }
    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Collider = GetComponent<Collider2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_SpriteForward = spriteFaceLeft ? Vector2.left : Vector2.right;
        if (m_SpriteRenderer.flipX)
        {
            m_SpriteForward = -m_SpriteForward;
        }

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
    }

    private void FixedUpdate()
    {
        m_moveVector.y = Mathf.Max(m_moveVector.y - gravity * Time.deltaTime, -gravity);

        m_CharacterController.Move(m_moveVector * Time.deltaTime);

        Debug.Log("move " + m_moveVector.x);
    }

    public bool CheckForObstacle(float forwardDistance)
    {
        if (Physics2D.CircleCast(m_Collider.bounds.center,m_Collider.bounds.extents.y - 0.2f,m_SpriteForward,forwardDistance,m_Filter.layerMask.value))
        {
            return true;
        }

        Vector3 castingPosition = (Vector2)(transform.position + m_LocalBounds.center) + m_SpriteForward * m_LocalBounds.extents.x;
        Debug.DrawLine(castingPosition, castingPosition + Vector3.down * (m_LocalBounds.extents.y + 0.2f));

        if (!Physics2D.CircleCast(castingPosition,0.1f,Vector2.down,m_LocalBounds.extents.y+0.2f,m_CharacterController.groundedLayerMask.value))
        {
            return true;
        }

        return false;
    }

    public void SetHorizontalSpeed(float horizontalSpeed)
    {
        m_moveVector.x = horizontalSpeed * m_SpriteForward.x;


    }


}
