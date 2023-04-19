using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class Cell
    {
        enum CellType { Normal, Blank }
        CellType cellType;
        internal bool IsNormal { get => cellType == CellType.Normal; }

    }

    public struct BlockMatch
    {
        public Queue<Vector2Int> matchBlocks;

    }

    public class Match3Board
    {


        readonly Vector2Int size = new Vector2Int(9, 9);
        Block[,] blocks = new Block[0, 9];
        Queue<Vector2Int> usedBlocks = new Queue<Vector2Int>();
        internal bool behaviour { get => usedBlocks.Count > 0; }


        internal System.Func<Block> blockGen;

        Queue<Vector2Int> activeBlocks = new Queue<Vector2Int>();

        public Match3Board(System.Func<Block> blockGen)
        {
            this.blockGen = blockGen;

        }

        internal void SwapBlock(Vector2Int block, Vector2Int target)
        {

        }

        void Move(Vector2Int start, Vector2Int end)
        {

        }

        internal void Drop()
        {

        }

        internal IEnumerator WaitBlocks()
        {
            while (usedBlocks.Count > 0)
            {
                Vector2Int pos = usedBlocks.Dequeue();
                if (blocks[pos.y, pos.x].IsIdle) continue;
                yield return new WaitUntil(() => blocks[pos.y, pos.x].IsIdle);
            }
        }

    }
}