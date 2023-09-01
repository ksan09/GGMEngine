using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerInput _playerInput;
    private KeyAction _keyAction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _keyAction = new KeyAction();
        _keyAction.Enable();
        _keyAction.Player.Jump.performed += JumpPerfomed;
        //_keyAction.Player.Movement.performed += MovementPerfomed;

        //_playerInput.onActionTriggered += OnInputActionTriggered;
    }

    private void MovementPerfomed(InputAction.CallbackContext context)
    {
        Vector2 inputVec = context.ReadValue<Vector2>();
        Debug.Log(inputVec);
        
    }

    private void Update()
    {
        Vector2 inputVec = _keyAction.Player.Movement.ReadValue<Vector2>();
        float speed = 10f;
        _rb.AddForce(new Vector3(inputVec.x, 0, inputVec.y) * speed, ForceMode.Force);
    }

    private void JumpPerfomed(InputAction.CallbackContext context)
    {
        if (context.performed)
            _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

    private void OnInputActionTriggered(InputAction.CallbackContext context)
    {
            Debug.Log($"Jump{context.phase}");
        if (context.performed)
            _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
        Debug.Log($"Jump{context.phase}");
        
    }
}
