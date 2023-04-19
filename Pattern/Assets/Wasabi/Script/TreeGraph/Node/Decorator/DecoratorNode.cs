using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Wasabi.TreeGraph
{
    public abstract class DecoratorNode : Node
    {
        [HideInInspector] public Node child;

        public override Node Clone(GameObject gameObject)
        {
            DecoratorNode node = Instantiate(this);
            node.child = child.Clone(gameObject);
            return node;
        }
    }
}