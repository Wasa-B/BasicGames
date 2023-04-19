using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class Match3Manager : MonoBehaviour
    {

        Match3Board board;
        PoolManager poolManager = new PoolManager();

        public GridLayoutGroup blockGridLayout;
        public Block blockPrefab;


        bool userBehavior = true;

        private void Awake()
        {
            board = new Match3Board(GenerateBlock);
            StartCoroutine(TestRoutin());
        }

        IEnumerator TestRoutin()
        {
            for (int i = 0; i < 81; i++)
            {
                var b = GenerateBlock();
            }
            yield return null;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var cell = blockGridLayout.transform.GetChild(i + j);
                }
            }

        }

        IEnumerator GameRoutine()
        {
            while (true)
            {
                if (board.behaviour)
                {
                    userBehavior = false;
                    yield return board.WaitBlocks();
                    userBehavior = true;
                }
                else
                    yield return null;
            }
        }

        Block GenerateBlock()
        {
            var block = poolManager.GetObject<Block>(blockPrefab, Instantiate);
            block.transform.SetParent(blockGridLayout.transform);
            return block;
        }
    }
}