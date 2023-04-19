using UnityEngine;

namespace Match3
{
    public class Block : PoolObject
    {
        internal Vector2Int index;

        enum StateType {Idle, Action};
        StateType state = StateType.Idle;

        static internal System.Func<Vector2Int, Vector2> worldPosition;
        internal bool IsIdle { get => state == StateType.Idle;}


        internal void MoveBlock()
        {

        }

    }
}