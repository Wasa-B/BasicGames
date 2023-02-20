using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetRoad
{
    public enum CandyType {orange, green, blue, red, violet, white, rainbow};
    public enum SpecialType {None, Striped_V, Striped_H, Wrapped};
    public class Candy : MonoBehaviour
    {
        internal CandyType type;
        internal SpecialType specialType = SpecialType.None;

        private void Start()
        {
            if(specialType != SpecialType.None)
            {
                Debug.Log(specialType.ToString());
            }
        }
    }
}