using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class NeedReloding : Node
{
    EnemyGunAttack _enemyGunAttack;

    public NeedReloding(EnemyGunAttack enemyGunAttack)
    {
        _enemyGunAttack = enemyGunAttack;
    }

    public override NodeState Evaluate()
    {
        if(_enemyGunAttack.NeedReloading())
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
