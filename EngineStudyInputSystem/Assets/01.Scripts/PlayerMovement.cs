using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _gravity = -9.8f;

    private float _gravityMultiplier = 1f;

    [SerializeField] private float _jumpPower = 4f;

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

    private PlayerInput _playerInput;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.OnMovement += SetPlayerMovement;
        _playerInput.OnJump += Jump;
        _playerInput.OnAim += SetAim;
    }

    //����� PlayInput���� ����ó�� �ɰ���.
    public void SetPlayerMovement(Vector2 value)
    {
        _inputDirection = value;
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity = new Vector3(_inputDirection.x, 0, _inputDirection.y) * (_moveSpeed * Time.fixedDeltaTime);

        //if (_movementVelocity.sqrMagnitude > 0)
        //{
        //    transform.rotation = Quaternion.LookRotation(_movementVelocity); //������ ������ ���� �ϰ�
        //}
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

    public void Jump()
    {
        if (!IsGround) return;
        _verticalVelocity += _jumpPower;
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

    private void SetAim(Vector2 mousePos)
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit);
        if(hit.collider)
        {
            Vector3 pos = hit.point;
            Vector3 dir = (pos - transform.position).normalized;
            float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, -angle+90, 0));
        }
        
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
