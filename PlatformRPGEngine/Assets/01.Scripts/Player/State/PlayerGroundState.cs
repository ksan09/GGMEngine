using System;
using UnityEngine;

public abstract class PlayerGroundState : PlayerState
{
    protected PlayerGroundState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.JumpEvent += HandleJumpEvent;
        _player.PlayerInput.PrimaryAttackEvent += HandlePrimaryAttackEvent;
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.JumpEvent -= HandleJumpEvent;
        _player.PlayerInput.PrimaryAttackEvent -= HandlePrimaryAttackEvent;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if(_player.IsGroundDetected() == false)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Fall);
        }
    }

    private void HandlePrimaryAttackEvent()
    {
        if (_player.IsGroundDetected())
            _stateMachine.ChangeState(PlayerStateEnum.PrimaryAttack);
    }

    private void HandleJumpEvent()
    {
        if (_player.IsGroundDetected())
            _stateMachine.ChangeState(PlayerStateEnum.Jump);
    }

}