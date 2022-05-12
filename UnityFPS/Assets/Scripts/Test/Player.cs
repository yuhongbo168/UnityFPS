using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{




    public Rope rope;


    private Vector2 m_MoveVector;
    private Vector2 m_InputVector;

    public LayerMask ropes;

    public GameObject oo;

    public Transform targetTransform;


    private ContactFilter2D m_contactFilter2D;
    private RaycastHit2D[] m_Hit=new RaycastHit2D[3];
    public GameObject m_RopeGameObject;
    private Vector2 m_GrapPos=new Vector3();
    private ApplyForce force;
    private float timer;
    private Vector2 m_CapCollistionSize;
    private bool m_DetectionRope = false;

    private Rigidbody2D m_Rigidbody2d;
    private CapsuleCollider2D m_CapsuleCollider2D;
    private PlayerController m_PlayerController;
    private SpriteRenderer m_SprinteRenderer;
    private void Awake()
    {
        m_contactFilter2D.layerMask = ropes;
        m_contactFilter2D.useLayerMask = true;
        m_contactFilter2D.useTriggers = false;

        m_PlayerController = GetComponent<PlayerController>();
        m_Rigidbody2d = GetComponent<Rigidbody2D>();
        m_CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        m_SprinteRenderer = GetComponent<SpriteRenderer>();

        timer = Time.time;
        m_CapCollistionSize = m_CapsuleCollider2D.size;
    }

    public void Update()
    {
        GrabRope();
    }

    private void FixedUpdate()
    {

       // transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime);

        m_PlayerController.Move(m_MoveVector * Time.deltaTime);

        if (Input.GetMouseButton(0))
        {
            if (force!=null)
            {
                Debug.Log("right");
                force.ApplyForceToJoint(false);
            }
        }

        if (Input.GetKey(KeyCode.G))
        {
            //m_DetectionRope = false;
            //Debug.Log("down");
            //m_RopeGameObject = null;
            m_CapsuleCollider2D.size = m_CapCollistionSize;
  
        }

        if (Input.GetMouseButton(1))
        {         

            if (force != null)
            {
                Debug.Log("left");
                force.ApplyForceToJoint(true);
            }
        }

        if (Input.GetKey(KeyCode.W))
        {

            if (Time.time > timer)
            {
                GrapUP();
                timer = Time.time + 0.1f;
            }                    
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (Time.time > timer)
            {
                GrapDown();
                timer = Time.time + 0.1f;
            }
        }


        VerticalMovement();

        HorizontalMovement();

        FindRope();

       

        UpdateFacing();

      
    }
    private void VerticalMovement()
    {
        m_MoveVector.y -= 60 * Time.deltaTime;

        if (m_MoveVector.y < -60 * Time.deltaTime * 10f)
        {
            m_MoveVector.y = -60 * Time.deltaTime * 10f;
        }
    }

    private void HorizontalMovement()
    {
        float desiredSpeed =  m_InputVector.x * 9f ;
        float acceleration = m_InputVector.x != 0 ? 100f : 100f;
        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration*Time.deltaTime);

    }

    public void UpdateFacing()
    {
        bool left = m_MoveVector.x < 0;
        bool right = m_MoveVector.x > 1;

        if (left)
        {
            m_SprinteRenderer.flipX = true;
        }
        if (right)
        {
            m_SprinteRenderer.flipX = false;
        }
    }

    private void SetVerticalMovement()
    {
        m_MoveVector.y = 20f;       
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rope.SubtractLinks();
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        m_InputVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SetVerticalMovement();

            if (m_DetectionRope==false)
            {
                m_DetectionRope = true;
                return;
            }
            else
            {
                m_DetectionRope = false;
                m_RopeGameObject = null;
            }


           
            
        }
    }
   

    bool FindRope()
    {

        if (m_DetectionRope)
        {
            int count = Physics2D.Raycast(transform.position,m_SprinteRenderer.flipX?Vector2.left:Vector2.right, m_contactFilter2D, m_Hit, 1f);

            if (count > 0)
            {

                if (m_Hit[0].collider != null)
                {
                    if (m_RopeGameObject == null)
                    {
                        m_RopeGameObject = m_Hit[0].collider.gameObject;
                        return true;
                    }
                }
            }

        }

        return false;

    }

    void GrabRope()
    {
        if (m_RopeGameObject!=null)
        {
            if (m_CapsuleCollider2D.enabled == true)
            {
                m_CapsuleCollider2D.enabled = false;
            }

            Transform parentRope = m_RopeGameObject.transform.parent;

            var derection = (parentRope.position - transform.position).normalized;

            float angle = Mathf.Atan2(derection.y, derection.x) * Mathf.Rad2Deg;

            if (force == null)
            {
                var Nodeslist = parentRope.GetComponent<Rope>();
                int index = Nodeslist.Nodes.IndexOf(m_RopeGameObject);

                GameObject newNode = m_RopeGameObject;
                if (index>2)
                {
                    newNode = Nodeslist.Nodes[index - 1];
                }
                

                Rigidbody2D newRigindbody = newNode.GetComponent<Rigidbody2D>();

                newRigindbody.mass = m_Rigidbody2d.mass + 50;

                force = newNode.AddComponent<ApplyForce>();
                force.force = new Vector2(2500, 0);
            }

            transform.position = m_RopeGameObject.transform.position-new Vector3(0.0f,0f,0f);

            transform.localEulerAngles = new Vector3(0f, 0f, angle - 90f);
        }
        else
        {
            if (m_MoveVector.y<0)
            {
                m_CapsuleCollider2D.enabled = true;
            }
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
  

    }


    void GrapUP()
    {

        if (m_RopeGameObject!=null)
        {
            Transform parentRope = m_RopeGameObject.transform.parent;
            Transform childerRope = m_RopeGameObject.transform;

            var Nodeslist = parentRope.GetComponent<Rope>();
            int index = Nodeslist.Nodes.IndexOf(m_RopeGameObject);



            if (index > 3)
            {
                var newNode = Nodeslist.Nodes[index-1];
                m_RopeGameObject = newNode;
                force = null;

            }
        }
       
    }


    void GrapDown()
    {

        if (m_RopeGameObject != null)
        {
            Transform parentRope = m_RopeGameObject.transform.parent;
            Transform childerRope = m_RopeGameObject.transform;

            var Nodeslist = parentRope.GetComponent<Rope>();
            int index = Nodeslist.Nodes.IndexOf(m_RopeGameObject);

            if (index < Nodeslist.Nodes.Count-3)
            {
                var newNode = Nodeslist.Nodes[index + 1];
                m_RopeGameObject = newNode;
                force = null;
            }
        }

    }

}
