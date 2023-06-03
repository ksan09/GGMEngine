using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimator : MonoBehaviour
{
    private readonly int _hashMoveX = Animator.StringToHash("move_x");
    private readonly int _hashMoveY = Animator.StringToHash("move_y");
    private readonly int _hashIsMoving = Animator.StringToHash("is_moving");

    private readonly int _hashSpeed = Animator.StringToHash("speed");
    private readonly int _hashIsAttack = Animator.StringToHash("is_attack");
    private readonly int _hashAttack = Animator.StringToHash("attack");
    private readonly int _hashIsDead = Animator.StringToHash("is_dead");
    private readonly int _hashDead = Animator.StringToHash("dead");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMoving(bool value)
    {
        _animator.SetBool(_hashIsMoving, value);
    }

    public void SetDirection(Vector2 dir)
    {
        _animator.SetFloat(_hashMoveX, dir.x);
        _animator.SetFloat(_hashMoveY, dir.y);
    }

    public void SetSpeed(float speed)
    {
        _animator.SetFloat(_hashSpeed, speed);
    }

    public void SetDeadAnimation()
    {
        _animator.SetBool(_hashIsDead, true);
        _animator.SetTrigger(_hashDead);
    }

    public void SetAttackAnimation(bool value)
    {
        _animator.SetBool(_hashIsAttack, value);
        if (value)
        {
            _animator.SetTrigger(_hashAttack);
        }
        else
        {
            _animator.ResetTrigger(_hashAttack);
        }
    }

    public Action OnAnimationEndTrigger = null;
    public Action OnAttackAnimationTrigger = null;

    public void OnAnimationEnd()
    {
        OnAnimationEndTrigger?.Invoke();
    }

    public void OnAttackAnimation()
    {
        OnAttackAnimationTrigger?.Invoke();
    }
}
