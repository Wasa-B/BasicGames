using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WasabiGame
{

    public class PoolManager : MonoBehaviour
    {
        static PoolManager instance;
        
        
        Dictionary<int,Queue<PoolObject>> poolQueue = new Dictionary<int,Queue<PoolObject>>();

        private void Awake()
        {
            instance = this;
        }


        T Generate<T>(T prefab) where T : PoolObject
        {
            var id = prefab.GetInstanceID();
            //Debug.Log($"Generate {id}");
            ID_Check(id);
            if(poolQueue[id].Count > 0) {
                var obj = poolQueue[id].Dequeue() as T;
                obj.gameObject.SetActive(true);
                obj.PoolOut();
                return obj;
            }
            else
            {
                var obj = Instantiate(prefab);
                obj.returnPool = ReturnPool;
                obj.poolIndex = id;
                return obj;
            }
        }

        void ReturnPool(PoolObject poolObject)
        {
            var id = poolObject.poolIndex;
            //Debug.Log($"ReturnPool {id}");
            poolObject.transform.position = Vector2.left * 1000;
            ID_Check(id);
            poolQueue[id].Enqueue(poolObject);

            poolObject.transform.SetParent(transform);
            poolObject.gameObject.SetActive(false);
        }
        
        void ID_Check(int id)
        {
            if (poolQueue.ContainsKey(id) == false)
                poolQueue.Add(id, new Queue<PoolObject>());
        }

        void Clear()
        {
            var keys = poolQueue.Keys;
            foreach (var key in keys)
            {
                var obj = poolQueue[key].Dequeue();
                Destroy(obj);
            }
            poolQueue.Clear();
        }

        static internal void ClearPool()
        {
            instance.Clear();
        }
        static internal T GenerateObject<T>(T prefab) where T : PoolObject
        {
            return instance.Generate(prefab);
        }

        
    }
}