using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetRoad
{
    [ExecuteInEditMode]
    public class TileEdit : MonoBehaviour
    {
        Tile tile;
        public TileType type = TileType.Normal;

        // Start is called before the first frame update
        private void OnValidate()
        {
            if(tile == null)
            {
                tile = GetComponent<Tile>();
            }
            tile.type = type;
        }
    }

}