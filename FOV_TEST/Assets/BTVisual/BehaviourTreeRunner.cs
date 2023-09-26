using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        //Æ®¸®
        private BehaviourTree _tree;

        private void Start()
        {
            _tree = ScriptableObject.CreateInstance<BehaviourTree>();
            var debugNode1 = ScriptableObject.CreateInstance<DebugNode>();
            debugNode1.message = "Behaviour Wow1";
            var debugNode2 = ScriptableObject.CreateInstance<DebugNode>();
            debugNode2.message = "Behaviour Wow2";
            var debugNode3 = ScriptableObject.CreateInstance<DebugNode>();
            debugNode3.message = "Behaviour Wow3";

            var waitNode = ScriptableObject.CreateInstance<WaitNode>();
            waitNode.duration = 2f;

            var SeqNode = ScriptableObject.CreateInstance<SequenceNode>();
            SeqNode.children.Add(waitNode);
            SeqNode.children.Add(debugNode1);
            SeqNode.children.Add(debugNode2);
            SeqNode.children.Add(debugNode3);

            var repeatNode = ScriptableObject.CreateInstance<RepeatNode>();
            repeatNode.child = SeqNode;

            

            _tree.rootNode = repeatNode;
        }

        private void Update()
        {
            _tree.Update();
        }
    }

}
