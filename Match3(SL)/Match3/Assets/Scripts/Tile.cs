using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetRoad
{

    public enum TileType { Normal, Blank, Hole };
    
    [RequireComponent(typeof(TileEdit))]
    public class Tile : MonoBehaviour
    {
        
        public TileType type = TileType.Normal;

        public GameObject block;
        
    }
}