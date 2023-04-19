using System.Collections.Generic;
using UnityEngine;
namespace Match3
{
    public class PoolManager
    {
        Dictionary<int, Queue<PoolObject>> pool = new Dictionary<int, Queue<PoolObject>>();

        public T GetObject<T>(T prefab, System.Func<T, T> instantiate) where T : PoolObject
        {
            T pobj;
            var code = prefab.GetInstanceID();

            if (pool.ContainsKey(code) == false)
                pool.Add(code, new Queue<PoolObject>());

            if (pool[code].Count > 0)
            {
                pobj = pool[code].Dequeue() as T;
                pobj.Initialize();
            }
            else
            {
                pobj = instantiate(prefab);
                pobj.SetPool(code, ReturnPool);
            }
            
            return pobj;
        }

        void ReturnPool(int code, PoolObject poolObject)
        {
            if (pool.ContainsKey(code) == false)
            {
                
            }
            pool[code].Enqueue(poolObject);
        }

    }
}