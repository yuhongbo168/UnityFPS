using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class CharacterPlayer : MonoBehaviour
{

    public PlayerController PC;
    public float movementSmoothingSpeed = 0.5f;

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovemnt;

    private void Awake()
    {
        PC = GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovementInputSmoothing();
    }

    private void FixedUpdate()
    {
        PC.rd.MovePosition(transform.position + rawInputMovement*4f*Time.deltaTime);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        Debug.Log(inputMovement);
        rawInputMovement = new Vector3(inputMovement.x, inputMovement.y, 0f);
    }

    void CalculateMovementInputSmoothing()
    {
        smoothInputMovemnt = Vector3.Lerp(smoothInputMovemnt, rawInputMovement, Time.deltaTime * movementSmoothingSpeed);
    }

}
