using UnityEngine;

namespace WasabiGame
{
    [System.Serializable]
    public class TriggerVarialbe<T>
    {
        [SerializeField]
        T data;
        public T Value { get => data; set { data = value; Update?.Invoke(data); } }
        public event System.Action<T> Update;

        public static implicit operator T(TriggerVarialbe<T> a) => a.Value;
    }
}