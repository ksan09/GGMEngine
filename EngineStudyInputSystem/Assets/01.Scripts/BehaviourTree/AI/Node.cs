
namespace BehaviourTree
{
    // 노드의 상태
    public enum NodeState
    {
        SUCCESS = 1,
        FAILURE = 2,
        RUNNING = 3,
    }

    public abstract class Node
    {
        protected NodeState _nodeState;

        public NodeState nodeState => _nodeState;

        //노드의 상태를 판단하는 메서드
        public abstract NodeState Evaluate();
    }
}

