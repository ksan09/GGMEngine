using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
using TMPro;
using System;

public abstract class EnemyBrain : MonoBehaviour
{
    [SerializeField]
    protected Transform _targetTrm;

    private UIBar _uiBar;

    protected EnemyAttack _enemyAttack;
    protected NavMeshAgent _navAgent;
    public NavMeshAgent NavAgent => _navAgent;

    public NodeActionCode currentCode;

    protected virtual void Awake()
    {
        _enemyAttack = GetComponent<EnemyAttack>();
        _navAgent = GetComponent<NavMeshAgent>();

        _uiBar = transform.Find("UIBar").GetComponent<UIBar>();
    }

    protected virtual void Start()
    {

    }

    private Coroutine _coroutine;
    private float _timer = 1f;

    public void TryToTalk(string text)
    {
        Debug.Log(text);
        _uiBar.DialogText = text;
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(PanelFade(_timer));
    }

    IEnumerator PanelFade(float timer)
    {
        _uiBar.IsOn = true;
        yield return new WaitForSeconds(timer);
        _uiBar.IsOn = false;
    }

    public void LookTarget()
    {
        
        Vector3 dir = transform.position - _targetTrm.position;
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle - 90, 0));
    }

    public abstract void Attack();
}
