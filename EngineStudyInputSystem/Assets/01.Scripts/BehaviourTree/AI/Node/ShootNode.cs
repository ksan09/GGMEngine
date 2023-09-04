using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node
{
    private NavMeshAgent _agent;
    private EnemyBrain _brain;

    private float _coolTime = 0;
    private float _lastFireTime = 0;

    public ShootNode(NavMeshAgent agent, EnemyBrain brain, float coolTime)
    {
        _agent = agent;
        _brain = brain;
        _coolTime = coolTime;
        _code = NodeActionCode.SHOOT;
    }

    public override NodeState Evaluate()
    {
        _agent.isStopped = true;    // 정지

        if(_brain.currentCode != _code)
        {
            _brain.TryToTalk("공격상태로 전환");
            _brain.currentCode = _code;
        }

        if(_coolTime + _lastFireTime <= Time.time)
        {
            _brain.TryToTalk("공격");
            _lastFireTime = Time.time;
        }

        return NodeState.RUNNING;
    }
}
