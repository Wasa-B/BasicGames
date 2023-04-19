using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wasabi.TreeGraph
{
    public class SequencerNode : CompositeNode
    {
        int current;

        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {


            while (current < children.Count)
            {
                var child = children[current];
                switch (child.Update())
                {
                    case State.Running:
                        return State.Running;
                    case State.Failure:
                        return State.Failure;
                    case State.Success:
                        current++;
                        break;
                }
            }

            return current == children.Count ? State.Success : State.Running;
        }
    }
}
