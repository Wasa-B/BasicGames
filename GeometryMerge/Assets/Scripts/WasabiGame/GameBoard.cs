using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WasabiGame
{

    public class GameBoard : MonoBehaviour
    {
        public static GameBoard instance;

        enum BoardObject { NULL, Tile, Player, Enemy, Bomb, Wall, Count };
        Vector2 playerPos;

        const int boardSize = 7;

        BoardObject[,] board = new BoardObject[boardSize, boardSize];
        Tile[,] tiles = new Tile[boardSize, boardSize];
        public GameObject tilePrefabs;

        private void Awake()
        {
            instance = this;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = BoardObject.Tile;
                    var tile = Instantiate<GameObject>(tilePrefabs);
                    tile.transform.position = new Vector2(i, j);
                    tile.transform.SetParent(transform);
                    tiles[i,j] = tile.GetComponent<Tile>();
                    tiles[i, j].pos = new Vector2Int(i, j);
                }
            }

        }

        public Vector2 GetPosition(Vector2 position, Vector2 dir)
        {
            var pos = Vector2Int.FloorToInt(position);

            if (dir.y > 0)
            {
                for(int i = pos.y; i < boardSize -1; ++i)
                {
                    if (board[pos.x, i+1] == BoardObject.Wall) return new Vector2(pos.x,i);
                }
                return new Vector2(pos.x, boardSize -1);
            }
            else if (dir.y < 0)
            {
                for (int i = pos.y; i > 0; --i)
                {
                    if (board[pos.x, i - 1] == BoardObject.Wall) return new Vector2(pos.x, i);
                }
                return new Vector2(pos.x, 0);
            }
            else if (dir.x > 0)
            {
                for (int i = pos.x; i < boardSize -1; ++i)
                {
                    if (board[i+1,pos.y] == BoardObject.Wall) return new Vector2( i, pos.y);
                }
                return new Vector2(boardSize - 1, pos.y);
            }
            else if (dir.x < 0)
            {
                for (int i = pos.x; i > 0; --i)
                {
                    if (board[i - 1, pos.y] == BoardObject.Wall) return new Vector2(i, pos.y);

                }
                return new Vector2(0, pos.y);

            }

            return position;

        }


        internal System.Action moveUpdate;
        public void IntoTile(Vector2Int pos)
        {
            board[pos.x, pos.y] = BoardObject.Wall;
            tiles[pos.x, pos.y].GenerateWall();
            if (moveUpdate != null) moveUpdate();
        }

        public void WallDelete(Vector2Int pos)
        {
            board[pos.x,pos.y] = BoardObject.Tile;
        }

    }

}