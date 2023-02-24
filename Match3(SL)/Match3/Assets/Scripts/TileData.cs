using System.Collections.Generic;
using UnityEngine;


namespace SweetRoad
{
    [CreateAssetMenu(fileName = "TileData", menuName = "SweetRoad/TileData")]
    public class TileData : ScriptableObject
    {
        [System.Serializable]
        public struct TileStatus
        {
            public TileType type;
            public Sprite sprite;
        }

        public List<TileStatus> candyStatus;

        public Dictionary<TileType, Sprite> GetDictionary()
        {
            Dictionary<TileType, Sprite> dic = new Dictionary<TileType, Sprite>();
            foreach (var c in candyStatus)
            {
                dic.Add(c.type, c.sprite);
            }
            return dic;
        }
    }

}