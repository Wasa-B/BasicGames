using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wasabi.Map
{


    public class MapDrawer : MonoBehaviour
    {

        public MapData mapData;
        public Vector2Int startPos;
        public GameObject block;
        private void Awake()
        {
            Draw(startPos);
        }

        void Draw(Vector2Int startPos, string mapName = "newMap")
        {
            GameObject map = new GameObject(mapName);
            Vector2Int pos = startPos;
            for (int i = 0; i < mapData.GetData().Length; i++)
            {
                foreach (char c in mapData.GetData()[i])
                {
                    switch (c)
                    {
                        case '#':
                            GameObject tile =Instantiate(block, ((Vector3Int)pos), new Quaternion(),map.transform);
                            tile.AddComponent<BoxCollider2D>().usedByComposite = true;
                            break;
                        default:
                            break;
                    }
                    pos.x += 1;
                }
                pos.x = startPos.x;
                pos.y -= 1;
            }
            map.AddComponent<CompositeCollider2D>().generationType = CompositeCollider2D.GenerationType.Manual;
            map.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}