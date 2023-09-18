using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PInputAct;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "SO/Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> MovementEvent;
    public Vector2 AimPos { get; private set; }

    private PInputAct _playerInputAction;

    private void OnEnable()
    {
        if(_playerInputAction == null)
        {
            _playerInputAction = new PInputAct();
            _playerInputAction.Player.SetCallbacks(this);
        }

        _playerInputAction.Player.Enable();
    }

    public void OnAIM(InputAction.CallbackContext context)
    {
        //
        AimPos = context.ReadValue<Vector2>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        //
        Vector2 value = context.ReadValue<Vector2>();
        MovementEvent?.Invoke(value);
    }
}
