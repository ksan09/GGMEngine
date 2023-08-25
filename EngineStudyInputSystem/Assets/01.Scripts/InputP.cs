using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputP : MonoBehaviour
{
    private KeyAction _keyAction;

    public Action<Vector2> OnMovement;
    public Action OnJump;

    private void Awake()
    {
        _keyAction = new KeyAction();
        _keyAction.Player.Enable();
        _keyAction.Player.Jump.performed += JumpPerformed;
    }

    private void Update()
    {
        Vector2 inputVector = _keyAction.Player.Movement.ReadValue<Vector2>();
        OnMovement?.Invoke(inputVector);
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }
}
