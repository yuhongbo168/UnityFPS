using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour
{
    [SerializeField]
    public Vector2 force;
    Rigidbody2D m_Rd;

    private void Awake()
    {
       
    }

    private void Start()
    {
        m_Rd = GetComponent<Rigidbody2D>();

    }

    public void ApplyForceToJoint(bool left)
    {
        if (m_Rd != null)
        {

            if (left)
            {
                m_Rd.AddForce(-force);
            }
            else
            {
                m_Rd.AddForce(force);
            }

        }
    }
}
