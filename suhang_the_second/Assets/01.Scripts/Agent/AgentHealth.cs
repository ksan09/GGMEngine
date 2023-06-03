using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

public class AgentHealth : MonoBehaviour, IDamageable
{
    //������Ʈ ��Ʈ�ѷ� �ȸ���Ŵ�..������.. ���⼭ ��Ʈ��
    [SerializeField]
    private int _maxHP = 100;
    private int _currentHP;

    public bool IsDead = false;

    private AgentAnimator _agentAnimator;
    private RigBuilder _rigBuilder;

    public UnityEvent OnDeadTrigger = null;

    private void Awake()
    {
        _agentAnimator = transform.Find("PlayerRig/Visual").GetComponent<AgentAnimator>();
        _rigBuilder = transform.Find("PlayerRig/Visual").GetComponent<RigBuilder>();
    }
    private void Start()
    {
        _currentHP = _maxHP;
    }

    public void OnDamage(int damage, Vector3 point, Vector3 normal)
    {
        if (IsDead) return;

        _currentHP -= damage;
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
        if(_currentHP <= 0)
        {
            DeadProcess();
        }

        OnDeadTrigger?.Invoke();
        Debug.Log("�ƾ�");
    }

    private void DeadProcess()
    {
        IsDead = true;
        _rigBuilder.enabled = false; //���׺��� ��������
        _agentAnimator.SetDeadAnimation();
    }
}
