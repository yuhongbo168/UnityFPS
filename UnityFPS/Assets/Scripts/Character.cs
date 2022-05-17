using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem.Interactions;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject hand;

    public Transform resetPos;


    protected static Character s_PlayerCharacter;
    public static Character PlayerCharacter { get { return s_PlayerCharacter; } }

    [System.Serializable]
    public class TakeCoinUI : UnityEvent<CoinCanvas> { };

    //setting hit color
    [Header("Seting Color")]
    public Color hitColor;
    public List<Color> m_OriginalColor;
    public SpriteRenderer[] sprites;
    public float flickeringDuration;
    protected Coroutine m_FlickeringCoroutine;

    [Header("Damabeable")]
    public bool airAttack = true;
    public bool canLunchjumpUpEffect = true;
    public Damabeable damageable;
    public Damager meleedamager;
    [Range(0, 90)]
    public float hurtJumpAngle = 45f;
    public float hurtJumpSpeed = 5f;
    protected float m_TranHurtJumpAngle;

    [Header("Other")]
    public Transform characterTransform;
    public CapsuleCollider2D m_CapsuleCollider2D;
    public CharacterController PC;
    private Vector3 localScale = new Vector3(-1, 1, 1);
    private bool m_canMeleeAttack = true;

    [Header("Inventory")]
    private int m_storePice;
    public int GetStorePice { get { return m_storePice; } }
    public TakeCoinUI OnTakeCoinUI;

    public bool CanMeleeAttack
    {
        set { m_canMeleeAttack=value; } get { return m_canMeleeAttack; }
    }


    [Header("Movement")]
    public float movementSmoothingSpeed = 1f;
    public float moveSpeed = 3f;
    public float gravity = 50f;
    public float jumpSpeed = 40f;
    public float jumpAbortSpeedReduction = 100f;
    public float groundAccleration = 100f;
    public float groundDeceleration = 100f;
    public float airborneAccleration = 120f;
    public float airborneDeceleration = 120f;
    public float secondBounceHorizontal = 20f;
    public float secondBounceVertical = 20f;
    private Vector2 moveDirecation;
    private bool m_MoveDownY;
    private Vector2 m_MoveVector;
    private Vector3 flipX = new Vector3(1, 0, 0);

    [Header("Shooting")]
    public bool spriteOriginallyFacesLeft;
    public BulletPool bulletPool;
    public Transform facingLeftBulletSpawnPoint;
    public Transform facingRightBulletSpawnPoint;
    private Transform m_currenShootPosition;
    public float bulletSpeed = 10f;
    public Transform shootingPos;
    public float shotsPerSecond = 1f;
    public bool isShootingMagic = false;
    public float LunchAngle = 40f;

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovemnt;

 

    private float m_NextShotTime;
    private float m_ShotSpawnGap;
    private Coroutine fireCoroutine;

    //Jump Setting
    private bool m_Jump;
    private bool m_Crouching;
    public bool secoudJump
    {
        set;get;
    }    
    public bool IsForwardWall
    {
        set;get;
    }
  
    private bool m_SecondJump;
    private bool m_push;
    public LayerMask wallLayer;
    private ContactFilter2D m_ContactFilterWall;


    [Header("GrabRope")]
    public LayerMask ropeLayer;
    private ContactFilter2D m_contactFilter2D;
    private RaycastHit2D[] m_RopeHit = new RaycastHit2D[3];
    private GameObject m_RopeGameObject;
    private bool m_HaveDetectionRope = true;
    private ApplyForce force;
    bool m_UseForce;
    float m_ForceRopeTimer;
    Transform m_RopeNodes;


    [Header("OnChain")]
    public float slidSpeed = 5f;
    protected ChainSide m_ChainSide;
    protected bool m_OnChainJump;
    protected int m_Index;
    public bool OnChainJump
    {
        get { return m_OnChainJump; }
    }
    
    public bool SlidChian
    {
        set;get;
    }


    public ChainSide chainside
    {
        set;get;
    }

    //Pushable
    private Pushable m_Pushable;
    private Pushable m_CurrentPushable;
    protected List<Pushable> m_CurrentPushables = new List<Pushable>(4);
    protected Transform m_Transform;

    //private Animator m_Animator;
    public Animator m_Animator;

    protected readonly int m_HashHorizontalSpeedPara = Animator.StringToHash("HorizontalSpeed");
    protected readonly int m_HashGroundedPara = Animator.StringToHash("Grounded");
    protected readonly int m_HashCrouchingPara = Animator.StringToHash("Crouching");
    protected readonly int m_HashMeleeAttackPara = Animator.StringToHash("MeleeAttack");
    protected readonly int m_HashDeadPara = Animator.StringToHash("Dead");
    protected readonly int m_HashHurtPara = Animator.StringToHash("Hurt"); 
    protected readonly int m_HashRespawnPara = Animator.StringToHash("Respawn");
    protected readonly int m_ForcedRespawnPapa = Animator.StringToHash("ForcedRespawn");
    protected readonly int m_HashShootMagicPapa = Animator.StringToHash("ShootMagic");
    protected readonly int m_HashBounceJumpPapa = Animator.StringToHash("BounceJump");
    protected readonly int m_HashPushingPapa = Animator.StringToHash("Pushing");
    protected readonly int m_HashPullPapa = Animator.StringToHash("Pull");
    protected readonly int m_HashPushReadyPapa = Animator.StringToHash("PushReady");
    protected readonly int m_HashGrabPapa = Animator.StringToHash("Grab");
    protected readonly int m_HashGrabUPPapa = Animator.StringToHash("GrabUP");
    protected readonly int m_HashGrabDownPapa = Animator.StringToHash("GrabDown");
    protected readonly int m_HashSlidAtChainPapa = Animator.StringToHash("OnChain");

    protected const float k_GroundedStickingVelocityMultiplier = 3f;

    protected Checkpoint m_LastCheckpoint = null;
    private  PlayerInput m_PlyaerInput;
    private Vector3 m_StartingPosition = Vector3.zero;

    RaycastHit2D[] hit = new RaycastHit2D[3];

    private void Awake()
    {
        s_PlayerCharacter = this;

        PC = GetComponent<CharacterController>();
        // m_Animator = GetComponent<Animator>();
        m_CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        m_PlyaerInput = GetComponent<PlayerInput>();

        m_currenShootPosition = spriteOriginallyFacesLeft ? facingLeftBulletSpawnPoint : facingRightBulletSpawnPoint;
        flipX = spriteOriginallyFacesLeft ? localScale : Vector3.one;

        m_Transform = transform;

        m_ContactFilterWall.layerMask = wallLayer;
        m_ContactFilterWall.useTriggers = false;
        m_ContactFilterWall.useLayerMask = true;

        m_contactFilter2D.layerMask = ropeLayer;
        m_contactFilter2D.useLayerMask = true;
        m_contactFilter2D.useTriggers = false;

        m_ForceRopeTimer = 0.3f;

    }

    void Start()
    {
        hurtJumpAngle = Mathf.Clamp(hurtJumpAngle, 0, 90);
        m_TranHurtJumpAngle = Mathf.Tan(Mathf.Deg2Rad * hurtJumpAngle);
        sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (var item in sprites)
        {
            m_OriginalColor.Add(item.color);
        }

        m_ShotSpawnGap = 1f / shotsPerSecond;
        m_NextShotTime = Time.time;

        ScenceLinkSMB<Character>.Initialise(m_Animator,this);

        meleedamager.DisableDamage();

        m_StartingPosition = transform.position;

    }

    private void FixedUpdate()
    {
       // ResetHandPosition();


        PC.Move(m_MoveVector * Time.deltaTime);
        m_Animator.SetFloat(m_HashHorizontalSpeedPara, m_MoveVector.x);

        if (PC.IsGrounded)
        {
            airAttack = true;
        }

       // CheckBounceJump();

        Pushable();
        // CheckForPushing();
        GrabRope();

        if (m_UseForce)
        {
            if (force!=null)
            {
                force.ApplyForceToJoint(flipX.x < 0);
            }
        }
        else if (force != null)
        {
            if (m_ForceRopeTimer>0)
            {
                m_ForceRopeTimer-=Time.deltaTime;
                //force.force = new Vector2(1000f, 0);
                force.ApplyForceToJoint(flipX.x < 0);
            }
        }

        if (PC.IsGrounded)
        {
            if (chainside != null)
            {
                for (int i = 0; i < chainside.nodes.Count; i++)
                {
                    chainside.nodes[i].GetComponent<CircleCollider2D>().enabled = true;
                }
            }
            chainside = null;

            m_Animator.SetBool(m_HashSlidAtChainPapa, false);
        }

        CheckForwardCollision();

    }

    public void Pushable()
    {
        if (m_Pushable != null)
        {
            if (m_push)
            {

                bool movingRight = moveDirecation.x > float.Epsilon;
                bool movingLeft = moveDirecation.x < -float.Epsilon;

                float pushablePosX = m_Pushable.pushablePosition.position.x;
                float playerPosX = m_Transform.position.x;

                // UpdateFacing(pushablePosX < playerPosX);

                if (pushablePosX < playerPosX)
                {


                    Vector2 moveToPosition = m_Pushable.playerPushingRightPosition.position;
                    moveToPosition.y = PC.rd.position.y;
                    PC.Teleport(moveToPosition);

                    m_Animator.SetBool(m_HashPushReadyPapa, m_push);

                    m_Animator.SetBool(m_HashPullPapa, movingRight);

                    m_Animator.SetBool(m_HashPushingPapa, movingLeft);

                }

                if (pushablePosX > playerPosX)
                {

                    Vector2 moveToPosition = m_Pushable.playerPushingLeftPosition.position;
                    moveToPosition.y = PC.rd.position.y;
                    PC.Teleport(moveToPosition);
                    

                    m_Animator.SetBool(m_HashPushReadyPapa, m_push);

                    m_Animator.SetBool(m_HashPullPapa, movingLeft);

                    m_Animator.SetBool(m_HashPushingPapa, movingRight);

                }
            }
            else
            {
                m_Animator.SetBool(m_HashPushReadyPapa, false);
                m_Animator.SetBool(m_HashPullPapa, false);
                m_Animator.SetBool(m_HashPushingPapa, false);
            }

        }

    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        moveDirecation = value.ReadValue<Vector2>();
    }

    public void OnCrouching(InputAction.CallbackContext value)
    {
        //if (value.started)
        //{
        //    m_Crouching = true;
        //}
        //if (value.canceled)
        //{
        //    m_Crouching = false;
        //}
    }

    public void OnPush(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (m_Pushable!=null)
            {
                m_push = true;
            }
           
        }
        if (value.canceled)
        {
            m_push = false;
        }
    }

    public void CheckForCrouching()
    {
        m_MoveDownY = moveDirecation.y < -0.2f ? true : false;
        m_Animator.SetBool(m_HashCrouchingPara, moveDirecation.y < -0.2f ? true : false);
    }
    public void OnJump(InputAction.CallbackContext context)
    {

        // bool twoJump = false;

        if (!PC.IsGrounded&&context.started)
        {
            m_SecondJump = context.started;
        }


        

        m_Jump = context.started;

//         if (context.started)
//         {
//             m_Jump = true;
// 
//         }

        if (SlidChian)
        {
            m_OnChainJump = context.started;
        }



        if (context.canceled)
        {
            m_SecondJump = false;
            // 
            //             m_Jump = false;

               m_OnChainJump = false;
        }
        
    }

    public bool CheckForFallInput()
    {
        return m_Jump && moveDirecation.y < 0 ;
    }

    public bool MakePlatformFallthrough()
    {

        int colliderCount = 0;
       // int fallthroughColliderCount = 0;

        for (int i = 0; i < PC.GroundColliders.Length; i++)
        {
            Collider2D col = PC.GroundColliders[i];
            if (col == null)
            {
                continue;
            }

            colliderCount++;

        }

        if (colliderCount > 0)
        {
            for (int i = 0; i < PC.GroundColliders.Length; i++)
            {
                Collider2D col = PC.GroundColliders[i];
                if (col == null)
                {
                    continue;
                }

                PlatformEffector2D effector;

                effector = col.GetComponent<PlatformEffector2D>();

                if (effector!=null)
                {
                    FallthroughReseter reseter = effector.gameObject.AddComponent<FallthroughReseter>();
                    reseter.StartFall(effector);
                }
           
            }
        }

        return true;
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

    public void ShootingMagic()
    {
        BulletObject obj = bulletPool.Pop(shootingPos.position);
        float lunchAngle = Mathf.Tan(Mathf.Deg2Rad * LunchAngle);
        float y = Mathf.Abs(flipX.x) * lunchAngle;
        Vector2 newVector = new Vector2(flipX.x, y).normalized;
        newVector *= bulletSpeed;
        obj.rigidbody2D.AddForce(newVector);
    }

    public bool CheckForGrounded()
    {
        bool grounded = PC.IsGrounded;

        m_Animator.SetBool(m_HashGroundedPara, grounded);
        

        return grounded;
    }

    protected void SpawnBullet()
    {
        BulletObject bullet = bulletPool.Pop(m_currenShootPosition.position);

        bool faceLeft = m_currenShootPosition == facingLeftBulletSpawnPoint;
        bullet.rigidbody2D.velocity = new Vector2(faceLeft? -bulletSpeed:bulletSpeed, -0f);
        bullet.spriteRenderer.flipX = faceLeft ^ bullet.buller.spriteOriginallyFacesLeft;
    }


    public void OnMeleeAttack(InputAction.CallbackContext value)
    {
        

        if (value.started)
        {
            m_UseForce = true;
            if (m_canMeleeAttack&&m_RopeGameObject==null)
            {
                if (PC.IsGrounded)
                {
                    m_Animator.SetTrigger(m_HashMeleeAttackPara);
                }
                else
                {
                    if (airAttack)
                    {
                        airAttack = false;
                        m_Animator.SetTrigger(m_HashMeleeAttackPara);
                        
                    }
                }
            }        
            
        }

        if (value.canceled)
        {
            m_UseForce = false;
        }

    }

    public void OnFire(InputAction.CallbackContext value)
    {
 
        if (value.started)
        {
            if (!isShootingMagic)
            {
                //isShootingMagic = true;
                m_Animator.SetTrigger(m_HashShootMagicPapa);
            }
            
            //CheckAndFireGun();
     
        }
        else if (value.canceled)
        {

            //StopFireing();
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

    public bool CheckForSeconJumpInut()
    {
        return m_SecondJump;
    }

    public void SetVerticalMovement(float value)
    {

        m_MoveVector.y = value;

//         if (m_Animator.GetBool(m_HashSlidAtChainPapa))
//         {
//             m_Animator.SetBool(m_HashSlidAtChainPapa, false);
//         }

    }

    public void SetMoveVector(Vector2 newMoveVector)
    {
        m_MoveVector = newMoveVector;
    }

    public Vector2 GetHurtDirection()
    {
        Vector2 damageDirection = damageable.GetDamageDirection();

        if (damageDirection.y < 0f)
        {
            return new Vector2(Mathf.Sign(damageDirection.x), 0);
        }
        float y = Mathf.Abs(damageDirection.x) * m_TranHurtJumpAngle;

        return new Vector2(damageDirection.x, y).normalized;
    
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

        if (m_MoveVector.y < -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier)
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
        float acceleration = useInput && moveDirecation.x != 0 ? groundAccleration * airborneAccleration : groundDeceleration * airborneDeceleration;

        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
    }

    public void UpdateJump()
    {      
        if (m_MoveVector.y > 0.0f)
        {         
            m_MoveVector.y -= jumpAbortSpeedReduction * Time.deltaTime;
        }
    }

    public void UpdateFacing()
    {
        bool faceLeft = moveDirecation.x < 0f;
        bool faceRight = moveDirecation.x > 0f;

        if (faceLeft)
        {
            flipX = localScale;
            characterTransform.localScale = localScale;
        }

        if (faceRight)
        {
            flipX = Vector3.one;
            characterTransform.localScale = Vector3.one;
        }
    }

    public void UpdateFacing(bool faceLeft)
    {
        if (faceLeft)
        {
            flipX = localScale;
            characterTransform.localScale = localScale;
        }
        else
        {
            flipX = Vector3.one;
            characterTransform.localScale = Vector3.one;
        }

    }

    public void UpdateFacingSlid()
    {
        if (chainside!=null)
        {
            bool left = transform.position.x - chainside.chainEnd.position.x > 0 ? true:false;
            if (left)
            {
                flipX = localScale;
                characterTransform.localScale = localScale;
            }
            else
            {
                flipX = Vector3.one;
                characterTransform.localScale = Vector3.one;
            }
        
        }
    }

    public void EnableMelleAttack()
    {
        //canMeleeAttack = false;
        meleedamager.EnableDamage();
        meleedamager.disableDamageAterHit = true;
    }

  
    public void DisableMeleeAttack()
    {
        CanMeleeAttack = true;
        meleedamager.DisableDamage();
    }

    public void ApplyFootEffect(string vfxname)
    {
        VFXController.Instance.Trigger(vfxname,transform.position,0,true,null,null);

    }

    public void ApplyFootDownEffect(string vfxname)
    {
        VFXController.Instance.Trigger(vfxname, transform.position, 0, true, null, null);
    }

    public void TeleportToColliderBottom()
    {
        Vector2 colliderBottom = PC.rd.position + m_CapsuleCollider2D.offset + Vector2.down * m_CapsuleCollider2D.size.y * 0.5f;
        PC.Teleport(colliderBottom);
    }

    public void ApplySceneEffect(string vfxname)
    {
        float footsetp = PC.rd.position.y - PC.capsuleCollider.size.y + PC.capsuleCollider.offset.y;
        var footsetpPosition = new Vector3(PC.rd.position.x, footsetp);

        VFXController.Instance.Trigger(vfxname, footsetpPosition, 0, false, null, null);
    }

    public void OnHurt(Damager damager,Damabeable damageable)
    {
               
        UpdateFacing(damageable.GetDamageDirection().x > 0f);
        damageable.EnableInvuInerability();


        m_Animator.SetTrigger(m_HashHurtPara);

     
        m_Animator.SetBool(m_HashGroundedPara, false);

        if (m_FlickeringCoroutine!=null)
        {
            StopCoroutine(m_FlickeringCoroutine);
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = m_OriginalColor[i];
            }

        }

        StartFlickering();

    }

    public void StartFlickering()
    {
        m_FlickeringCoroutine = StartCoroutine(Flicker(damageable));
    }

    public void StopFlickering()
    {
        StopCoroutine(m_FlickeringCoroutine);
    }

    public void OnDie(Damager damager,Damabeable damabeable)
    {
        m_Animator.SetTrigger(m_HashDeadPara);
        StartCoroutine(DieRespawnCoroutine(true,true));
    }
    IEnumerator DieRespawnCoroutine(bool resetHealth,bool useCheckPoint)
    {
        m_PlyaerInput.actions.Disable();
        yield return new WaitForSeconds(0.1f);
        if (!useCheckPoint)
        {
            yield return new WaitForSeconds(1f);
        }
        Respawn(resetHealth, useCheckPoint);

       // yield return new WaitForEndOfFrame();
        m_PlyaerInput.actions.Enable();
    }

    public IEnumerator Flicker(Damabeable damageable)
    {
        float timer = 0f;
        float hitTimer = 0f;
        float sinceLastChange = 0.0f;

        Color[] transparent = new Color[m_OriginalColor.Count];
        for (int i = 0; i < m_OriginalColor.Count; i++)
        {
            transparent[i] = hitColor;
        }

        int state = 1;

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = transparent[i];
        }


        while (timer < damageable.invulnerabilityDuration)
        {
            yield return null;
            
            timer += Time.deltaTime;
            sinceLastChange += Time.deltaTime;

            if (sinceLastChange > flickeringDuration)
            {
                sinceLastChange -= flickeringDuration;
                state = 1 - state;
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].color = state == 1 ? transparent[i] : m_OriginalColor[i];
                }
            }
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = m_OriginalColor[i];
        }
    }

    public void Respawn(bool resetHealth,bool useCheckpoint)
    {
        if (resetHealth)
        {
            damageable.SetHealth(damageable.startingHealth);
            damageable.OnSetHealth.Invoke(damageable);
        }

        //m_Animator.ResetTrigger()

        if (m_FlickeringCoroutine!=null)
        {
            StopFlickering();
        }

        m_Animator.SetTrigger(m_HashRespawnPara);

        if (useCheckpoint && m_LastCheckpoint != null)
        {
            //UpdateFacing(m_LastCheckpoint.respawnFacingLeft);
            GameObjectTeleporter.Teleport(gameObject, m_LastCheckpoint.transform.position);
        }
        else
        {
            //UpdateFacing(flipX.x > 0);
            GameObjectTeleporter.Teleport(gameObject, m_StartingPosition);
        }

    }

    public bool IsFalling()
    {
        return m_MoveVector.y < 0f && !m_Animator.GetBool(m_HashGroundedPara);
    }

    public void SetChekpoint(Checkpoint checkpoint)
    {
        m_LastCheckpoint = checkpoint;
    }

    public void SetStorePice(int count)
    {
        m_storePice += count;
    }

    protected void CheckForwardCollision()
    {
        int count = Physics2D.Raycast(transform.position, flipX.x < 0 ? Vector2.left * 0.2f : Vector2.right * 0.2f, m_ContactFilterWall, hit, .5f);
        if (count>0)
        {
            if (hit[0].collider != null && !PC.IsGrounded)
            {
                IsForwardWall = true;
            }
        }
        else
        {
            IsForwardWall = false;
        }
 
       

    }

    public bool CheckBounceJump()
    {
        //m_SecondJump = true;
        //airAttack = true;
        m_Animator.SetBool(m_HashBounceJumpPapa, IsForwardWall);
        return IsForwardWall;            
    }
    public void SecondBounceJump()
    {

        if (hit[0].collider != null && !PC.IsGrounded)
        {

            SetVerticalMovement(secondBounceVertical);
            SetAirborneHorizonalMovement(secondBounceHorizontal * (hit[0].normal.x));
            UpdateFacing(flipX.x > 0 ? true : false);

        }

    }

    public void SetAirborneHorizonalMovement(float value)
    {
        m_MoveVector.x = value;
    }

    public void SecondBounceJumpVerticlMovment()
    {
         if (Mathf.Approximately(m_MoveVector.y,0))
         {
             m_MoveVector.y = 0;
         }

        m_MoveVector.y -= gravity * 0.3f * Time.deltaTime;

    }

    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        m_Pushable = collision.GetComponent<Pushable>();
        //         //         Pushable pushable = collision.GetComponent<Pushable>();
        //         //         if (pushable != null)
        //         //         {
        //         //             m_CurrentPushables.Add(pushable);
        //         //         }
        // 
        //         m_Pushable = collision.GetComponent<Pushable>();
        // 
        //         if (m_Pushable != null)
        //         {
        //             
        //             // Debug.Log(m_Pushable);
        //         }
        // 
        //         if (m_push)
        //         {                   
        //            
        //         }
        // 
    }

  

    private void OnTriggerExit2D(Collider2D collision)
    {
        

        Pushable pushable = collision.GetComponent<Pushable>();
        if (pushable!=null)
        {
            m_Animator.SetBool(m_HashPushReadyPapa, false);
            m_Animator.SetBool(m_HashPullPapa, false);
            m_Animator.SetBool(m_HashPushingPapa, false);

            m_Pushable = null;
            if (m_CurrentPushables.Contains(pushable))
            {
                m_CurrentPushables.Remove(pushable);
            }
        }
    }

    public void CheckForPushing()
    {
        bool pushableOnCorrectSide = false;
        Pushable previousPushable = m_CurrentPushable;

        m_CurrentPushable = null;

        if (m_CurrentPushables.Count > 0)
        {
            bool movingRight = moveDirecation.x > float.Epsilon;
            bool movingLeft = moveDirecation.x < -float.Epsilon;

            for (int i = 0; i < m_CurrentPushables.Count; i++)
            {
                float pushablePosX = m_CurrentPushables[i].pushablePosition.position.x;
                float playerPosX = m_Transform.position.x;
                if (pushablePosX < playerPosX && movingLeft || pushablePosX > playerPosX && movingRight)
                {
                    pushableOnCorrectSide = true;
                    m_CurrentPushable = m_CurrentPushables[i];
                    break;
                }
            }

            if (pushableOnCorrectSide)
            {
                Vector2 moveToPosition = movingRight ? m_CurrentPushable.playerPushingRightPosition.position : m_CurrentPushable.playerPushingLeftPosition.position;
                moveToPosition.y = PC.rd.position.y;
                PC.Teleport(moveToPosition);
            }
        }

        if (previousPushable != null && m_CurrentPushable != previousPushable)
        {//we changed pushable (or don't have one anymore), stop the old one sound
            
        }

        m_Animator.SetBool(m_HashPushingPapa, pushableOnCorrectSide);
        // m_Animator.SetBool(m_HashPushingPapa, pushableOnCorrectSide);
    }

    public void MovePushable()
    {
//         if (m_CurrentPushable && m_CurrentPushable.Grounded)
//         {
//             Vector2 newMoveVector = new Vector2(m_MoveVector.x, 0f);
//             m_CurrentPushable.Move(newMoveVector * Time.deltaTime);
//         }

        if (m_Pushable!=null && m_Pushable.Grounded)
        {
            Vector2 newMoveVector = new Vector2(m_MoveVector.x, 0f);
            m_Pushable.Move(newMoveVector * Time.deltaTime);
        }

    }

   public bool FindRope()
    {

        if (m_HaveDetectionRope)
        {
            int count = Physics2D.Raycast(transform.position, flipX.x < 0 ? Vector2.left : Vector2.right, m_contactFilter2D, m_RopeHit, .5f);

            if (count > 0)
            {

                if (m_RopeHit[0].collider != null)
                {
                    if (m_RopeGameObject == null)
                    {
                        m_RopeGameObject = m_RopeHit[0].collider.gameObject;
                        m_Animator.SetBool(m_HashGrabPapa,true);
                       // m_HaveDetectionRope = false;
                        //jumpCount = 1;
                        return true;
                    }
                }
            }

        }
        
        return false;
    }


  public void GrabRope()
    {
        if (m_RopeGameObject != null)
        {
            if (m_CapsuleCollider2D.enabled == true)
            {
                m_CapsuleCollider2D.enabled = false;
            }

            m_RopeNodes = m_RopeGameObject.transform.parent;

            var derection = (m_RopeNodes.position - transform.position).normalized;

            float angle = Mathf.Atan2(derection.y, derection.x) * Mathf.Rad2Deg;

            if (force == null)
            {
                var Nodeslist = m_RopeNodes.GetComponent<Rope>();
                int index = Nodeslist.Nodes.IndexOf(m_RopeGameObject);

                GameObject newNode = m_RopeGameObject;

                if (index > 2)
                {
                    newNode = Nodeslist.Nodes[index - 1];                   
                }            

                Rigidbody2D newRigindbody = newNode.GetComponent<Rigidbody2D>();

                newRigindbody.mass = PC.rd.mass + 50;

                force = newNode.AddComponent<ApplyForce>();
                force.force = new Vector2(1000, 0);           
                force.ApplyForceToJoint(false);
  
            }

            characterTransform.localPosition = new Vector3(0f, -0.5f, 0f);
            transform.position = Vector3.MoveTowards(transform.position, m_RopeGameObject.transform.position - new Vector3(flipX.x > 0 ? .2f : -.2f, 0f, 0f), 15*Time.deltaTime);
            //transform.position = m_RopeGameObject.transform.position - new Vector3( flipX.x > 0? .2f:-.2f, 0f, 0f);
            transform.localEulerAngles = new Vector3(0f, 0f, angle - 90f);
        }
        else
        {
            if (m_MoveVector.y < 1f)
            {
                m_CapsuleCollider2D.enabled = true;
            }
            m_ForceRopeTimer = 0.3f;
            characterTransform.localPosition = Vector3.zero;
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            force = null;

            if (m_RopeNodes!=null)
            {
                foreach (var item in m_RopeNodes.GetComponent<Rope>().Nodes)
                {
                    Rigidbody2D newRigindbodys = item.GetComponent<Rigidbody2D>();

                    newRigindbodys.mass = 10;
                }
            }
   
        }

    }

   public void GrapUP()
    {

        if (m_RopeGameObject != null)
        {

            Transform parentRope = m_RopeGameObject.transform.parent;
            Transform childerRope = m_RopeGameObject.transform;

            var Nodeslist = parentRope.GetComponent<Rope>();
            int index = Nodeslist.Nodes.IndexOf(m_RopeGameObject);

            if (index > 4)
            {
                var newNode = Nodeslist.Nodes[index - 2];
                m_RopeGameObject = newNode;
                force = null;

            }
        }
    }


   public void GrabDown()
    {

        if (m_RopeGameObject != null)
        {
            Transform parentRope = m_RopeGameObject.transform.parent;
            Transform childerRope = m_RopeGameObject.transform;

            var Nodeslist = parentRope.GetComponent<Rope>();
            int index = Nodeslist.Nodes.IndexOf(m_RopeGameObject);

            if (index < Nodeslist.Nodes.Count - 4)
            {
                var newNode = Nodeslist.Nodes[index + 2];
                m_RopeGameObject = newNode;
                force = null;

            }


        }

    }

    public void CheckGrab()
    {
        if (moveDirecation.y > 0.1f)
        {
            m_Animator.SetTrigger(m_HashGrabUPPapa);
        }

        if (moveDirecation.y < -.1f)
        {
            m_Animator.SetTrigger(m_HashGrabDownPapa);
        }
    }

    public void CancelGrapRope()
    {
        if (m_RopeGameObject)
        {
            m_HaveDetectionRope = true;
            m_RopeGameObject = null;
            m_Animator.SetBool(m_HashGrabPapa, false);
            SetVerticalMovement(15);
        }
    }


    public void CheckStartChain(int index)
    {
        m_Animator.SetBool(m_HashSlidAtChainPapa, true);
      
     
        if (chainside != null)
        {
           // PC.rd.gravityScale = 0f;
          
            for (int i = 0; i < chainside.nodes.Count; i++)
            {
                chainside.nodes[i].GetComponent<CircleCollider2D>().enabled = false;
            }

            m_Index = index;
            transform.position = chainside.nodes[index + 1].transform.position - new Vector3(0, 0.7f, 0);

  
        }

    }

    public void RunAtChain()
    {
        if (SlidChian)
        {
            if (Vector3.Distance(transform.position, chainside.chainEnd.position) > 0.9f)
            {
                PC.rd.gravityScale = 0f;
                transform.position = Vector3.MoveTowards(transform.position, chainside.chainEnd.position - new Vector3(0, 0.7f, 0), slidSpeed * Time.deltaTime);
                //Debug.Log(Vector3.Distance(transform.position, chainside.chainEnd.position));
            }
            else
            {
                m_Animator.SetBool(m_HashSlidAtChainPapa, false);
                SlidChian = false;
                PC.rd.gravityScale = 1f;
            }
         
        }
        else
        {
            m_Animator.SetBool(m_HashSlidAtChainPapa, false);
            PC.rd.gravityScale = 1f;
        }
       

        
    }

    public void CheckStopChian()
    {

        if (chainside!=null)
        {
            for (int i = 0; i < chainside.nodes.Count; i++)
            {
                chainside.nodes[i].GetComponent<CircleCollider2D>().enabled = true;
            }
        }
        
    }

}
