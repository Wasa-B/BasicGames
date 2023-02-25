using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SweetRoad
{


    public class GameManager : MonoBehaviour
    {
        enum GameState { Idle, Explosion, Drop }

        static Queue<IEnumerator> behaviour = new Queue<IEnumerator>();

        static bool playerBehaviour = false;
        static Candy[,] candies = new Candy[9, 9];
        static List<Candy> candyPool = new List<Candy>();
        static Tile[,] tiles = new Tile[9, 9];
        static Transform boardTransform;



        public Transform tileLayout;
        public Candy candyPrefab;

        public CandyData candyData;
        public TileData tileData;
        public Sprite[] normalTiles;
        Dictionary<TileType, Sprite> tileSprites;
        static Dictionary<CandyType, Sprite> candySprites;
        static Vector2[,] tilePosition = new Vector2[9,9];


        private void Awake()
        {
            boardTransform = transform;
            candies = new Candy[9, 9];
            tiles = new Tile[9, 9];
            candySprites = candyData.GetDictionary();
            tileSprites = tileData.GetDictionary();
            Vector2 cellSize = GetComponentInChildren<GridLayoutGroup>().cellSize;
            for (int i = 0; i < tileLayout.childCount; ++i)
            {
                Tile tile = tileLayout.GetChild(i).GetComponent<Tile>();
                tiles[i % 9, i / 9] = tile;

                if (tile.type == TileType.Normal || tile.defaultCandy == false)
                    tile.GetComponent<Image>().sprite = normalTiles[i % 2];
                if (tile.defaultCandy == false) continue;

                Candy ncandy = Instantiate<Candy>(candyPrefab, tile.transform);
                ncandy.pos = new Vector2Int(i % 9, i / 9);
                candies[i % 9, i / 9] = ncandy;
                ncandy.type = tile.candy;
                ncandy.GetComponent<Image>().sprite = candySprites[tile.candy];
            }


            StartCoroutine(GameRoutine());
        }
        private IEnumerator Start()
        {
            yield return null;
            for (int i = 0; i < 9; ++i)
                for (int j = 0; j < 9; ++j)
                {
                    if (candies[i, j] != null)
                        candies[i, j].transform.SetParent(boardTransform);
                    tilePosition[i,j] = tiles[i, j].transform.position;
                }
        }

        static Candy GetCandy(Vector2Int pos)
        {
            return candies[pos.x, pos.y];
        }
        static bool CheckCandy(Vector2Int pos, CandyType type)
        {

            if (pos.x < 0 || pos.y < 0 || pos.x > 8 || pos.y > 8 || candies[pos.x, pos.y] == null) return false;
            if (candies[pos.x, pos.y].type == type) return true;
            return false;
        }

        static bool CheckMatch(Candy candy)
        {
            bool horizontal = false;
            bool vertical = false;
            bool square = false;
            bool[] side = new bool[4];
            List<Candy> matchCandy = new List<Candy>();

            //horizontal
            if (CheckCandy(candy.pos - Vector2Int.right, candy.type))
            {
                side[0] = true;
                if (CheckCandy(candy.pos - Vector2Int.right * 2, candy.type))
                {
                    horizontal = true;
                    matchCandy.Add(GetCandy(candy.pos - Vector2Int.right));
                    matchCandy.Add(GetCandy(candy.pos - Vector2Int.right * 2));
                }
            }
            if (CheckCandy(candy.pos + Vector2Int.right, candy.type))
            {
                side[1] = true;
                if (CheckCandy(candy.pos + Vector2Int.right * 2, candy.type))
                {
                    horizontal = true;
                    matchCandy.Add(GetCandy(candy.pos + Vector2Int.right));
                    matchCandy.Add(GetCandy(candy.pos + Vector2Int.right * 2));
                }
                if (side[0])
                {
                    horizontal = true;
                    matchCandy.Add(GetCandy(candy.pos - Vector2Int.right));
                    matchCandy.Add(GetCandy(candy.pos + Vector2Int.right));
                }
            }

            //Vertical
            if (CheckCandy(candy.pos - Vector2Int.up, candy.type))
            {
                side[2] = true;
                if (CheckCandy(candy.pos - Vector2Int.up * 2, candy.type))
                {
                    vertical = true;
                    matchCandy.Add(GetCandy(candy.pos - Vector2Int.up));
                    matchCandy.Add(GetCandy(candy.pos - Vector2Int.up * 2));
                }
            }
            if (CheckCandy(candy.pos + Vector2Int.up, candy.type))
            {
                side[3] = true;
                if (CheckCandy(candy.pos + Vector2Int.up * 2, candy.type))
                {
                    vertical = true;
                    matchCandy.Add(GetCandy(candy.pos + Vector2Int.up));
                    matchCandy.Add(GetCandy(candy.pos + Vector2Int.up * 2));
                }
                if (side[2])
                {
                    vertical = true;
                    matchCandy.Add(GetCandy(candy.pos - Vector2Int.up));
                    matchCandy.Add(GetCandy(candy.pos + Vector2Int.up));
                }
            }

            //Square
            if (side[0])
            {
                if (side[2] && CheckCandy(candy.pos + new Vector2Int(-1, -1), candy.type))
                {
                    square = true;
                    matchCandy.Add(GetCandy(candy.pos - Vector2Int.up));
                    matchCandy.Add(GetCandy(candy.pos + new Vector2Int(-1, -1)));
                }
                else if (side[3] && CheckCandy(candy.pos + new Vector2Int(-1, 1), candy.type))
                {
                    square = true;
                    matchCandy.Add(GetCandy(candy.pos + Vector2Int.up));
                    matchCandy.Add(GetCandy(candy.pos + new Vector2Int(-1, 1)));
                }
                if (square) matchCandy.Add(GetCandy(candy.pos - Vector2Int.right));
            }
            if (side[1])
            {
                if (side[2] && CheckCandy(candy.pos + new Vector2Int(1, -1), candy.type))
                {
                    square = true;
                    matchCandy.Add(GetCandy(candy.pos - Vector2Int.up));
                    matchCandy.Add(GetCandy(candy.pos + new Vector2Int(1, -1)));
                }
                else if (side[3] && CheckCandy(candy.pos + new Vector2Int(1, 1), candy.type))
                {
                    square = true;
                    matchCandy.Add(GetCandy(candy.pos + Vector2Int.up));
                    matchCandy.Add(GetCandy(candy.pos + new Vector2Int(1, 1)));
                }
                if (square) matchCandy.Add(GetCandy(candy.pos + Vector2Int.right));
            }

            if (matchCandy.Count > 0) matchCandy.Add(candy);
            matchCandy = matchCandy.Distinct().ToList();
            foreach (var cd in matchCandy)
            {
                candies[cd.pos.x, cd.pos.y] = null;
                candyPool.Add(cd);
                cd.gameObject.SetActive(false);

            }

            if (square)
            {
                candy.gameObject.SetActive(true);
                candy.GetComponent<Image>().sprite = candySprites[CandyType.white];
                candy.type = CandyType.white;
            }

            return vertical || horizontal || square;
        }


        /*
         * 준비
         * 플레이어 입력 대기
         *      스왑체크
         * 
         * 애니메이션
         * 
         */

        IEnumerator GameRoutine()
        {

            while (true)
            {
                if (behaviour.Count > 0)
                {
                    playerBehaviour = false;
                    while(behaviour.Count > 0)
                        yield return behaviour.Dequeue();

                    //Drop Candy

                }
                else
                {
                    playerBehaviour = true;
                    yield return null;
                }
            }
        }

        /*
         * 사탕의 위치 교환
         * 매치되지 않으면 원상태로 롤백한다.
         */
        static internal void SwapCandy(Candy candy, Vector2 dir)
        {
            if ((dir.x < 0 && candy.pos.x == 0) || (dir.y < 0 && candy.pos.y == 0) || (dir.x > 8 && candy.pos.x == 8) || (dir.y > 8 && candy.pos.y == 8) || candies[candy.pos.x + (int)dir.x, candy.pos.y + (int)dir.y] == null)
                return;

            if (playerBehaviour)
            {
                behaviour.Enqueue(SwapCandyRoutine(candy, dir));
                behaviour.Enqueue(CheckSwapMatch(candy, dir));
            }
        }

        static IEnumerator SwapCandyRoutine(Candy candy, Vector2 dir)
        {
            Candy targetCandy = candies[candy.pos.x + (int)dir.x, candy.pos.y + (int)dir.y];
            candy.transform.SetParent(boardTransform);
            targetCandy.transform.SetParent(boardTransform);

            Vector2 startPos = candy.transform.position;
            Vector2 targetPos = targetCandy.transform.position;
            Vector2 moveAmount = targetPos - startPos;

            float moveTime = .3f;
            float cTime = moveTime;
            while (cTime > 0)
            {
                candy.transform.Translate((moveAmount / moveTime) * Time.deltaTime);
                targetCandy.transform.Translate((-moveAmount / moveTime) * Time.deltaTime);
                yield return null;
                cTime -= Time.deltaTime;
            }
            candy.transform.position = targetPos;
            targetCandy.transform.position = startPos;
            Vector2Int tmpPos = candy.pos;
            candy.pos = targetCandy.pos;
            targetCandy.pos = tmpPos;
            candies[candy.pos.x, candy.pos.y] = candy;
            candies[targetCandy.pos.x, targetCandy.pos.y] = targetCandy;
        }

        static IEnumerator CheckSwapMatch(Candy candy, Vector2 dir)
        {
            dir *= -1;
            Candy targetCandy = candies[candy.pos.x + (int)dir.x, candy.pos.y + (int)dir.y];
            bool check = CheckMatch(candy);
            check = CheckMatch(targetCandy) || check;
            if (check == false)
            {
                behaviour.Enqueue(SwapCandyRoutine(candy, dir));
            }
            else yield return null;
        }
    }

}