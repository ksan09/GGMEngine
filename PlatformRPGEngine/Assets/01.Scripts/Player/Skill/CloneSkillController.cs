using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private int _attackCategoryCount = 3;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");
    private int _facingDirection = 1;

    private CloneSkill _skill;
    // private float _lifeTime;
    // private float _curLifetime = 0;
    // private bool _isDestroyed = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SetUpClone(CloneSkill skill, Transform origin, Vector3 offset)
    {
        _animator.SetInteger(_comboCounterHash, Random.Range(0, _attackCategoryCount));
        _skill = skill;
        transform.position = origin.position + offset;

        // 여기서 가장 가까운 적을 바라보도록 설정해야 함
        FacingClosestTarget();

        // FadeOut
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(_skill.cloneDuration)
            .Append(_spriteRenderer.DOFade(0, 0.1f)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            }));
    }

    private void FacingClosestTarget()
    {
        Transform target = _skill.FindClosestEnemy(transform, _skill.findEnemyRadius);
        if(target != null)
        {
            if(target.position.x < transform.position.x)
            {
                _facingDirection = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }

    public void AnimationEndTrigger()
    {

    }
}
