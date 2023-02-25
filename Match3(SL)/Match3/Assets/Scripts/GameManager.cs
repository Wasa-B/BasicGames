using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SweetRoad
{


    public class GameManager : MonoBehaviour
    {
        enum GameState { Idle, Explosion, Drop, Match }
        GameState state = GameState.Idle;
        static Queue<IEnumerator> behaviour = new Queue<IEnumerator>();

        static bool playerBehaviour = false;
        static Candy[,] candies = new Candy[9, 9];
        static List<Candy> candyPool = new List<Candy>();
        static Tile[,] tiles = new Tile[9, 9];
        static Transform boardTransform;



        public Transform tileLayout;
        public Candy candyPrefab;
        public CandyType[] candyList;
        public CandyData candyData;
        public TileData tileData;
        public Sprite[] normalTiles;
        Dictionary<TileType, Sprite> tileSprites;
        static Dictionary<CandyType, Sprite> candySprites;
        static Vector2[,] tilePosition = new Vector2[9, 9];
        List<Candy> dropCandies;

        private void Awake()
        {
            boardTransform = transform;
            candies = new Candy[9, 9];
            tiles = new Tile[9, 9];
            candySprites = candyData.GetDictionary();
            tileSprites = tileData.GetDictionary();
            dropCandies = new List<Candy>();
            Vector2 cellSize = GetComponentInChildren<GridLayoutGroup>().cellSize;
            for (int i = 0; i < tileLayout.childCount; ++i)
            {
                Tile tile = tileLayout.GetChild(i).GetComponent<Tile>();
                tiles[i % 9, i / 9] = tile;

                if (tile.type == TileType.Normal || tile.defaultCandy == false)
                    tile.GetComponent<Image>().sprite = normalTiles[i % 2];
                if (tile.defaultCandy == false) continue;

                Candy ncandy = GenerateCandy(new Vector2Int(i % 9, i / 9), tile.candy);
                ncandy.transform.SetParent(tile.transform);
                ncandy.transform.localPosition = Vector2.zero;
            }


            StartCoroutine(GameRoutine());
        }


        Candy GenerateCandy(Vector2Int pos, CandyType type)
        {
            Candy ncandy = Instantiate<Candy>(candyPrefab, boardTransform);
            ncandy.pos = new Vector2Int(pos.x, pos.y);
            candies[pos.x, pos.y] = ncandy;
            ncandy.type = type;
            ncandy.GetComponent<Image>().sprite = candySprites[type];
            return ncandy;
        }

        private IEnumerator Start()
        {
            yield return null;
            for (int i = 0; i < 9; ++i)
                for (int j = 0; j < 9; ++j)
                {
                    if (candies[i, j] != null)
                        candies[i, j].transform.SetParent(boardTransform);
                    tilePosition[i, j] = tiles[i, j].transform.position;
                }
        }
        static internal Vector2 GetPosition(Vector2Int pos)
        {
            return tilePosition[pos.x, pos.y];
        }
        static Candy GetCandy(Vector2Int pos)
        {
            return candies[pos.x, pos.y];
        }
        CandyType RandomCandy()
        {
            return candyList[Random.Range(0,candyList.Length)];
        }
        static bool CheckCandy(Vector2Int pos, CandyType type)
        {

            if (pos.x < 0 || pos.y < 0 || pos.x > 8 || pos.y > 8 || candies[pos.x, pos.y] == null) return false;
            if (candies[pos.x, pos.y].type == type) return true;
            return false;
        }

        static bool CheckMatch(Candy candy)
        {
            if (candy.type == CandyType.white || candy.type == CandyType.rainbow) return false;
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
                candies[candy.pos.x, candy.pos.y] = candy;
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
                    while (behaviour.Count > 0)
                        yield return behaviour.Dequeue();

                    //Drop Candy
                    while (DropCandy())
                    {
                        bool drop = true;
                        //사탕 이동 대기
                        while (drop)
                        {
                            drop = false;
                            foreach (var dc in dropCandies)
                            {
                                if (dc.state == CandyState.Drop) drop = true;
                            }
                            yield return null;
                        }
                        //이동한 사탕 매칭확인
                        foreach (var dc in dropCandies)
                        {
                            CheckMatch(dc);
                        }
                        dropCandies.Clear();
                    }
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

        bool DropCandy()
        {
            Dictionary<Vector2Int, int> genCount = new Dictionary<Vector2Int, int>();
            float sizeY = tilePosition[0, 0].y - tilePosition[0, 1].y;
            for (int x = 0; x < 8; ++x)
            {
                for (int y = 8; y >= 0; --y)
                {
                    if (tiles[x, y].type == TileType.Normal && candies[x, y] == null)
                    {
                        if (y >= 0)
                        {
                            for (int y2 = y; y2 >= 0; --y2)
                            {
                                if (candies[x, y2] != null)
                                {
                                    candies[x, y] = candies[x, y2];
                                    candies[x, y2] = null;
                                    dropCandies.Add(candies[x, y]);
                                    candies[x, y].state = CandyState.Drop;
                                    candies[x, y].pos = new Vector2Int(x, y);
                                    break;
                                }
                                //일반타일이 아닐 때,
                                else if (y2 == 0 || tiles[x, y2 - 1].type != TileType.Normal)
                                {
                                    if (tiles[x, y2].dropCandy)
                                    {
                                        Vector2Int gpos = new Vector2Int(x, y2);
                                        if (genCount.ContainsKey(gpos) == false)
                                        {
                                            genCount.Add(gpos, 0);
                                        }
                                        genCount[gpos] += 1;

                                        Candy nCandy = GenerateCandy(new Vector2Int(x, y), RandomCandy());
                                        nCandy.transform.position = tilePosition[x, y2] + Vector2.up * sizeY * genCount[gpos];
                                        nCandy.state = CandyState.Drop;
                                        dropCandies.Add(nCandy);
                                    }
                                    break;
                                }

                            }
                        }
                    }
                }
            }
            return dropCandies.Count > 0;
        }
    }

}