using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 8f, _gravity = -9.8f;

    private CharacterController _characterController;

    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;
    private float _verticalVelocity;

    private Vector3 _inputVelocity;

    private AgentAnimator _agentAnimator;
    public bool IsGround => _characterController.isGrounded;

    private Quaternion degreeY45;
    private Transform _rigTrm;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _agentAnimator = transform.Find("PlayerRig/Visual").GetComponent<AgentAnimator>(); //������Ʈ �ִϸ����� ��������
        degreeY45 = Quaternion.AngleAxis(45f, Vector3.up);
        _rigTrm = transform.Find("PlayerRig");
    }

    public void SetInputVelocity(Vector3 value)
    {
        _inputVelocity = value;
    }

    private void CalculatePlayerMovement()
    {
        _inputVelocity.Normalize();

        _movementVelocity = degreeY45 * _inputVelocity; //�̵������� y������ 45�� ȸ�����Ѽ� ó���Ѵ�. 

        _movementVelocity *= _moveSpeed * Time.fixedDeltaTime;

        _agentAnimator?.SetMoving(_movementVelocity.sqrMagnitude > 0); //�̵��ӵ��� �ݿ�                
    }

    public void SetRotation(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        dir.y = 0;
        _rigTrm.rotation = Quaternion.LookRotation(dir.normalized);

        Vector3 forwardVec = Quaternion.Inverse(_rigTrm.rotation) * _movementVelocity;

        _agentAnimator.SetDirection(new Vector2(forwardVec.x, forwardVec.z).normalized);
    }

    //��� ����
    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
        _agentAnimator?.SetMoving(false);
    }

    private void FixedUpdate()
    {
        CalculatePlayerMovement(); //�÷��̾� �̵��� ��� �� ȸ��
        
        if (_characterController.isGrounded == false)  //���߿� ���ִٴ� ��
        {
            _verticalVelocity = _gravity * Time.fixedDeltaTime;
        }
        else
        {
            _verticalVelocity = _gravity * 0.3f * Time.fixedDeltaTime;
        }

        //Debug.Log($"In Fixed : { _movementVelocity }");

        Vector3 move = _movementVelocity + _verticalVelocity * Vector3.up;
        _characterController.Move(move);
    }
}
