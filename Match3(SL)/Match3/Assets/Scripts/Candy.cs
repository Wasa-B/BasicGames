using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetRoad
{
    public enum CandyType {green, blue, red, violet, white, rainbow};
    public enum SpecialType {None, Striped_V, Striped_H, Wrapped};
    [RequireComponent(typeof(SwapManager))]
    public class Candy : MonoBehaviour, ISwap
    {
        internal CandyType type;
        internal SpecialType specialType = SpecialType.None;
        internal Vector2Int pos;

        internal void SetCandy(CandyType _type, SpecialType _specialType = SpecialType.None)
        {
            type = _type;
            specialType = _specialType;
        }

        public void OnSwap(Vector2 dir)
        {
            GameManager.SwapCandy(this, dir);
        }

        private void Start()
        {
            if(specialType != SpecialType.None)
            {
                Debug.Log(specialType.ToString());
            }
        }

        IEnumerator UseCandy(Vector2 dir)
        {
            switch (type)
            {
                case CandyType.white:
                    yield return WhiteCandy(dir);
                    break;
                default:
                    break;
            }
        }

        IEnumerator WhiteCandy(Vector2 dir)
        {

            yield return null;
        }
    }
}