using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetRoad
{
    public enum CandyType {green, blue, red, violet, white, rainbow};
    public enum SpecialType {None, Striped_V, Striped_H, Wrapped};
    public enum CandyState {Idle, Explode, Drop, Disable}
    [RequireComponent(typeof(SwapManager))]
    public class Candy : MonoBehaviour, ISwap
    {
        internal CandyType type;
        internal SpecialType specialType = SpecialType.None;
        internal Vector2Int pos;
        internal CandyState state = CandyState.Idle;
        internal Vector2 dropPosition;
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
        void Update()
        {
            if (state == CandyState.Drop) Drop();
        }

        float acc = 300;
        float speed = 130;
        float vel = 130;
        internal void Drop()
        {
            Vector2 dir = GameManager.GetPosition(pos) - (Vector2)transform.position;
            if(dir.y > 0)
            {
                vel = speed;
                state = CandyState.Idle;
                transform.position = GameManager.GetPosition(pos);
            }
            else
            {
              
                transform.Translate(dir.normalized* vel * Time.deltaTime);
                vel += acc * Time.deltaTime;
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