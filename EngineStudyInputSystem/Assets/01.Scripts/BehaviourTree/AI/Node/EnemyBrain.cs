using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public abstract class EnemyBrain : MonoBehaviour
{
    [SerializeField]
    protected Transform _targetTrm;

    protected NavMeshAgent _navAgent;
    public NavMeshAgent NavAgent => _navAgent;

    public NodeActionCode currentCode;

    protected virtual void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {

    }

    public void TryToTalk(string text)
    {
        Debug.Log(text);
    }
}
