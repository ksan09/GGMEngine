using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int _comboCounter;
    private float _lastAttackTime; // 마지막 공격 시간
    private float _comboWindow = 0.8f; // 콤보 끊기는 시간

    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");
    public PlayerPrimaryAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (_comboCounter > 2 || Time.time >= _lastAttackTime + _comboCounter)
            _comboCounter = 0;

        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);
        _player.AnimatorCompo.speed = _player.attackSpeed;

        float attackDirection = _player.FacingDirection;
        float xInput = _player.PlayerInput.XInput;

        if (Mathf.Abs(xInput) > 0.05f)
            attackDirection = xInput;

        Vector2 move = _player.attackMovement[_comboCounter];
        _player.SetVelocity(move.x * attackDirection, move.y);
        _player.StartDelayAction(() =>
        {
            _player.StopImmediately(false);
        }, 0.1f);

        //과제
    }

    public override void Exit()
    {
        ++_comboCounter;
        _lastAttackTime = Time.time;
        _player.AnimatorCompo.speed = 1;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
