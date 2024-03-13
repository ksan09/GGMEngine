using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private CrystalSkill _skill;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private readonly int _hashExplodeTrigger = Animator.StringToHash("explode");

    private float _lifeTimer;
    private bool _isDestroyed = false;

    private Transform _targetTrm = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetUpCrystal(CrystalSkill skill)
    {
        _skill = skill;
        _lifeTimer = skill.timeOut;
        _isDestroyed = false;
    }

    private void Update()
    {
        if(_skill.canMove)
        {
            if(_targetTrm == null)
            {
                _targetTrm = _skill.FindClosestEnemy(transform, _skill.findEnemyRadius);
            }
            else
            {
                ChaseToTarget();
            }
        }

        _lifeTimer -= Time.deltaTime;
        if(_lifeTimer <= 0 && !_isDestroyed)
        {
            EndOfCrystal();
        }
    }

    private void ChaseToTarget()
    {
        if (_isDestroyed) return;

        transform.position = Vector2.MoveTowards(
            transform.position, _targetTrm.position, _skill.moveSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, _targetTrm.position) < 1f)
        {
            EndOfCrystal();
        }
    }

    public void EndOfCrystal()
    {
        _isDestroyed = true;
        if(_skill.canExplode)
        {
            transform.DOScale(Vector3.one * 2.5f, 0.08f);
            _animator.SetTrigger(_hashExplodeTrigger);
        }
        else
            DestroySelf();
    }

    private void DestroySelf(float tweenTime = 0.4f)
    {
        _skill.UnlinkCrystal();
        _spriteRenderer.DOFade(0f, tweenTime).OnComplete(() => Destroy(gameObject));
    }

    private void EndOfExplosionAnimation()
    {
        
        transform.DOKill();
        DestroySelf(0.1f);
    }
}
