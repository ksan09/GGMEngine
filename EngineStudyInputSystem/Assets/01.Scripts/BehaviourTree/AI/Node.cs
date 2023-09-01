
namespace BehaviourTree
{
    // ����� ����
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

        //����� ���¸� �Ǵ��ϴ� �޼���
        public abstract NodeState Evaluate();
    }
}

