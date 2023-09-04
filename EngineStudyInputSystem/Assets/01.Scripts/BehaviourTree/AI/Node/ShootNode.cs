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
        _agent.isStopped = true;    // ����

        if(_brain.currentCode != _code)
        {
            _brain.TryToTalk("���ݻ��·� ��ȯ");
            _brain.currentCode = _code;
        }

        if(_coolTime + _lastFireTime <= Time.time)
        {
            _brain.TryToTalk("����");
            _lastFireTime = Time.time;
        }

        return NodeState.RUNNING;
    }
}
