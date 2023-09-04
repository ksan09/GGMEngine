using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using System;

public class SimpleEnemyBrain : EnemyBrain
{
    private Node _topNode;

    protected override void Start()
    {
        base.Start();
        ConstructAITree();
    }

    private void Update()
    {
        _topNode.Evaluate();

        if(_topNode.nodeState == NodeState.FAILURE && currentCode != NodeActionCode.NONE)
        {
            currentCode = NodeActionCode.NONE;
            _navAgent.isStopped = true;
            TryToTalk("...");
        }

    }

    private void ConstructAITree()
    {
        Transform me = transform;
        RangeNode shootingRange = new RangeNode(8f, _targetTrm, me);
        ShootNode shootNode = new ShootNode(_navAgent, this, 1f);

        Sequence attackSeq = 
            new Sequence(new List<Node>() { shootingRange, shootNode });

        RangeNode chaseRange = new RangeNode(15f, _targetTrm, me);
        ChaseNode chaseNode = new ChaseNode(_navAgent, _targetTrm, this);
        Sequence chaseSeq =
            new Sequence(new List<Node>() { chaseRange, chaseNode });

        _topNode = new Selector(new List<Node>() { attackSeq, chaseSeq }); 
    }
}
