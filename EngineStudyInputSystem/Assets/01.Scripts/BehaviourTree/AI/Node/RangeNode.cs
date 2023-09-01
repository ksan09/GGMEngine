using System;
using BehaviourTree;
using UnityEngine;

public class RangeNode : Node
{
    private float _range;
    private Transform _target;
    private Transform _transform;

    public RangeNode(float range,Transform target, Transform transform)
    {
        _range = range;
        _target = target;
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(_transform.position, _target.position);

        _nodeState = (_range >= distance) ? NodeState.SUCCESS : NodeState.FAILURE;
        return _nodeState;
    }
}
