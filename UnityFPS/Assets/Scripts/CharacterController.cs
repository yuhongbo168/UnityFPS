using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Rigidbody2D rd;

    private CharacterController m_pc;

    private Vector2 m_NextMovement;

    private Vector2 m_PriveousVector;
    private Vector2 m_CrreuntMovementVector;
    
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


    // Start is called before the first frame update

    private void Awake()
    {
        m_pc = GetComponent<CharacterController>();
        rd = GetComponent<Rigidbody2D>();
        rd.gravityScale = 0f;
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
}
