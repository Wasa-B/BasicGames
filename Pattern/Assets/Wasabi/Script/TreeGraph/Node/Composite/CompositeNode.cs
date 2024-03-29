using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wasabi.TreeGraph
{


    public abstract class CompositeNode : Node
    {
        [HideInInspector] public List<Node> children = new List<Node>();
        public override Node Clone(GameObject gameObject)
        {
            CompositeNode node = Instantiate(this);
            node.children = children.ConvertAll(c=>c.Clone(gameObject));
            return node;
        }
    }

}