using System.Collections.Generic;
using UnityEngine;


namespace WPooling
{


    public class PoolManager : MonoBehaviour
    {
        static public PoolManager instance;

        Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();
        Dictionary<int, int> objects = new Dictionary<int, int>();

        public class PoolObject : MonoBehaviour
        {
            internal event System.Action<GameObject> OnDelete;
        }

        private void Awake()
        {
            instance = this;
        }

        public GameObject GetGameObject(GameObject gameObject)
        {
            var key = gameObject.GetInstanceID();
            if (pools.ContainsKey(key))
            {
                GameObject go;

                if (pools[key].Count > 0) go = pools[key].Dequeue();
                else
                {
                    go = Instantiate(gameObject);
                    objects.Add(go.GetInstanceID(), gameObject.GetInstanceID());
                }

                go.gameObject.SetActive(true);
                
                return go;
            }
            else
            {
                pools.Add(gameObject.GetInstanceID(), new Queue<GameObject>());

                var go = Instantiate(gameObject);
                objects.Add(go.GetInstanceID(), gameObject.GetInstanceID());
                return go;
            }
        }

        public void ReturnObject(GameObject gameObject)
        {
            var key = objects.ContainsKey(gameObject.GetInstanceID()) ? objects[gameObject.GetInstanceID()] : -1;
            if (key != -1)
            {
                pools[key].Enqueue(gameObject);
                gameObject.SetActive(false);
            }
            else Destroy(gameObject);
        }
    }
}