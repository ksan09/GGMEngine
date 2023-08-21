using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("����������")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _bodyTrm;
    private Rigidbody2D _rigidbody;

    [Header("���ð���")]
    [SerializeField] private float _movementSpeed = 4f;
    [SerializeField] private float _turningRate = 30f;

    private Vector2 _prevMovementInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) // ���ʰ� �ƴ� ��쿡�� �Է°��� ���ؼ� �۵� �� �ϴϱ� ����
            return;
        _inputReader.MovementEvent += HandleMovement;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) // ���ʰ� �ƴ� ��쿡�� �Է°��� ���ؼ� �۵� �� �ϴϱ� ����
            return;
        _inputReader.MovementEvent -= HandleMovement;
    }

    private void HandleMovement(Vector2 move)
    {
        _prevMovementInput = move;
    }

    private void Update()
    {
        if (!IsOwner) // ���ʰ� �ƴ� ��쿡�� �Է°��� ���ؼ� �۵� �� �ϴϱ� ����
            return;

        //��ü Tread ��ž ȸ��
        if(_prevMovementInput.x != 0)
        {
            _bodyTrm.Rotate(0, 0, _prevMovementInput.x * -_turningRate * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) // ���ʰ� �ƴ� ��쿡�� �Է°��� ���ؼ� �۵� �� �ϴϱ� ����
            return;

        //��ġ �̵�
        _rigidbody.velocity = (Vector2)_bodyTrm.up * _movementSpeed * _prevMovementInput.y;
    }
}
