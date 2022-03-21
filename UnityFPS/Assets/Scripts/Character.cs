using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine;

public class Character : MonoBehaviour
{

    public CharacterController PC;
    public float movementSmoothingSpeed = 1f;
    public float moveSpeed = 3f;
    public float gravity = 50f;
    public float jumpSpeed = 40f;
    public float jumpAbortSpeedReduction = 100f;


    public BulletPool bulletPool;
    public Transform currenShootPosition;
    public float bulletSpeed = 10f;

    public float groundAccleration = 100f;
    public float groundDeceleration = 100f;
    public float airborneAccleration = 120f;
    public float airborneDeceleration = 120f;

    public float shotsPerSecond = 1f;

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovemnt;

    private Vector2 moveDirecation;
    private Vector2 m_MoveVector;

    private bool m_fire;
    private float m_NextShotTime;
    private float m_ShotSpawnGap;
    private Coroutine fireCoroutine;

    private bool m_Jump;

    private Animator m_Animator;

    protected readonly int m_HashHorizontalSpeedPara = Animator.StringToHash("HorizontalSpeed");
    protected readonly int m_HashGroundedPara = Animator.StringToHash("Grounded");

    protected const float k_GroundedStickingVelocityMultiplier = 3f;

    private void Awake()
    {
        PC = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

        m_ShotSpawnGap = 1f / shotsPerSecond;
        m_NextShotTime = Time.time;

        ScenceLinkSMB<Character>.Initialise(m_Animator,this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        PC.Move(m_MoveVector * Time.deltaTime);
        m_Animator.SetFloat(m_HashHorizontalSpeedPara, m_MoveVector.x);
        
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        moveDirecation = value.ReadValue<Vector2>();
       
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        m_Jump = context.started;
        
    }

    protected IEnumerator Shoot()
    {
        while (true)
        {
            if (Time.time>m_NextShotTime)
            {
                SpawnBullet();
                m_NextShotTime = Time.time + m_ShotSpawnGap;
            }
            yield return null;
        }
    }
    public bool CheckForGrounded()
    {
        bool grounded = PC.IsGrounded;

        m_Animator.SetBool(m_HashGroundedPara, grounded);

        return grounded;
    }

    protected void SpawnBullet()
    {
        BulletObject bullet = bulletPool.Pop(currenShootPosition.position);
        bullet.rigidbody2D.velocity = new Vector2(bulletSpeed, -0f);
    }


    public void OnFire(InputAction.CallbackContext value)
    {
 
        if (value.started)
        {       
            if (Time.time > m_NextShotTime)
            {
                CheckAndFireGun();
                m_NextShotTime = Time.time + m_ShotSpawnGap;
            }
        }
        else if (value.canceled)
        {
            StopFireing();
        }       
    }

    public void CheckAndFireGun()
    {
        fireCoroutine = StartCoroutine(Shoot());
    }

    public void StopFireing()
    {
        if (fireCoroutine!=null)
        {
            StopCoroutine(fireCoroutine);
        }
    }

   

    public bool CheckForJumpInput()
    {
        return m_Jump;
    }

    public void SetVerticalMovement(float value)
    {
        m_MoveVector.y = value;
    }

    void CalculateMovementInputSmoothing()
    {
        smoothInputMovemnt = Vector3.Lerp(smoothInputMovemnt, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    public void GroundedHorizonalMovement(bool useInput,float speedScale = 1f)
    {
        
        float desiredSpeed = useInput ? moveDirecation.x * moveSpeed * speedScale : 0;
        
        float acceleration = useInput && moveDirecation.x != 0 ? groundAccleration : groundDeceleration;
        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
       
    }

    public void GroundedVerticalMovement()
    {
        m_MoveVector.y -= gravity * Time.deltaTime;

        if (m_MoveVector.y<-gravity*Time.deltaTime*k_GroundedStickingVelocityMultiplier)
        {
            m_MoveVector.y = -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier;
        }

    }

    public void AirborneVerticalMovement()
    {
        if (Mathf.Approximately(m_MoveVector.y,0f))
        {
            m_MoveVector.y = 0f;
        }
        m_MoveVector.y -= gravity * Time.deltaTime;
    }

    public void AirborneHorizonalMovement(bool useInput, float speedScale = 1f)
    {
        float desiredSpeed = useInput ? moveDirecation.x * moveSpeed * speedScale : 0;

        float acceleration = useInput && moveDirecation.x != 0 ? airborneAccleration : airborneDeceleration;
        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
    }

    public void UpdateJump()
    {
        
        if (!m_Jump && m_MoveVector.y > 0.0f)
        {
            Debug.Log("Jump");
            m_MoveVector.y -= jumpAbortSpeedReduction * Time.deltaTime;
        }
    }

}
