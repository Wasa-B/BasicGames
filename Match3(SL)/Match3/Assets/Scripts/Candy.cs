using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SweetRoad
{

    public enum CandyType {green, blue, red, violet, white, rainbow};
    public enum SpecialType {None, Striped_V, Striped_H, Wrapped,Rainbow,White};
    public enum CandyState {Idle, Explode, Drop, Disable}
    [RequireComponent(typeof(SwapManager))]
    public class Candy : MonoBehaviour, ISwap
    {
        public ExplosionEffect explosionEffect;
        public Bullet whiteCandyRolling;
        public Bullet stripedEffect;
        internal CandyType type;
        internal SpecialType specialType = SpecialType.None;
        internal Vector2Int pos;
        internal CandyState state = CandyState.Idle;
        internal Vector2 dropPosition;

        [SerializeField]
        internal Image spImg;

        
        float acc = 1;

        internal System.Action<Candy> returnPool = null;

        internal void SetCandy(CandyType _type, SpecialType _specialType = SpecialType.None)
        {
            type = _type;
            specialType = _specialType;
        }

        public void OnSwap(Vector2 dir)
        {
            GameManager.SwapCandy(this, dir);
        }


        private void FixedUpdate()
        {
            if (state == CandyState.Drop) Drop();

        }

        internal void Drop()
        {
            Vector2 dir = GameManager.GetPosition(pos) - (Vector2)transform.position;
            if (dropPosition.y > transform.position.y) GetComponent<Image>().color = Color.white;
            if(dir.y > 0)
            {
                acc = 1;
                state = CandyState.Idle;
                transform.position = GameManager.GetPosition(pos);
            }
            else
            {
                acc += acc * 1.00f * Time.fixedDeltaTime;
                transform.Translate(dir.normalized* (GameManager.GetCandySize().y * acc)* 5f * Time.fixedDeltaTime);

                dir = GameManager.GetPosition(pos) - (Vector2)transform.position;
                if(dir.y > 0)
                {
                    acc = 1;
                    state = CandyState.Idle;
                    transform.position = GameManager.GetPosition(pos);
                }
            }
        }
        
        internal void Use(Vector2 dir)
        {
            state = CandyState.Explode;
            StartCoroutine(UseCandy(dir));
        }

        IEnumerator UseCandy(Vector2 dir)
        {
            
            switch (specialType)
            {
                case SpecialType.White:
                    yield return WhiteCandy(dir);
                    break;
                case SpecialType.Wrapped:

                    for(int x = -2; x <= 2; x++)
                    {
                        for(int y = -2; y <= 2; y++)
                        {
                            if ((x == 0 && y == 0)|| (x*x + y*y)>4) continue;
                            GameManager.UseCandy(pos + new Vector2Int(x, y),dir);
                        }
                    }
                    yield return Explosion();
                    break;
                case SpecialType.Striped_V:
                    yield return StripedCandy();
                    break;
                case SpecialType.Striped_H:
                    yield return StripedCandy();
                    break;
                default:
                    yield return Explosion();
                    break;
            }
            state = CandyState.Idle;
            
            returnPool(this);
            transform.position = Vector2.zero;
        }



        IEnumerator StripedCandy()
        {
            yield return Explosion();
            GetComponent<Image>().color = Color.clear;
            spImg.color = Color.clear;
            List<Bullet> stripedBullet = new List<Bullet>();
            Vector2Int dir = specialType == SpecialType.Striped_H ? Vector2Int.right: Vector2Int.up;

            for(int i = -1; i < 2; i += 2)
            {
                Bullet nb = Instantiate<Bullet>(stripedEffect, transform.parent);
                nb.transform.position = GameManager.GetPosition(pos);
                nb.dir = dir*i;
                nb.pos = pos;
                nb.GetComponent<Image>().color = CandyColor();
                stripedBullet.Add(nb);
            }
            
            foreach (Bullet b in stripedBullet)
                yield return new WaitUntil(()=>b.moveEnd);

            foreach (Bullet b in stripedBullet)
                Destroy(b.gameObject);
            stripedBullet.Clear();

        }

        IEnumerator WhiteCandy(Vector2 dir)
        {
            GetComponent<Image>().color = Color.clear;

            Bullet whiteCandy = Instantiate<Bullet>(whiteCandyRolling, transform.parent);
            whiteCandy.transform.position = GameManager.GetPosition(pos);
            whiteCandy.dir = Vector2Int.right *(int)dir.x + Vector2Int.up *(int)dir.y;
            whiteCandy.pos = pos;

            if (dir.x < 0)
                whiteCandy.transform.rotation = Quaternion.Euler(0,0,180);
            else if(dir.x == 0)
            {
                whiteCandy.transform.rotation = Quaternion.Euler(0, 0, dir.y < 0? 90: -90);
            }
            
            yield return new WaitUntil(()=> whiteCandy.moveEnd);
            
            Destroy(whiteCandy.gameObject);
        }

        IEnumerator Explosion()
        {
            GetComponent<Image>().color = Color.clear;
            spImg.color = Color.clear;
            ExplosionEffect ne = Instantiate<ExplosionEffect>(explosionEffect, transform.parent);
            ne.transform.position = transform.position;
            ne.transform.SetSiblingIndex(1);
            
            ne.GetComponent<Image>().color = CandyColor();
            
            yield return new WaitUntil(() => ne.effectEnd);
            Destroy(ne.gameObject);
        }

        Color CandyColor()
        {
            switch (type)
            {
                case CandyType.green:
                    return Color.green;
                case CandyType.blue:
                    return Color.blue;
                case CandyType.red:
                    return Color.red;
                case CandyType.violet:
                    return Color.magenta;
                case CandyType.white:
                case CandyType.rainbow:
                    return Color.white;
                default:
                    return Color.white;
            }
        }
    }
}