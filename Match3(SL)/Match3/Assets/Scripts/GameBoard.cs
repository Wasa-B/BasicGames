using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SweetRoad
{

    public class GameBoard : MonoBehaviour
    {
        enum GameState { Idle, Explosion, Drop, Match }
        GameState state = GameState.Idle;
        Queue<IEnumerator> behaviour = new Queue<IEnumerator>();

        internal bool playerBehaviour = false;
        Candy[,] candies = new Candy[9, 9];
        Queue<Candy> candyPool = new Queue<Candy>();
        Queue<Candy> mergeCandy = new Queue<Candy>();
        internal Tile[,] tiles = new Tile[9, 9];
        Transform boardTransform;



        public Transform tileLayout;
        public Candy candyPrefab;
        public CandyType[] candyList;
        public Image tileOverImg;
        public CandyData candyData;
        public TileData tileData;
        public Sprite[] normalTiles;

        Dictionary<TileType, Sprite> tileSprites;
        Dictionary<CandyType, Sprite> candySprites;
        Dictionary<SpecialType, Sprite> spCandySprites;
        Vector2[,] tilePosition = new Vector2[9, 9];
        List<Candy> dropCandies;
        List<Candy> effectCandy = new List<Candy>();
        Vector2Int boardSize = new Vector2Int(8, 8);

        internal System.Action swapCandyAction;

        private void Awake()
        {
            boardTransform = transform;
            candies = new Candy[9, 9];
            tiles = new Tile[9, 9];
            candySprites = candyData.GetDictionary();
            spCandySprites = candyData.GetSPDictionary();
            tileSprites = tileData.GetDictionary();
            dropCandies = new List<Candy>();
            Vector2 cellSize = GetComponentInChildren<GridLayoutGroup>().cellSize;
            for (int i = 0; i < tileLayout.childCount; ++i)
            {
                Tile tile = tileLayout.GetChild(i).GetComponent<Tile>();
                tiles[i % 9, i / 9] = tile;

                if (tile.type == TileType.Blank)
                {
                    tile.GetComponent<Image>().sprite = null;
                    tile.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }
                else
                {
                    tile.GetComponent<Image>().sprite = normalTiles[i % 2];
                    if (tile.type == TileType.Hole)
                    {
                        Image tImg = Instantiate<Image>(tileOverImg, tile.transform);
                        tImg.sprite = tileSprites[TileType.Hole];
                    }
                }
                if (tile.defaultCandy == false) continue;

                Candy ncandy = GenerateCandy(new Vector2Int(i % 9, i / 9), tile.candy);
                ncandy.transform.SetParent(tile.transform);
                ncandy.transform.localPosition = Vector2.zero;
            }


            StartCoroutine(BoardRoutin());
        }
        void ReturnPool(Candy candy)
        {
            if (candies[candy.pos.x, candy.pos.y] == candy)
                candies[candy.pos.x, candy.pos.y] = null;
            candyPool.Enqueue(candy); 
            candy.transform.position = Vector2.zero; 
        }

        Candy GenerateCandy(Vector2Int pos, CandyType type, SpecialType specialType = SpecialType.None)
        {
            Candy ncandy;

            if (candyPool.Count > 0)
            {
                ncandy = candyPool.Dequeue();
                ncandy.GetComponent<Image>().color = Color.white;
            }
            else
            {
                ncandy = Instantiate<Candy>(candyPrefab, boardTransform);
            }

            ncandy.returnPool = ReturnPool;
            ncandy.pos = new Vector2Int(pos.x, pos.y);
            candies[pos.x, pos.y] = ncandy;
            ncandy.type = type;
            ncandy.specialType = specialType;
            ncandy.GetComponent<Image>().sprite = candySprites[type];

            switch (specialType)
            {
                case SpecialType.None:
                    ncandy.spImg.color = Color.clear;
                    break;
                case SpecialType.Striped_V:
                case SpecialType.Striped_H:
                case SpecialType.Wrapped:
                    ncandy.spImg.color = Color.white;
                    ncandy.spImg.sprite = spCandySprites[specialType];
                    break;
                case SpecialType.Rainbow:
                    ncandy.type = CandyType.rainbow;
                    ncandy.spImg.color = Color.clear;
                    ncandy.GetComponent<Image>().sprite = candySprites[CandyType.rainbow];
                    break;
                case SpecialType.White:
                    ncandy.type = CandyType.white;
                    ncandy.GetComponent<Image>().sprite = candySprites[CandyType.white];
                    ncandy.spImg.color = Color.clear;
                    break;
            }

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

        internal bool OnBoardPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x <= boardSize.x && y <= boardSize.y;
        }
        internal bool OnBoardPosition(Vector2Int pos)
        {
            return OnBoardPosition(pos.x, pos.y);
        }
        internal Vector2 GetPosition(Vector2Int pos)
        {
            return tilePosition[pos.x, pos.y];
        }
        Candy GetCandy(Vector2Int pos)
        {
            return candies[pos.x, pos.y];
        }
        CandyType RandomCandy()
        {
            return candyList[Random.Range(0, candyList.Length)];
        }
        bool CheckCandy(int x, int y, CandyType type)
        {

            if ( OnBoardPosition(x,y) == false || candies[x, y] == null ||candies[x,y].state != CandyState.Idle) return false;
            if (candies[x, y].type == type) return true;
            return false;
        }
        bool CheckCandy(Vector2Int pos, CandyType type)
        {
            return CheckCandy(pos.x, pos.y, type);
        }

        Vector2Int RandomDir()
        {
            Vector2Int[] vector2s = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            return vector2s[Random.Range(0, vector2s.Length)];
        }

        List<Candy> MatchLine(Candy candy, bool vertical)
        {
            List<Candy> tmpList = new List<Candy>();
            int x = 1;
            int y = 0;
            if (vertical)
            {
                x = 0;
                y = 1;
            }
            for (int d = -2; d <= 2; d++)
            {
                if (d == 0) continue;
                if (d == 2 && tmpList.Count == 0)
                {
                    tmpList.Clear();
                    break;
                }
                Vector2Int pos = new Vector2Int(candy.pos.x + x * d, candy.pos.y + y * d);
                if (CheckCandy(candy.pos.x + x * d, candy.pos.y + y * d, candy.type))
                {
                    tmpList.Add(GetCandy(pos));
                }
                else if (tmpList.Count < 2)
                {
                    tmpList.Clear();
                    continue;
                }
                else break;
            }
            return tmpList;
        }

        List<Candy> MatchSquare(Candy candy)
        {
            List<Candy> tmpList = new List<Candy>();
            for (int i = 0; i < 4; ++i)
            {
                int[] x = { 1, 1, -1, -1 };
                int[] y = { 1, -1, 1, -1 };
                Vector2Int[] v = { Vector2Int.up, Vector2Int.right, Vector2Int.one };
                Vector2Int pos = candy.pos;
                for (int j = 0; j < 3; ++j)
                {
                    Vector2Int d = v[j];
                    d.x *= x[i];
                    d.y *= y[i];
                    if (CheckCandy(pos + d, candy.type))
                    {
                        tmpList.Add(GetCandy(pos + d));
                    }
                    else
                    {
                        tmpList.Clear();
                        break;
                    }
                }
                if (tmpList.Count > 0) break;
            }
            return tmpList;
        }


        SpecialType MatchCandy(Candy candy, out List<Candy> result)
        {

            SpecialType type = SpecialType.None;
            int horizontal = 0;
            int vertical = 0;
            bool square = false;
            result = new List<Candy>();

            List<Candy> tmpList = MatchLine(candy, true);
            vertical = tmpList.Count;
            result.AddRange(tmpList);
            tmpList = MatchLine(candy, false);
            horizontal = tmpList.Count;
            result.AddRange(tmpList);
            tmpList = MatchSquare(candy);
            square = tmpList.Count > 0;
            result.AddRange(tmpList);
            if(result.Count > 0)
                result.Add(candy);
            result = result.Distinct().ToList();
            if (horizontal == 4 || vertical == 4) type = SpecialType.Rainbow;
            else if (horizontal >= 2 && vertical >= 2) type = SpecialType.Wrapped;
            else if (horizontal == 3) type = SpecialType.Striped_V;
            else if (vertical == 3) type = SpecialType.Striped_H;
            else if (square) type = SpecialType.White;

            return type;
        }

        bool CheckMatch(Candy candy, bool drop = false)
        {
            if (candy.state != CandyState.Idle||candy.type == CandyType.white || candy.type == CandyType.rainbow) return false;

            List<Candy> matchCandy;
            SpecialType genType = MatchCandy(candy, out matchCandy);
            Vector2Int pos = candy.pos;


            if (matchCandy.Count > 0) matchCandy.Add(candy);

            if (drop)
            {
                List<Candy> tmpList;
                List<Vector2Int> visit = new List<Vector2Int>();
                bool maxMatch = false;

                while (maxMatch == false)
                {
                    maxMatch = true;
                    foreach (Candy cd in matchCandy)
                    {
                        if (visit.Contains(cd.pos)) continue;
                        else visit.Add(cd.pos);

                        SpecialType tmpType = MatchCandy(cd, out tmpList);
                        if (tmpList.Count > matchCandy.Count)
                        {
                            genType = tmpType;
                            maxMatch = false;
                            matchCandy = tmpList;
                            pos = cd.pos;
                            Debug.Log(tmpList.Count);
                        }
                    }
                }
            }



            foreach (var cd in matchCandy)
            {
                UseCandy(cd.pos, RandomDir());
            }

            if (genType != SpecialType.None)
            {
                Candy nCandy = GenerateCandy(pos, candy.type, genType);
                candies[pos.x, pos.y] = nCandy;
                nCandy.transform.position = GetPosition(pos);
                nCandy.state = CandyState.Disable;
                mergeCandy.Enqueue(nCandy);
            }




            return matchCandy.Count > 0;
        }
        internal void UseCandy(Vector2Int pos, Vector2 dir)
        {
            if ( OnBoardPosition(pos) == false|| candies[pos.x, pos.y] == null||candies[pos.x,pos.y].state != CandyState.Idle) return;

            Candy candy = candies[pos.x, pos.y];
            candies[pos.x, pos.y] = null;
            candy.Use(dir);
            effectCandy.Add(candy);

        }



        internal IEnumerator BoardRoutin()
        {

            while (true)
            {
                //Board 에서 처리해야할 행동이 있을때,
                if (behaviour.Count > 0)
                {
                    playerBehaviour = false;
                    while (behaviour.Count > 0)
                        yield return behaviour.Dequeue();

                    yield return WaitCandyState(effectCandy, CandyState.Explode, true);
                    CandyUsable();
                    //Drop Candy
                    while (DropCandy())
                    {
                        yield return WaitCandyState(dropCandies, CandyState.Drop);
                        CandyUsable();
                        //이동한 사탕 매칭확인
                        foreach (var candy in dropCandies)
                            CheckMatch(candy, true);
                        yield return WaitCandyState(effectCandy, CandyState.Explode, true);
                        dropCandies.Clear();
                        
                    }
                }
                else
                //Board 에서 처리해야할 행동이 없을때, 유저 입력 대기
                {
                    playerBehaviour = true;
                    yield return null;
                }
            }
        }

        internal void CandyUsable()
        {
            while (mergeCandy.Count > 0)
                mergeCandy.Dequeue().state = CandyState.Idle;
        }

        IEnumerator WaitCandyState(List<Candy> candies, CandyState state, bool clear = false)
        {
            for (int i = 0; i < candies.Count; i++)
                yield return new WaitUntil(() => candies[i].state != state);
            if (clear) candies.Clear();
        }

        /*
         * 사탕의 위치 교환
         * 매치되지 않으면 원상태로 롤백한다.
         */
        internal void SwapCandy(Candy candy, Vector2 dir)
        {
            if ((dir.x < 0 && candy.pos.x == 0) || (dir.y < 0 && candy.pos.y == 0) || (dir.x > 8 && candy.pos.x == 8) || (dir.y > 8 && candy.pos.y == 8))
                return;

            if (playerBehaviour)
            {
                if (candy.specialType == SpecialType.White || candy.specialType == SpecialType.Rainbow)
                {
                    UseCandy(candy.pos, dir);
                    swapCandyAction();
                    behaviour.Enqueue(null);
                }
                else if (candies[candy.pos.x + (int)dir.x, candy.pos.y + (int)dir.y] != null)
                {
                    behaviour.Enqueue(SwapCandyRoutine(candy, dir));
                    behaviour.Enqueue(CheckSwapMatch(candy, dir));
                }

            }
            return;
        }

        IEnumerator SwapCandyRoutine(Candy candy, Vector2 dir)
        {
            Candy targetCandy = candies[candy.pos.x + (int)dir.x, candy.pos.y + (int)dir.y];

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

        IEnumerator CheckSwapMatch(Candy candy, Vector2 dir)
        {
            dir *= -1;
            Candy targetCandy = candies[candy.pos.x + (int)dir.x, candy.pos.y + (int)dir.y];
            bool check = CheckMatch(candy);
            check = CheckMatch(targetCandy) || check;
            if (check == false)
            {
                behaviour.Enqueue(SwapCandyRoutine(candy, dir));
            }
            else
            {
                if (swapCandyAction != null) swapCandyAction();
                yield return null;
            }
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
                                //새로운 캔디 드롭
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
                                        nCandy.dropPosition = tilePosition[x, y2] + Vector2.up * sizeY;
                                        nCandy.GetComponent<Image>().color = Color.clear;
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