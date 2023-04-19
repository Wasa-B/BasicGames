using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WasabiTool
{
    public class PlayerInput : InputHandler
    {
        Queue<Command> recode = new Queue<Command>();

        Command rightMove;

        public override Command HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) return rightMove;
            return base.HandleInput();
        }
    }
}