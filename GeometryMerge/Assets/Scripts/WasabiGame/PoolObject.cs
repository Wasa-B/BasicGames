using System;
using UnityEngine;


namespace WasabiGame
{
    public class PoolObject : MonoBehaviour
    {
        public System.Action<PoolObject> returnPool;
        internal int poolIndex;
        public virtual void PoolOut()
        {
            
        }
        public virtual void EndObject()
        {
            returnPool?.Invoke(this);
        }
    }

}