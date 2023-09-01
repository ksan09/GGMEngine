using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Inverter : Node
    {
        //해당 노드의 결과를 반전
        protected Node _node;

        public Inverter(Node node)
        {
            _node = node;
        }

        public override NodeState Evaluate()
        {
            switch(_node.Evaluate())
            {
                case NodeState.RUNNING:
                    _nodeState = NodeState.RUNNING;
                    break;
                case NodeState.SUCCESS:
                    _nodeState = NodeState.FAILURE;
                    break;
                case NodeState.FAILURE:
                    _nodeState = NodeState.SUCCESS;
                    break;
                default:
                    break;
            }
            return _nodeState;
        }
    }
}