using UnityEngine;
using System;

namespace Wasabi.TreeGraph
{
    public class DebugLogNode : ActionNode
    {
        public string message;
        public bool start, stop;
        public bool update = true;

        string DebugString()
        {

            return message + " : " + Time.frameCount;
        }

        protected override void OnStart()
        {
            if (start)
                Debug.Log($"OnStart {DebugString()}");
        }
        protected override void OnStop()
        {
            if (stop)
                Debug.Log($"OnStop {DebugString()}");
        }
        protected override State OnUpdate()
        {
            if (update)
                Debug.Log($"OnUpdate {DebugString()}");
            return Node.State.Success;
        }
    }

}
