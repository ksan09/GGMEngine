using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerStateEnum
{
    Idle,
    Move,
    Jump,
    Fall,
    Dash,
    WallSlide,
    WallJump,
    PrimaryAttack
}

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }
    public Dictionary<PlayerStateEnum, PlayerState> StateDictinary { get; private set; } = new Dictionary<PlayerStateEnum, PlayerState>();
    
    private Player _player;

    public void Initalize(PlayerStateEnum startState, Player player)
    {
        _player = player;
        CurrentState = StateDictinary[startState];
        CurrentState.Enter();
    }

    public void AddState(PlayerStateEnum state, PlayerState playerState)
    {
        StateDictinary.Add(state, playerState);

    }

    public void ChangeState(PlayerStateEnum state)
    {
        // �÷��̾ ó�°ų� ���� �־ ���¸� ��ȯ���� ���ϴ� ���

        CurrentState.Exit();
        CurrentState = StateDictinary[state];
        CurrentState.Enter();
    }

}
