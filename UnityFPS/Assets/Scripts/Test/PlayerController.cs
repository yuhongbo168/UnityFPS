using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{

    private Vector2 m_NextMovement;
    private Vector2 m_PriveMoveVector;
    private Vector2 m_CurrentMoveVector;

    protected Rigidbody2D m_Rigidbody2D;
    protected CapsuleCollider2D m_CapsuleCollider2D;

    private void Awake()
    {

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_CapsuleCollider2D = GetComponent<CapsuleCollider2D>();

        m_Rigidbody2D.gravityScale = 1f;

    }

    private void FixedUpdate()
    {
        m_PriveMoveVector = m_Rigidbody2D.position;
        m_CurrentMoveVector = m_PriveMoveVector + m_NextMovement;
        m_Rigidbody2D.MovePosition(m_CurrentMoveVector);

        m_NextMovement = Vector2.zero;

        if (canMove)
        {
            Teleport(newPosistion);
        }
    }

    public void Move(Vector2 poistion)
    {
        m_NextMovement += poistion;
    }

    public void Teleport(Vector2 position)
    {
//         Vector2 date = position - m_CurrentMoveVector;
//         m_PriveMoveVector += date;
//         m_CurrentMoveVector = position;
        m_Rigidbody2D.MovePosition(position);
    }

    public Vector2 newPosistion;
    public bool canMove;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovePosition newMovePosition = collision.GetComponent<MovePosition>();

        if (newMovePosition != null)
        {
            canMove = true;
            newPosistion = newMovePosition.movePosition.position;
            Debug.Log(newMovePosition.movePosition.position);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canMove = false;
        newPosistion = Vector2.zero;
    }

}
