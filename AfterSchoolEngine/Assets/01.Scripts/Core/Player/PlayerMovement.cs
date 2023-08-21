using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("참조데이터")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _bodyTrm;
    private Rigidbody2D _rigidbody;

    [Header("셋팅값들")]
    [SerializeField] private float _movementSpeed = 4f;
    [SerializeField] private float _turningRate = 30f;

    private Vector2 _prevMovementInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) // 오너가 아닐 경우에는 입력값에 대해서 작동 안 하니까 리턴
            return;
        _inputReader.MovementEvent += HandleMovement;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) // 오너가 아닐 경우에는 입력값에 대해서 작동 안 하니까 리턴
            return;
        _inputReader.MovementEvent -= HandleMovement;
    }

    private void HandleMovement(Vector2 move)
    {
        _prevMovementInput = move;
    }

    private void Update()
    {
        if (!IsOwner) // 오너가 아닐 경우에는 입력값에 대해서 작동 안 하니까 리턴
            return;

        //몸체 Tread 포탑 회전
        if(_prevMovementInput.x != 0)
        {
            _bodyTrm.Rotate(0, 0, _prevMovementInput.x * -_turningRate * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) // 오너가 아닐 경우에는 입력값에 대해서 작동 안 하니까 리턴
            return;

        //위치 이동
        _rigidbody.velocity = (Vector2)_bodyTrm.up * _movementSpeed * _prevMovementInput.y;
    }
}
