using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasabi.Attribute;
using UnityEngine;

namespace Wasabi.TreeGraph
{
    public class RepeatNode : DecoratorNode
    {
        [Header("RepeatNode")]

        public bool infinity = true;
        [ValueEqual(nameof(infinity),false)]
        public int repeatCount = 1;
        int current = 0;

        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            switch (child.Update())
            {
                case State.Running:
                    break;
                case State.Failure:
                    break;
                case State.Success:
                    current++;
                    break;
            }
            if (infinity == false && current >= repeatCount) return State.Success;

            return State.Running;
        }
    }
}
