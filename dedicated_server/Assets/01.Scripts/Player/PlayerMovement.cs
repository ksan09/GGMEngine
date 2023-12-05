using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpPower;

    [SerializeField] private PlayerAnimation _playerAnimation;

    private Vector2 _movementInput;
    private Rigidbody2D _rigidbody2D;

    [SerializeField] private Transform _groundCheckRayPos;
    [SerializeField] private Vector2 _rayDist;
    [SerializeField] private LayerMask _groundLayer;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _inputReader.MovementEvent += HandleMovement;
        _inputReader.JumpEvent += HandleJump;
    }

    private void HandleJump()
    {
        if (!IsOwner) return;
        if (GroundCheck() == false) return;
        _rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
    }

    private void HandleMovement(Vector2 movement)
    {
        if (!IsOwner) return;
        _movementInput = movement;
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        _playerAnimation.SetMove(_rigidbody2D.velocity.magnitude > 0.1f);
        _playerAnimation.FlipController( _rigidbody2D.velocity.x);

        Vector2 velo = _rigidbody2D.velocity;
        velo.x = _movementInput.x * _movementSpeed;
        _rigidbody2D.velocity = velo;

    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        _inputReader.MovementEvent -= HandleMovement;
        _inputReader.JumpEvent -= HandleJump;
    }

    private bool GroundCheck()
    {
        return Physics2D.Raycast(_groundCheckRayPos.position, _rayDist.normalized, Mathf.Abs(_rayDist.y), _groundLayer).collider;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_groundCheckRayPos.position, _groundCheckRayPos.position + (Vector3)_rayDist);
    }
#endif
}
