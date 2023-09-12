using UnityEngine.Events;
namespace WUtility
{

    [System.Serializable]
    public class Data<T>
    {
        public UnityEvent<T> OnChange;
        T _value;
        public T value
        {
            get { return _value; }
            set { _value = value;
                OnChange?.Invoke(_value);
            }
        }
    }

    
}