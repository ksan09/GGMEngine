using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public abstract class EnemyBrain : MonoBehaviour
{
    [SerializeField]
    protected Transform _targetTrm;


    protected EnemyAttack _enemyAttack;
    protected NavMeshAgent _navAgent;
    public NavMeshAgent NavAgent => _navAgent;

    public NodeActionCode currentCode;

    protected virtual void Awake()
    {
        _enemyAttack = GetComponent<EnemyAttack>();
        _navAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {

    }

    public void TryToTalk(string text)
    {
        Debug.Log(text);
    }
    public void LookTarget()
    {
        
        Vector3 dir = transform.position - _targetTrm.position;
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle - 90, 0));
    }

    public abstract void Attack();
}
