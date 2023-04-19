using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wasabi.TreeGraph
{


    public class WaitNode : ActionNode
    {
        [Header("WaitNode")]
        public float duration = 1;
        float startTime;
        protected override void OnStart()
        {
            startTime = Time.time;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (Time.time - startTime > duration)
            {
                return State.Success;
            }
            return State.Running;
        }
    }

}