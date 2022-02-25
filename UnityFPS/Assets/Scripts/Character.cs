using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Character : MonoBehaviour
{

    public CharacterController PC;
    public float movementSmoothingSpeed = 1f;
    public BulletPool bulletPool;
    public Transform currenShootPosition;
    public float bulletSpeed = 10f;

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovemnt;

    private void Awake()
    {
        PC = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Fire()
    {
        BulletObject bullet = bulletPool.Pop(currenShootPosition.position);
        bullet.rigidbody2D.velocity = new Vector2(bulletSpeed, -0f);

    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovementInputSmoothing();
    }

    private void FixedUpdate()
    {
        PC.rd.MovePosition(transform.position + smoothInputMovemnt * 3f * Time.deltaTime);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        Debug.Log(inputMovement);
        rawInputMovement = new Vector3(inputMovement.x, inputMovement.y, 0f);
    }

    public void OnFire(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            BulletObject bullet = bulletPool.Pop(currenShootPosition.position);
            bullet.rigidbody2D.velocity = new Vector2(bulletSpeed, -0f);
        }
    }

    void CalculateMovementInputSmoothing()
    {
        smoothInputMovemnt = Vector3.Lerp(smoothInputMovemnt, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

}
