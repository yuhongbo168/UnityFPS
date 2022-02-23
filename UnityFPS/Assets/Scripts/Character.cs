﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Character : MonoBehaviour
{

    public CharacterController PC;
    public float movementSmoothingSpeed = 1f;

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

    // Update is called once per frame
    void Update()
    {
        CalculateMovementInputSmoothing();
    }

    private void FixedUpdate()
    {
        PC.rd.MovePosition(transform.position + smoothInputMovemnt * 10f * Time.deltaTime);
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
