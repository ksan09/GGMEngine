using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("����������")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _bodyTrm;
    [SerializeField] private ParticleSystem _dustCloudEffect;
    private Rigidbody2D _rigidbody;

    [Header("���ð���")]
    [SerializeField] private float _movementSpeed = 4f;
    [SerializeField] private float _turningRate = 30f;
    [SerializeField] private float _dustParticleEmisionValue = 10;

    private ParticleSystem.EmissionModule _emissionModule;
    private const float particleStopThreshold = 0.005f;

    private Vector2 _prevMovementInput;
    private Vector3 _prevPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _emissionModule = _dustCloudEffect.emission;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return; //���ʰ� �ƴ� ��쿡�� �Է°��� ���ؼ� �۵����� �������ϱ� ����
        _inputReader.MovementEvent += HandleMovement;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return; 
        _inputReader.MovementEvent -= HandleMovement;
    }

    private void HandleMovement(Vector2 move)
    {
        _prevMovementInput = move;
    }

    private void Update()
    {
        //��ü�� Tread �� ���� ����

        //���� owner������ �˻��ؾ���. ���ʰ� �ƴ϶�� ������ �ʿ䰡 ���� 
        // TurningRate�ӵ���ŭ _prevMovement���� X�Է��� ȸ�� ġ�� ���ؼ� 
        // �ٵ� Ʈ�������� ȸ�������ָ� �ȴ�.
        if (!IsOwner) return;

        float zRotation = _prevMovementInput.x * -_turningRate * Time.deltaTime;
        _bodyTrm.Rotate(0, 0, zRotation);

    }

    private void FixedUpdate()
    {
        //��ġ�� �̵���ų����
        //�������� �˻��ؼ�
        // ������ٵ��� �ӵ����ٰ� �ٵ��� up�������� y���� �����ؼ� movementSpeed��ŭ �̵������ָ� �ȴ�.
        if (!IsOwner) return; //���ʰ� �ƴϸ� ����

        if ((transform.position - _prevPosition).sqrMagnitude > particleStopThreshold)
        {
            _emissionModule.rateOverTime = _dustParticleEmisionValue;
        }
        else
        {
            _emissionModule.rateOverTime = 0;
        }
        _prevPosition = transform.position;

        _rigidbody.velocity = _bodyTrm.up * (_prevMovementInput.y * _movementSpeed);

    }
}
