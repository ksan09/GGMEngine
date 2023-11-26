using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _reader;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _jumpPower = 3f;
    [SerializeField] private float _desiredRotationSpeed = 0.3f;
    [SerializeField] private float _allowPlayerRotation = 0.1f;
    [SerializeField] private Transform _modelTrm;

    private CharacterController _characterController;
    public bool IsGrounded => _characterController.isGrounded;

    private float _verticalVelocity; //세로 이동속도
    private Vector3 _movementVelocity;
    private Vector2 _inputDir;
    private Vector3 _desiredMoveDirection;
    public bool blockRotationPlayer = false; //사격중일때는 플레이어가 회전하지 않도록 한다.

    [SerializeField] private PlayerAnimator _animator;

    private Camera _mainCam;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _reader.MovementEvent   += HandleSetMove;
        _reader.JumpEvent       += HandleJump;

        _mainCam = Camera.main;
    }

    private void FixedUpdate()
    {

        CalculatePlayerMovement();
        ApplyGravity();
        Move();

        ApplyAnimation();

    }

    private void HandleSetMove(Vector2 movement)
    {
        //
        _inputDir = movement;
    }

    private void HandleJump()
    {
        //
        if (!IsGrounded) return;

        _verticalVelocity = _jumpPower;
    }

    private void CalculatePlayerMovement()
    {
        var forward = _mainCam.transform.forward;
        var right   = _mainCam.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        _desiredMoveDirection = forward * _inputDir.y + right * _inputDir.x;

        if (blockRotationPlayer == false && 
            _inputDir.sqrMagnitude > _allowPlayerRotation)
        {
            // 가는 방향으로 회전
            _modelTrm.rotation = Quaternion.Slerp(_modelTrm.rotation,
                Quaternion.LookRotation(_desiredMoveDirection),
                _desiredRotationSpeed);
        }

        _movementVelocity = _desiredMoveDirection * _moveSpeed * Time.deltaTime;


    }

    private void ApplyGravity()
    {
        if(IsGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -1;
        }
        else
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
        _movementVelocity.y = _verticalVelocity;
    }

    private void Move()
    {
        _characterController.Move(_movementVelocity);
    }

    private void ApplyAnimation()
    {
        _animator.SetXY(_inputDir);
        _animator.SetBlendValue(_inputDir.sqrMagnitude);
        _animator.SetShooting(blockRotationPlayer);
    }

    private void OnDestroy()
    {
        _reader.MovementEvent   -= HandleSetMove;
        _reader.JumpEvent       -= HandleJump;
    }
}
