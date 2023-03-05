using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace SweetRoad
{
    public class GameManager : MonoBehaviour
    {
        static GameManager instance;
        static GameBoard gameBoard;

        public int behaviourCount = 21;
        public Text behaviourCountText;

        public enum Goal {HoleIn}
        public Goal goal = Goal.HoleIn;
        public int goalScore = 0;
        public Text goalText;

        public GameObject gameOverUI;
        public GameObject gameClearUI;

        static bool playerUse = true;


        private void Awake()
        {
            instance = this;
            playerUse = true;
            gameBoard = GetComponent<GameBoard>();
            gameBoard.swapCandyAction = UserBehavoiur;
            //StartCoroutine(GameRoutine());
            behaviourCountText.text = behaviourCount.ToString();
            goalText.text = goalScore.ToString();
        }
        
        public void MoveScene(string sceneName)
        {

        }

        void UserBehavoiur()
        {
            if (playerUse)
            {
                behaviourCount -= 1;
                if (behaviourCount <= 0)
                {
                    behaviourCount = 0;
                    StartCoroutine(GameEnd());
                }
            }
           
            behaviourCountText.text = behaviourCount.ToString();
        }
        void UpdateGoal()
        {
            goalScore -= 1;
            if(goalScore < 0) goalScore = 0;
            goalText.text = goalScore.ToString();
            if(playerUse && goalScore == 0)
                StartCoroutine(GameEnd());
        }


        IEnumerator GameEnd()
        {
            playerUse = false;
            yield return new WaitForSeconds(1);
            yield return new WaitUntil(()=>gameBoard.playerBehaviour);
            yield return new WaitForSeconds(3);

            if (goalScore <= 0)
            {
                gameClearUI.SetActive(true);
            }
            else
            {
                gameOverUI.SetActive(true);
            }
        }

        internal static Vector2 GetPosition(Vector2Int pos)
        {
            return gameBoard.GetPosition(pos);
        }
        internal static Tile GetTile(Vector2Int pos)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x > 8 || pos.y > 8) return null; 
            return gameBoard.tiles[pos.x, pos.y];
        }
        internal static void SwapCandy(Candy candy, Vector2 dir)
        {
            if (playerUse)
            {
                gameBoard.SwapCandy(candy, dir);
            }
        }
        internal static void UseCandy(Vector2Int pos, Vector2 dir)
        {
            gameBoard.UseCandy(pos, dir);
        }
        internal static Vector2 GetCandySize()
        {
            return GetPosition(Vector2Int.zero) - GetPosition(Vector2Int.one);
        }

        internal static bool OnBoardPosition(Vector2Int pos)
        {
            return gameBoard.OnBoardPosition(pos);
        }
        internal static void UpdateGoal(Goal goal)
        {
            if(instance.goal == goal)
            {
                instance.UpdateGoal();
            }
        }
        internal static void Log(CandyType candyType)
        {

        }
        internal static void Log(string str)
        {

        }
    }   

}