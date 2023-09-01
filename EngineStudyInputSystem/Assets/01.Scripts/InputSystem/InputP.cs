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

        _keyAction.UI.Submit.performed += UISubmitPressed;

        _keyAction.Player.Disable();
        _keyAction.Player.Jump.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<keyboard>/escape")
            .OnComplete( op =>
            {
                op.Dispose();
                _keyAction.Player.Enable();
            })
            .OnCancel(op =>
            {
                op.Dispose();
                _keyAction.Player.Enable();
            })
            .Start();
    }

    private bool _uiMode = false;

    private void Update()
    {
        Vector2 inputVector = _keyAction.Player.Movement.ReadValue<Vector2>();
        OnMovement?.Invoke(inputVector);

        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _keyAction.Disable();
            _uiMode = !_uiMode;
            if(_uiMode == true)
            {
                _keyAction.UI.Enable();
            }
            else
            {
                _keyAction.Player.Enable();
            }
        }
    }

    private void UISubmitPressed(InputAction.CallbackContext context)
    {
        Debug.Log("UI Space ´­¸²");
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }
}
