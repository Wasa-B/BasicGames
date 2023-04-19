using UnityEngine;

namespace Wasabi.TreeGraph
{
    public class WaitNextNode : DecoratorNode
    {
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
                switch (child.Update())
                {
                    case State.Running:
                        break;
                    case State.Failure:
                        break;
                    case State.Success:
                        return State.Success;
                }
                return State.Running;
            }
            return State.Running;
        }
    }
}
