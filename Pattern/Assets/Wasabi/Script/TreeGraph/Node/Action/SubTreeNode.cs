using UnityEngine;
namespace Wasabi.TreeGraph
{
    public class SubTreeNode : ActionNode
    {
        public BehaviourTree subTree;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            switch (subTree.Update())
            {
                case State.Running:
                    break;
                case State.Failure:
                    break;
                case State.Success:
                    subTree.rootNode.state = State.Running;
                    return State.Success;
            }

            return State.Running;
        }
        public override Node Clone(GameObject gameObject)
        {
            var clone = base.Clone(gameObject) as SubTreeNode;
            clone.subTree = subTree.Clone(gameObject);
            return clone;
        }
    }
}
