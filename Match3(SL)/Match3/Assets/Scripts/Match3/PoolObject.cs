using UnityEngine;

namespace Match3
{
    public class PoolObject : MonoBehaviour
    {
        System.Action<int, PoolObject> returnPool;
        int prefabPath;

        internal void SetPool(int prefab, System.Action<int,PoolObject> returnPool)
        {
            this.prefabPath = prefab;
            this.returnPool = returnPool;
        }

        internal virtual void Initialize() {

        }

        protected virtual void RetunPool()
        {
            returnPool(prefabPath,this);
        }
    }
}