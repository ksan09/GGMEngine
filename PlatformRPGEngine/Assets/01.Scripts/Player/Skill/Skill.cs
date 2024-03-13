using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public delegate void CoolDownNotify(float current, float total);

public abstract class Skill : MonoBehaviour
{
    public bool skillEnalbled = false;

    [SerializeField] protected LayerMask _whatIsEnemy;
    [SerializeField] protected float _cooldown;
    protected float _cooldownTimer;
    protected Player _player;

    [SerializeField] protected PlayerSkill _skillType;

    public event CoolDownNotify OnCoolDown;
    [SerializeField] protected int _maxColliderCount = 5;
    protected Collider2D[] _checkColliders;
    protected ContactFilter2D _contactFilter;

    protected virtual void Start()
    {
        _contactFilter = new ContactFilter2D { layerMask = _whatIsEnemy, useLayerMask = true };
        _checkColliders = new Collider2D[_maxColliderCount];
        _player = GameManager.Instance.Player;
    }

    protected virtual void Update()
    {
        if(_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            if(_cooldownTimer <= 0)
            {
                _cooldownTimer = 0;
            }

            OnCoolDown?.Invoke(current: _cooldownTimer, total: _cooldown);
        }
    }

    public virtual bool AtemptUseSkill()
    {
        if(_cooldownTimer <= 0 && skillEnalbled)
        {
            _cooldownTimer = _cooldown;
            UseSkill();
            return true;
        }
        Debug.Log("Skill cooldown or locked!");
        return false;
    }

    // ������ � ���ؼ� �� ���� �ߵ���ų �� �ʿ�
    public virtual void UseSkill()
    {
        //��ų ����� �ߵ��� ȿ������ �����ؼ� ���ָ� �ȴ�.
        SkillManager.Instance.UseSkillFeedback(_skillType);
    }

    public virtual void UseSkillWithoutCooltimeAndEffect()
    {

    }

    // ���� ����� ���� ã���ִ� ������ �ϳ� �ʿ�
    public virtual Transform FindClosestEnemy(Transform checkTransform, float radius)
    {
        Transform targetEnemy = null;
        int cnt = Physics2D.OverlapCircle(checkTransform.position, radius, _contactFilter, _checkColliders);

        float closestDistance = Mathf.Infinity;
        for(int i = 0; i < cnt; i++)
        {
            Collider2D collider = _checkColliders[i];

            float distance = Vector2.Distance(checkTransform.position, collider.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                targetEnemy = collider.transform;
            }
        }

        return targetEnemy;

    }
}
