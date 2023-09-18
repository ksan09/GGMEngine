using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravity = -9.8f;

    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _rootTrm; //���������� ���� ��Ʈ

    private float _gravityMultiplier = 1f;

    private CharacterController _characterController;
    public bool IsGround
    {
        get => _characterController.isGrounded;
    }

    private Vector2 _inputDirection;
    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;
    private float _verticalVelocity;

    //Ű����� �����̴� �����ΰ�?
    private bool _activeMove = true;
    public bool ActiveMove
    {
        get => _activeMove;
        set => _activeMove = value;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputReader.MovementEvent += SetMovement;
    }

    //����� PlayInput���� ����ó�� �ɰ���.
    public void SetMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity = (_rootTrm.forward * _inputDirection.y + _rootTrm.right * _inputDirection.x)
                            * (_moveSpeed * Time.deltaTime);
    }

    //��� ����
    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
    }

    //���� �ٸ� ��ũ��Ʈ���� �̵��� �ǵ帮�� �Ѵٸ� ���
    public void SetMovement(Vector3 value)
    {
        _movementVelocity = new Vector3(value.x, 0, value.z);
        _verticalVelocity = value.y;
    }

    private void ApplyGravity()
    {
        if (IsGround && _verticalVelocity < 0)  //���� ���� ����
        {
            _verticalVelocity = -1f;
        }
        else
        {
            _verticalVelocity += _gravity * _gravityMultiplier * Time.fixedDeltaTime;
        }

        _movementVelocity.y = _verticalVelocity;
    }

    private void Move()
    {
        _characterController.Move(_movementVelocity);
    }

    private void FixedUpdate()
    {
        //Ű����� �����϶��� �̷��� �����̰�
        if (_activeMove)
        {
            CalculatePlayerMovement();
        }
        ApplyGravity(); //�߷� ����
        Move();
    }
}
