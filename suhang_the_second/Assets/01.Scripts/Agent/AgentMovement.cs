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
        _agentAnimator = transform.Find("PlayerRig/Visual").GetComponent<AgentAnimator>(); //에이전트 애니메이터 가져오기
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

        _movementVelocity = degreeY45 * _inputVelocity; //이동방향을 y축으로 45도 회전시켜서 처리한다. 

        _movementVelocity *= _moveSpeed * Time.fixedDeltaTime;

        _agentAnimator?.SetMoving(_movementVelocity.sqrMagnitude > 0); //이동속도를 반영                
    }

    public void SetRotation(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        dir.y = 0;
        _rigTrm.rotation = Quaternion.LookRotation(dir.normalized);

        Vector3 forwardVec = Quaternion.Inverse(_rigTrm.rotation) * _movementVelocity;

        _agentAnimator.SetDirection(new Vector2(forwardVec.x, forwardVec.z).normalized);
    }

    //즉시 정지
    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
        _agentAnimator?.SetMoving(false);
    }

    private void FixedUpdate()
    {
        CalculatePlayerMovement(); //플레이어 이동량 계산 및 회전
        
        if (_characterController.isGrounded == false)  //공중에 떠있다는 뜻
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
