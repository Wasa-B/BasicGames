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
        public TileType type = TileType.Normal;
        public CandyData candyData;
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

            switch (type)
            {
                case TileType.Normal:
                    if (candyData != null)
                    {
                        if (candyInfo == null) candyInfo = candyData.GetDictionary();
                        tile.defaultCandy = defaultCandy;
                        if (defaultCandy)
                        {
                            tile.GetComponent<Image>().sprite = candyInfo[candyType];
                            tile.candy = candyType;
                        }
                        else
                        {

                        }
                        
                        
                    }
                    break;
            }
            
            tile.type = type;
            tile.defaultCandy = defaultCandy;
        }
    }

}