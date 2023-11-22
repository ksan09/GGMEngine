using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float _movementSpeed;

    [SerializeField] private PlayerAnimation _playerAnimation;

    private Vector2 _movementInput;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _inputReader.MovementEvent += HandleMovement;
    }

    private void HandleMovement(Vector2 movement)
    {
        if (!IsOwner) return;
        _movementInput = movement;
    }

    private void FixedUpdate()
    {
        _playerAnimation.SetMove(_rigidbody2D.velocity.magnitude > 0.1f);
        _playerAnimation.FlipController( _rigidbody2D.velocity.x);

        if (!IsOwner) return;

        _rigidbody2D.velocity = _movementInput * _movementSpeed;

    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        _inputReader.MovementEvent -= HandleMovement;
    }
}
