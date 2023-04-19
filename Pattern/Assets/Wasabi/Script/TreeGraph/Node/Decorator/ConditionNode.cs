using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wasabi.TreeGraph
{
    public class ConditionNode : DecoratorNode
    {


        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            return State.Success;
        }
    }
}