using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rd;

    private PlayerController m_pc;
    public PlayerController Pc
    {
        get
        {
            return m_pc;
        }
    }

    public void SetMove()
    {

    }

    public void Move()
    {

    }

   
    // Start is called before the first frame update

    private void Awake()
    {
        m_pc = GetComponent<PlayerController>();
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
        Move();
    }
}
