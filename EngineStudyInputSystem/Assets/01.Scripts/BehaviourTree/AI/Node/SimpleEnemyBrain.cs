using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using System;
using UnityEngine.InputSystem;

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

        if(Keyboard.current.pKey.IsPressed())
        {
            Attack();
        }
    }

    private void ConstructAITree()
    {
        Transform me = transform;

        NeedReloding needReloding = new NeedReloding(_enemyAttack as EnemyGunAttack);
        ReloadNode reloadNode = new ReloadNode(this, _enemyAttack as EnemyGunAttack);
        Sequence reloadSeq =
            new Sequence(new List<Node>() { needReloding, reloadNode });

        RangeNode shootingRange = new RangeNode(8f, _targetTrm, me);
        ShootNode shootNode = new ShootNode(_navAgent, this);
        Sequence attackSeq = 
            new Sequence(new List<Node>() { shootingRange, shootNode });

        RangeNode chaseRange = new RangeNode(15f, _targetTrm, me);
        ChaseNode chaseNode = new ChaseNode(_navAgent, _targetTrm, this);
        Sequence chaseSeq =
            new Sequence(new List<Node>() { chaseRange, chaseNode });

        _topNode = new Selector(new List<Node>() { reloadSeq, attackSeq, chaseSeq }); 
    }

    public override void Attack()
    {
        _enemyAttack.Attack();
    }
}
