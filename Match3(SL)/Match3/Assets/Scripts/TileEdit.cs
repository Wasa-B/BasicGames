using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SweetRoad
{

    [ExecuteInEditMode]
    public class TileEdit : MonoBehaviour
    {
        Tile tile;
        static Dictionary<CandyType, Sprite> candyInfo;
        static Dictionary<TileType, Sprite> tileInfo; 
        public TileType type = TileType.Normal;
        public CandyData candyData;
        public TileData tileData;
        public bool defaultCandy = true;
        public CandyType candyType = CandyType.red;
        public SpecialType specialType = SpecialType.None;
        // Start is called before the first frame update
        private void OnValidate()
        {
            if(tile == null)
            {
                tile = GetComponent<Tile>();
            }
            if(tileInfo == null)
            {
                tileInfo = tileData.GetDictionary();
            }
            switch (type)
            {
                case TileType.Normal:
                    tile.GetComponent<Image>().color = Color.white;
                    if (candyData != null)
                    {
                        if (candyInfo == null) candyInfo = candyData.GetDictionary();
                        tile.defaultCandy = defaultCandy;
                        if (defaultCandy)
                        {
                            tile.GetComponent<Image>().sprite = candyInfo[candyType];
                            tile.candy = candyType;
                        }
                    }
                    break;
                case TileType.Blank:
                    tile.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    tile.GetComponent<Image>().sprite = null;
                    tile.defaultCandy = false;
                    break;
                case TileType.Hole:
                    tile.GetComponent<Image>().sprite = tileInfo[type];
                    tile.defaultCandy = false;
                    break;

            }
            tile.type = type;
        }
    }

}