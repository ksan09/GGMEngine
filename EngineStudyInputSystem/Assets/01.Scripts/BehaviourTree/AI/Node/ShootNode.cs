using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node
{
    private NavMeshAgent _agent;
    private EnemyBrain _brain;

    public ShootNode(NavMeshAgent agent, EnemyBrain brain)
    {
        _agent = agent;
        _brain = brain;
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
        _brain.LookTarget();
        _brain.Attack();

        return NodeState.RUNNING;
    }
}
