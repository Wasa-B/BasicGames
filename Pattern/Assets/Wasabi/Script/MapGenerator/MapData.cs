using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wasabi.Map
{


    [CreateAssetMenu(fileName = "Data", menuName = "Map/Data")]
    public class MapData : ScriptableObject
    {
        [SerializeField]
        Vector2Int _size;
        [SerializeField]
        string[] data;

        internal string[] GetData() => data;
    }

}