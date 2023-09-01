using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class ChaseNode : Node
{
    private NavMeshAgent _agent;
    private Transform _target;
    private EnemyBrain _enemyBrain;

    public ChaseNode(NavMeshAgent agent, Transform target, EnemyBrain enemyBrain)
    {
        _agent = agent;
        _target = target;
        _enemyBrain = enemyBrain;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(_agent.transform.position, _target.position);
        if(distance > 0.2f)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_target.position);

            if(_nodeState != NodeState.RUNNING)
            {
                _enemyBrain.TryToTalk("추적! 시작한다!");
                _nodeState = NodeState.RUNNING;
            }
        }
        else
        {
            _agent.isStopped = true;
            _nodeState = NodeState.SUCCESS;
        }
        return _nodeState;
    }
}
