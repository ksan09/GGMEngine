using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class ReloadNode : Node
{
    EnemyBrain _brain;
    EnemyGunAttack _enemyGunAttack;

    public ReloadNode(EnemyBrain brain, EnemyGunAttack enemyGunAttack)
    {
        _brain = brain;
        _enemyGunAttack = enemyGunAttack;
        _code = NodeActionCode.RELOADING;
    }

    public override NodeState Evaluate()
    {
        if(_brain.currentCode != _code)
        {
            _brain.TryToTalk("¿Á¿Â¿¸..");
            _enemyGunAttack.Reloading();
            _brain.currentCode = _code;
        }

        return NodeState.RUNNING;
    }
}
