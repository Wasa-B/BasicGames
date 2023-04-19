using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WasabiTool
{
    public class Command
    {
        public virtual void Execute(WBehaviour actor) { }
        public virtual void Undo(WBehaviour actor) { }
    }

    public class JumpCommand : Command
    {
        public override void Execute(WBehaviour actor)
        {
            actor.Jump();
        }
    }

    public class MoveUnitCommand : Command
    {
        int x, y;
        public MoveUnitCommand(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public override void Execute(WBehaviour actor)
        {
            
        }
    }
}