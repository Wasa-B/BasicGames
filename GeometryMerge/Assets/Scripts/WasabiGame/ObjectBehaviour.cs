
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WasabiGame
{



    public class ObjectBehaviour : MonoBehaviour
    {
        protected Queue<BehaviourCommand> fixedUpdateCommands = new Queue<BehaviourCommand>();
        public MoveCommand moveCommand;

        protected virtual void Awake()
        {
            moveCommand = moveCommand.Clone(this.gameObject) as MoveCommand;
            
        }
        protected virtual void FixedUpdate()
        {
            if (fixedUpdateCommands.Count > 0)
            {
                var command = fixedUpdateCommands.Peek();
                if (command.Update() == BehaviourCommand.State.Success)
                    fixedUpdateCommands.Dequeue();
            }

            if (moveCommand.state == BehaviourCommand.State.Running)
            {
                moveCommand.Update();
            }
        }
    }
}