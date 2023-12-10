using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float _dashStartTime;
    private float _dashDirection;

    public PlayerDashState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        float xInput = _player.PlayerInput.XInput;

        if (Mathf.Abs(xInput) > 0.05f)
            _dashDirection = Mathf.Sign(xInput);
        else
            _dashDirection = _player.FacingDirection;

        _dashStartTime = Time.time;
        _player.Skill.GetSkill<CloneSkill>().CreateCloneOnDashStart();
    }

    public override void Exit()
    {
        _player.Skill.GetSkill<CloneSkill>().CreateCloneOnDashEnd();
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _player.SetVelocity(_dashDirection * _player.dashSpeed, 0);
        if(_dashStartTime + _player.dashDuration <= Time.time)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
