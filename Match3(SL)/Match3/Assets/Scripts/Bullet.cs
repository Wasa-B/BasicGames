using UnityEngine;
using UnityEngine.UI;
namespace SweetRoad
{
    public class Bullet : MonoBehaviour
    {
        internal Vector2Int pos;
        public  float speed = 10;
        internal Vector2Int dir;
        internal bool moveEnd = false;
        public bool whiteCandy = false;

        Vector2 candySize;
        private void Start()
        {
            candySize = GameManager.GetCandySize();

        }

        private void FixedUpdate()
        {
            if (moveEnd == false) Move();
        }
        void Move()
        {
            if (GameManager.OnBoardPosition(pos+dir) == false)
            {
                GetComponent<Image>().color = Color.clear;
                moveEnd = true;
                return;
            }
            Vector2 moveDir = (GameManager.GetPosition(pos + dir) - (Vector2)transform.position);
            transform.Translate(moveDir.normalized*speed * candySize.y * Time.fixedDeltaTime, Space.World);
            Vector2 moveDir2 = (GameManager.GetPosition(pos + dir) - (Vector2)transform.position);
            
            if (moveDir2.x * moveDir.x <= 0&& moveDir2.y * moveDir.y <= 0)
            {
                
                GameManager.UseCandy(pos, (Random.Range(0,2) == 0?1:-1) * new Vector2Int(dir.y,dir.x));
                pos += dir;
                if (whiteCandy && GameManager.GetTile(pos).type != TileType.Normal)
                {
                    GameManager.UpdateGoal(GameManager.Goal.HoleIn);
                    moveEnd = true;
                }
            }

        }
    }
}