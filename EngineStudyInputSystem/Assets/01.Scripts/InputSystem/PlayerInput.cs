using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputAction _inputAction;

    public event Action<Vector2> OnMovement;
    public event Action<Vector2> OnAim;
    public event Action OnJump;
    public event Action OnFire;

    public Vector2 MousePos;

    private void Awake()
    {
        _inputAction = new PlayerInputAction();
        
        _inputAction.Player.Enable();
        _inputAction.Player.Jump.performed += JumpHandle;
        _inputAction.Player.Fire.performed += FireHandle;
    }

    private void FireHandle(InputAction.CallbackContext context)
    {
        OnFire?.Invoke();
    }

    private void JumpHandle(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }

    private void Update()
    {
        Vector2 inputVector = _inputAction.Player.Movement.ReadValue<Vector2>();
        OnMovement?.Invoke(inputVector);
    }

    private void LateUpdate()
    {
        MousePos = _inputAction.Player.Aim.ReadValue<Vector2>();
        OnAim?.Invoke(MousePos);
    }
}
