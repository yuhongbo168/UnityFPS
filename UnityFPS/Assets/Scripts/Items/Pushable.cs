using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Pushable : MonoBehaviour
{
    static ContactPoint2D[] s_ContactPointBuffer = new ContactPoint2D[16];
    static Dictionary<Collider2D, Pushable> s_PushableCache = new Dictionary<Collider2D, Pushable>();

    public Transform playerPushingRightPosition;
    public Transform playerPushingLeftPosition;
    public Transform pushablePosition;


    protected SpriteRenderer m_SpriteRenderer;
    protected Rigidbody2D m_Rigidbody2D;
    private bool m_Grounded;


    public bool Grounded
    {
        get { return m_Grounded; }
    }
    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (s_PushableCache.Count == 0 )
        {
            Pushable[] pushables = FindObjectsOfType<Pushable>();
            for (int i = 0; i < pushables.Length; i++)
            {
                Collider2D[] pushableColliders = pushables[i].GetComponents<Collider2D>();

                for (int j = 0; j < pushableColliders.Length; j++)
                {
                    s_PushableCache.Add(pushableColliders[j], pushables[i]);
                }
            }
        }

        
    }

 

    private void FixedUpdate()
    {
        Vector2 velocity = m_Rigidbody2D.velocity;
        velocity.x = 0f;

        m_Rigidbody2D.velocity = velocity;

        CheckGrounded();
       
    }

    public void Move(Vector2 movement)
    {
        m_Rigidbody2D.position = m_Rigidbody2D.position + movement;
       
    }

    protected void CheckGrounded()
    {
        m_Grounded = false;

        int count = m_Rigidbody2D.GetContacts(s_ContactPointBuffer);
        for (int i = 0; i < count; i++)
        {
            if (s_ContactPointBuffer[i].normal.y > 0.9f)
            {
                m_Grounded = true;

                Pushable pushable;

                if (s_PushableCache.TryGetValue(s_ContactPointBuffer[i].collider,out pushable))
                {
                    m_SpriteRenderer.sortingOrder = pushable.m_SpriteRenderer.sortingOrder + 1;
                }


            }
        }

        
    }
}
