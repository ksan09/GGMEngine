using System;
using UnityEngine;

public abstract class PlayerAirState : PlayerState
{
    protected PlayerAirState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        float xInput = _player.PlayerInput.XInput;

        if(Mathf.Abs(xInput) > 0.05f)
        {
            _player.SetVelocity(xInput * _player.moveSpeed * 0.8f, _rigidbody.velocity.y);
        }
        
        if (_player.IsWallDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.WallSlide);
        }
    }
}