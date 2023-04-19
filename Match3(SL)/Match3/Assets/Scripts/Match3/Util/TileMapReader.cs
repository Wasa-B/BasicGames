using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Wasabi.Utility.Tilemaps
{
    [System.Serializable]
    public class TileMapInfo
    {
        [SerializeField]
        Tilemap tilemap;

        internal BoundsInt bounds { get => tilemap.cellBounds; }
        internal Vector2Int min { get => (Vector2Int)bounds.min; }
        internal Vector2Int max { get => (Vector2Int)bounds.max; }

        internal float weight { get => max.x-min.x;}
        internal float height { get => max.y-min.y;}

        
    }

    public class TileMapReader : MonoBehaviour
    {
        public TileMapInfo tileMapInfo;
        public Tilemap tilemap;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log(tilemap.cellBounds.min);
            Debug.Log(tilemap.cellBounds.max);
            Debug.Log(tilemap.cellBounds.size);
            for (int x = tilemap.cellBounds.min.x; x <= tilemap.cellBounds.max.x; x++)
            {
                for (int y = tilemap.cellBounds.min.y; y <= tilemap.cellBounds.max.y; y++)
                {
                    var pos = new Vector3Int(x, y);
                    var tile = tilemap.GetTile(pos);
                    if (tile)
                    {
                        Debug.Log(tile.name + " : " + pos.ToString() + " : " + tilemap.CellToWorld(pos).ToString());
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}