using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField]
    protected Transform _targetTrm;

    protected NavMeshAgent _navAgent;
    public NavMeshAgent NavAgent => _navAgent;

    protected virtual void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }

    public void TryToTalk(string text)
    {
        Debug.Log(text);
    }
}
