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
    public BulletPool bulletPool;
    public Transform currenShootPosition;
    public float bulletSpeed = 10f;

    public float groundAccleration = 100f;
    public float groundDeceleration = 1f;

    public float shotsPerSecond = 1f;

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovemnt;

    private Vector2 moveDirecation;
    private Vector2 m_MoveVector;

    private bool m_fire;
    private float m_NextShotTime;
    private float m_ShotSpawnGap;
    private Coroutine fireCoroutine;

    private void Awake()
    {
        PC = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {

        m_ShotSpawnGap = 1f / shotsPerSecond;
        m_NextShotTime = Time.time;
    }

    public void Fire()
    {
     

    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovementInputSmoothing();
    }

    private void FixedUpdate()
    {
        PC.Move(m_MoveVector * Time.deltaTime);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        moveDirecation = value.ReadValue<Vector2>();
       
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

    void CalculateMovementInputSmoothing()
    {
        smoothInputMovemnt = Vector3.Lerp(smoothInputMovemnt, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

    public void HorizonalMovement(bool useInput,float speedScale = 1f)
    {
        float desiredSpeed = useInput ? moveDirecation.x * moveSpeed * speedScale : 0;
        float acceleration = useInput && moveDirecation.x != 0 ? groundAccleration : groundDeceleration;
        m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
    }

}
