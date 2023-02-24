using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SweetRoad
{

    public enum TileType { Normal, Blank, Hole };
    
    [RequireComponent(typeof(TileEdit))]
    public class Tile : MonoBehaviour
    {
        
        public TileType type = TileType.Normal;
        public bool dropCandy = false;

        internal  bool defaultCandy = true;
        internal  CandyType candy = CandyType.red;

        GameObject Icon()
        {
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            else
                return null;
        }

    }
}